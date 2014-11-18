using CommonDomain.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CartExample.Domain.Carts
{
    public class CartCheckedOut
    {
        public CartCheckedOut()
        {
            this.CheckedOutOn = DateTime.UtcNow;
        }

        public readonly DateTime CheckedOutOn;
    }
}