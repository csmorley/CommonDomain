using CartExample.Domain.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CartExample.Domain
{
    public class ProductRemovedFromCart
    {
        public ProductRemovedFromCart(ProductId sku, int quantity)
        {
            this.Sku = sku;
            this.Quantity = quantity;
        }

        public readonly ProductId Sku;
        public readonly int Quantity;
    }
}