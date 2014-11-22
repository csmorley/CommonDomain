using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CommonDomain.Messaging;

namespace CartExample.Domain.Carts
{
    public class CartCreated : IEvent
    {
        // for testing, to allow injection of date
        public CartCreated(CartId value, DateTime createdOn)
        {
            this.CartId = value;
            this.CreatedOn = createdOn;
        }

        public readonly CartId CartId;
        public readonly DateTime CreatedOn;
    }
}