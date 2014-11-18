using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CartExample.Infrastructure
{
    public class Database
    {
        public readonly Dictionary<Guid, DateTime> StartTimes = new Dictionary<Guid, DateTime>();
        public readonly Dictionary<Guid, double> CheckoutDurations = new Dictionary<Guid, double>();        
        public double AverageCheckoutTime;

        public readonly Dictionary<Guid, List<Guid>> CartItems = new Dictionary<Guid, List<Guid>>();
    }
}