using CommonDomain.Aggregates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CartExample.Domain.Products
{
    public class Product : AggregateBase
    {
        public Product(Guid id, string description) : base(id)
        {           
            this.Description = description;
        }

        public decimal Price;
        public string Description;
    }
}