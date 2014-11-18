using CartExample.Domain.Products;
using CommonDomain.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;

namespace CartExample.Domain.Carts
{
    public class CartCheckedOut
    {
        public CartCheckedOut(Dictionary<ProductId, int> products)
        {
            this.CheckedOutOn = DateTime.UtcNow;
            this.Products = new Dictionary<ProductId, int>(products); // make a copy of the data
        }

        public readonly Dictionary<ProductId, int> Products;
        public readonly DateTime CheckedOutOn;
    }
}