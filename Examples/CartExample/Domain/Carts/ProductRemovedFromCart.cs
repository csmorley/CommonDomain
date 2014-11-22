using CartExample.Domain.Products;
using CommonDomain.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CartExample.Domain
{
    public class ProductRemovedFromCart : IEvent
    {
        public ProductRemovedFromCart(ProductId id, int quantity)
        {
            this.ProductId = id;
            this.Quantity = quantity;
        }

        public readonly ProductId ProductId;
        public readonly int Quantity;
    }
}