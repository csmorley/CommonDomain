using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CommonDomain.Messaging;
using CartExample.Domain.Carts;
using CommonDomain.Aggregates;

namespace CartExample.Projections
{
    public class AverageCheckoutTimeProjection : IEventHandler<CartCreated>, IEventHandler<CartCheckedOut>
    {
        public AverageCheckoutTimeProjection(Database database)
        {
            this.database = database;
        }


        Database database;

        public void Handle(Identity senderId, CartCreated eventToHandle, bool isReplay)
        {
            this.database.StartTimes.Add(eventToHandle.CartId, eventToHandle.CreatedOn);
        }
        public void Handle(Identity senderId, CartCheckedOut eventToHandle, bool isReplay)
        {
            DateTime start;

            if (this.database.StartTimes.TryGetValue(senderId, out start) == true)
            {
                TimeSpan duration = eventToHandle.CheckedOutOn - start;
                this.database.CheckoutDurations.Add(senderId, duration.TotalSeconds);
                this.database.StartTimes.Remove(senderId);
                
                double total = 0;
                int count = this.database.CheckoutDurations.Count;
                
                foreach(var span in this.database.CheckoutDurations.Values)
                {
                    total = total + span;
                }

                this.database.AverageCheckoutTime = total / count;
            }
        }

    }
}