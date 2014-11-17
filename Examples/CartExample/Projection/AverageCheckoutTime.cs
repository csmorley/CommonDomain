using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CommonDomain.Mediator;
using CartExample.Domain.Carts;

namespace CartExample.Projection
{
    public class AverageCheckoutTime : IEventHandler<CartCreated>, IEventHandler<CartCheckedOut>
    {
        public void Handle(CartCreated eventToHandle, bool isReplay)
        {
            
        }
        public void Handle(CartCheckedOut eventToHandle, bool isReplay)
        {
            
        }

    }
}