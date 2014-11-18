using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CommonDomain.Messaging;

namespace CartExample.Domain.Carts
{
    public class CartCreated
    {
        public CartCreated(string username, Guid id)
        {
            this.Username = username;
            this.CartId = id;
            this.CreatedOn = DateTime.UtcNow;
        }

        public readonly string Username;
        public readonly Guid CartId;
        public readonly DateTime CreatedOn;
    }
}