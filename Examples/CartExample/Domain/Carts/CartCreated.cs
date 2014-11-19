using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CommonDomain.Messaging;

namespace CartExample.Domain.Carts
{
    public class CartCreated
    {
        public CartCreated(CartId id)
        {
            this.CartId = id;
            this.CreatedOn = DateTime.UtcNow;
        }

        // for testing, to allow injection of date
        public CartCreated(CartId id, DateTime on)
        {
            this.CartId = id;
            this.CreatedOn = on;
        }

        public readonly CartId CartId;
        public readonly DateTime CreatedOn;
    }
}