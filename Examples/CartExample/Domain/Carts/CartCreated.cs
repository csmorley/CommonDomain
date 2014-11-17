using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CommonDomain.Messaging;

namespace CartExample.Domain.Carts
{
    public class CartCreated
    {
        public CartCreated(string username)
        {
            this.Username = username;
            this.CreatedOn = DateTime.UtcNow;
        }

        public readonly string Username;

        public readonly DateTime CreatedOn;
    }
}