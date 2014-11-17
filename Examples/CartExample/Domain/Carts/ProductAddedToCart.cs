using CartExample.Domain.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CommonDomain.Messaging;

namespace CartExample.Domain
{
    public class ProductAddedToCart
    {
        public ProductAddedToCart(ProductId sku, int quantity)
        {
            this.Sku = sku;
            this.Quantity = quantity;
        }

        public readonly ProductId Sku;
        public readonly int Quantity;
    }
}