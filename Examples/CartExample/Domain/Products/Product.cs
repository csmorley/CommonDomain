using CommonDomain.Aggregates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CartExample.Domain.Products
{
    public class Product : AggregateBase
    {
        public Product(ProductId id, string description)
        {
            this.Id = id;
            this.Description = description;            
        }

        public ProductId Id { get; protected set; }

        public decimal Price;
        public string Description;

        public override IIdentity GetId()
        {
            return this.Id;
        }
    }
}