using CommonDomain.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartExample.Domain.SocialMedia
{
    class UserTweet : IEvent
    {
        public readonly DateTime Date;
        public UserTweet(string userId, string message, DateTime date)
        {
            this.Date = date;
        }
    }
}
