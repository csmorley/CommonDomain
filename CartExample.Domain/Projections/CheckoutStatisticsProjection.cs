using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CommonDomain.Mediator;
using CartExample.Domain.Carts;
using CartExample.Domain;
using CommonDomain.Aggregates;

namespace CartExample.Projections
{
    public class DayCheckoutStatistic
    {
        public DayCheckoutStatistic(DateTime day)
        {
            this.Day = day;
        }
        public ulong TotalCarts;
        public ulong CartsThatHadAnAbandonedItem;
        public readonly DateTime Day;
        public double PercentageWithAbandonedItems { get {  return (double)this.CartsThatHadAnAbandonedItem / (double)this.TotalCarts * 100; } }
    }

    public class CheckoutStatisticsProjection : IEventHandler<ProductAddedToCart>, IEventHandler<CartCheckedOut>
    {
        Database database;

        public CheckoutStatisticsProjection(Database database)
        {
            this.database = database;
        }

        public void Handle(Identity senderId, ProductAddedToCart eventToHandle, bool isReplay)
        {
            // create the list if it doesn't exist
            if(this.database.AddedItems.ContainsKey(senderId)==false)
            {
                this.database.AddedItems.Add(senderId, new HashSet<string>());
            }

            var hashset = this.database.AddedItems[senderId];

            // check if already stored as added item
            if (hashset.Contains(eventToHandle.ProductId))
                return;

            hashset.Add(eventToHandle.ProductId);
        }

        public void Handle(Identity senderId, CartCheckedOut eventToHandle, bool isReplay)
        {
            this.database.CheckedOutCount++;

            var hashset = this.database.AddedItems[senderId];

            foreach(var id in eventToHandle.Products.Keys)
            {
                hashset.Remove(id);
            }

            var abandonedItemsInCartCount = (ulong)hashset.Count;
            var checkedOutOn = eventToHandle.CheckedOutOn.Date;

            if (abandonedItemsInCartCount > 0) // we have abandoned items that we need to report
            {
                this.database.CartsWithAbandonedItems.Add(senderId, new HashSet<string>(hashset));
                var cartsWithAbandonedItemsCount = this.database.CartsWithAbandonedItems.Count();
                this.database.PercentageOfCartsWithAbandonedItems = (double)cartsWithAbandonedItemsCount / (double)this.database.CheckedOutCount * 100;
            }

            DayCheckoutStatistic checkoutsForDate;
            if (this.database.CheckoutsByDate.TryGetValue(checkedOutOn, out checkoutsForDate) == false)
            {
                checkoutsForDate = new DayCheckoutStatistic(checkedOutOn) { CartsThatHadAnAbandonedItem = 0, TotalCarts = 0 };
                this.database.CheckoutsByDate.Add(checkedOutOn, checkoutsForDate);
            }

            this.database.CheckoutsByDate[checkedOutOn].TotalCarts++;

            if (abandonedItemsInCartCount > 0) 
                this.database.CheckoutsByDate[checkedOutOn].CartsThatHadAnAbandonedItem++;

            this.database.AddedItems.Remove(senderId); // clear out the temp AddedItems table
        }
    }
}