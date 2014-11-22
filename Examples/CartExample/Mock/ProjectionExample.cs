using CartExample.Domain.Products;
using CartExample.Projections;
using CommonDomain.Aggregates;
using CommonDomain.Messaging;
using SimpleInjector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartExample.Mock
{
    public class ProjectionExample
    {
        public static void Run(Container container)
        {
            var product1 = new Product(new ProductId("11-11-1111"), "Book 1");
            var product2 = new Product(new ProductId("22-22-2222"), "Book 2");
            var product3 = new Product(new ProductId("33-33-3333"), "Book 3");
            var product4 = new Product(new ProductId("44-44-4444"), "Book 4");
            var product5 = new Product(new ProductId("55-55-5555"), "Book 5");
            var product6 = new Product(new ProductId("66-66-6666"), "Book 6");
            var product7 = new Product(new ProductId("77-77-7777"), "Book 7");
            var product8 = new Product(new ProductId("88-88-8888"), "Book 8");
            var products = new List<Product>(new[] { product1, product2, product3, product4, product5, product6, product7, product8 });

            var testData = new TestData(products).Generate();

            int count = 0;
            var start = DateTime.UtcNow;
            var publisher = container.GetInstance<IEventPublisher>();
            foreach (var cart in testData)
            {
                var events = (cart as IAggregate).GetUncommittedEvents();

                foreach (var e in events)
                {
                    var envelope = new EventEnvelope(cart.Id, e as IEvent);
                    publisher.PublishEvent(envelope, false);
                    count++;
                }
            }

            var database = container.GetInstance<Database>();
            var duration = DateTime.UtcNow - start;
            var eventsPerSecond = (double)count / duration.TotalSeconds;

            var moreThanOneItemAbandoned = database.CartsWithAbandonedItems.Where(x => x.Value.Count > 1);
        }
    }
}
