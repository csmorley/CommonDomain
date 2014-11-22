using CartExample.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CartExample.Projections
{
    public class Database
    {
        // used for average checkout time projection
        public readonly Dictionary<Guid, DateTime> StartTimes = new Dictionary<Guid, DateTime>();
        public readonly Dictionary<Guid, double> CheckoutDurations = new Dictionary<Guid, double>();        
        public double AverageCheckoutTime;

        // used for adandoned items projection
        public readonly Dictionary<Guid, HashSet<string>> AddedItems = new Dictionary<Guid, HashSet<string>>();
        public readonly Dictionary<Guid, HashSet<string>> CartsWithAbandonedItems = new Dictionary<Guid, HashSet<string>>();
        public double PercentageOfCartsWithAbandonedItems;
        public ulong CheckedOutCount;
        public readonly Dictionary<DateTime, DayCheckoutStatistic> CheckoutsByDate = new Dictionary<DateTime, DayCheckoutStatistic>();

        public readonly Dictionary<DateTime, ulong> TweetsByDate = new Dictionary<DateTime, ulong>();
    }
}