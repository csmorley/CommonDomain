using CartExample.Domain.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CommonDomain.Messaging;

namespace CartExample.Domain
{
    public class ProductAddedToCart : IEvent
    {
        public ProductAddedToCart(ProductId productId, int quantity)
        {
            this.ProductId = productId;
            this.Quantity = quantity;
        }

        public readonly ProductId ProductId;
        public readonly int Quantity;
    }
}