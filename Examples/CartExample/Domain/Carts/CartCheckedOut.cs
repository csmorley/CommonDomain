﻿using CartExample.Domain.Products;
using CommonDomain.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;

namespace CartExample.Domain.Carts
{
    public class CartCheckedOut : IEvent
    {
        public CartCheckedOut(Dictionary<ProductId, int> products)
        {
            this.CheckedOutOn = DateTime.UtcNow;
            this.Products = new Dictionary<ProductId, int>(products); // make a copy of the data
        }

        // for testing, to allow injection of date
        public CartCheckedOut(Dictionary<ProductId, int> products, DateTime on)
        {
            this.CheckedOutOn = on;
            this.Products = new Dictionary<ProductId, int>(products); // make a copy of the data
        }

        public readonly Dictionary<ProductId, int> Products;
        public readonly DateTime CheckedOutOn;
    }
}