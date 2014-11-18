using CommonDomain.Aggregates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CartExample.Domain.Carts
{
    public class CartId : Identity
    {
        public CartId(Guid id)
            : base(id)
        {
        }
    }
}