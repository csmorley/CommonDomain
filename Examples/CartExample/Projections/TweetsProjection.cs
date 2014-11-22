using CartExample.Domain.SocialMedia;
using CommonDomain.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartExample.Projections
{
    class TweetsProjection : IEventHandler<UserTweet>
    {
        Database database;

        public TweetsProjection(Database database)
        {
            this.database = database;
        }

        public void Handle(CommonDomain.Aggregates.Identity senderId, UserTweet eventToHandle, bool isReplay)
        {
            ulong tweets;

            if(this.database.TweetsByDate.TryGetValue(eventToHandle.Date, out tweets)==false)
            {
                tweets = 0;
            }

            tweets = tweets + 1;

            this.database.TweetsByDate[eventToHandle.Date] = tweets;
        }
    }
}
