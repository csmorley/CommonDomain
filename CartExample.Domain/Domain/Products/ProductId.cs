using CommonDomain.Aggregates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CartExample.Domain.Products
{
    public class ProductId : Identity
    {
        public ProductId(string id) : base(id)
        {
        }
    }
}