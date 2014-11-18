using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CommonDomain.Mediator;
using CartExample.Domain.Carts;
using CartExample.Infrastructure;
using CartExample.Domain;

namespace CartExample.Projections
{
    public class AbandonedItems : IEventHandler<ProductAddedToCart>, IEventHandler<ProductRemovedFromCart>, IEventHandler<CartCheckedOut>
    {
        Database database;

        public AbandonedItems(Database database)
        {
            this.database = database;
        }

        public void Handle(ProductAddedToCart eventToHandle, bool isReplay)
        {

        }

        public void Handle(ProductRemovedFromCart eventToHandle, bool isReplay)
        {

        }
        public void Handle(CartCheckedOut eventToHandle, bool isReplay)
        {
        }
    }
}