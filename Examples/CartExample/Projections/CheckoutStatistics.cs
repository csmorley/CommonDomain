using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CommonDomain.Mediator;
using CartExample.Domain.Carts;
using CartExample.Infrastructure;
using CartExample.Domain;
using CommonDomain.Aggregates;

namespace CartExample.Projections
{
    public class CheckoutStatistics : IEventHandler<ProductAddedToCart>, IEventHandler<CartCheckedOut>
    {
        Database database;

        public CheckoutStatistics(Database database)
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
                this.database.CartsWithAbandonedItemsCount++;
                this.database.PercentageOfCartsWithAbandonedItems = (double)this.database.CartsWithAbandonedItemsCount / (double)this.database.CheckedOutCount * 100;
            }

            Tuple<ulong, ulong> checkoutsForDate;
            if (this.database.CheckoutsByDate.TryGetValue(checkedOutOn, out checkoutsForDate) == false)
            {
                checkoutsForDate = new Tuple<ulong, ulong>(0, 0);
                this.database.CheckoutsByDate.Add(checkedOutOn, checkoutsForDate);
            }

            var newCheckoutsCount = checkoutsForDate.Item1 + 1;
            var newAbandonedCount = abandonedItemsInCartCount > 0 ? checkoutsForDate.Item2 + abandonedItemsInCartCount : checkoutsForDate.Item2;
            this.database.CheckoutsByDate[checkedOutOn] = new Tuple<ulong, ulong>(newCheckoutsCount, newAbandonedCount); ;

            this.database.AddedItems.Remove(senderId); // clear out the temp AddedItems table
        }
    }
}