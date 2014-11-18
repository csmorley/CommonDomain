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
    public class AbandonedItems : IEventHandler<ProductAddedToCart>, IEventHandler<CartCheckedOut>
    {
        Database database;

        public AbandonedItems(Database database)
        {
            this.database = database;
        }

        public void Handle(Identity senderId, ProductAddedToCart eventToHandle, bool isReplay)
        {
            // create the list if it doesnt exist
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
            var hashset = this.database.AddedItems[senderId];

            foreach(var id in eventToHandle.Products.Keys)
            {
                hashset.Remove(id);
            }

            this.database.AbandonedItems.Add(senderId, new HashSet<string>(hashset));
        }
    }
}