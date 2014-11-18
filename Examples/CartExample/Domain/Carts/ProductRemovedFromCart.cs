using CartExample.Domain.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CartExample.Domain
{
    public class ProductRemovedFromCart
    {
        public ProductRemovedFromCart(Guid id, int quantity)
        {
            this.Id = id;
            this.Quantity = quantity;
        }

        public readonly Guid Id;
        public readonly int Quantity;
    }
}