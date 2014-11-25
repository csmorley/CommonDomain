using CartExample.Domain.SocialMedia;
using CartExample.Infrastructure;
using CommonDomain.Messaging;
using SimpleInjector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartExample.Mock
{
    public class TweetsExample
    {
        public static void Run(Container container)
        {
            var day = new DateTime(2014, 1, 1);
            var today = DateTime.Now.Date;

            for (var i = 0; day != today; i++ )
            {     
                int tweetsToGenerate;

                if(day.DayOfWeek == DayOfWeek.Thursday || day.DayOfWeek == DayOfWeek.Friday || day.DayOfWeek == DayOfWeek.Saturday )
                    tweetsToGenerate = RandomProvider.Next(300, 500);
                else
                    tweetsToGenerate = RandomProvider.Next(200, 300);

                for (int tweetCount=0; tweetCount < tweetsToGenerate; tweetCount++ )
                {
                    container
                        .GetInstance<IEventPublisher>()
                        .PublishEvent(new EventEnvelope(null,new UserTweet("some userid","their message", day.Date)),false);
                }

                day = day.AddDays(1);
            }
        }
    }
}
