using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CartExample.Domain.Products
{
    public class Product
    {
        public Product(ProductId id)
        {
            this.Sku = id;
        }

        public readonly ProductId Sku;
        public decimal Price;
        public string Description;
    }
}