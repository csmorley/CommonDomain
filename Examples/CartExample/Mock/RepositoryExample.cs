using CartExample.Domain.Carts;
using CartExample.Domain.Products;
using CommonDomain.Implementation;
using CommonDomain.Persistence;
using SimpleInjector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartExample.Mock
{
    public class RepositoryExample
    {
        public static void Run(Container container)
        {
            var eventStore = container.GetInstance<GetEventStore>();
            eventStore.Connect();

            var product1 = new Product(new ProductId("11-11-1111"), "Book 1");
            var product2 = new Product(new ProductId("22-22-2222"), "Book 2");
            var product3 = new Product(new ProductId("33-33-3333"), "Book 3");
            var product4 = new Product(new ProductId("44-44-4444"), "Book 4");
            var product5 = new Product(new ProductId("55-55-5555"), "Book 5");
            var product6 = new Product(new ProductId("66-66-6666"), "Book 6");
            var product7 = new Product(new ProductId("77-77-7777"), "Book 7");
            var product8 = new Product(new ProductId("88-88-8888"), "Book 8");
            var products = new List<Product>(new[] { product1, product2, product3, product4, product5, product6, product7, product8 });

            var now = DateTime.UtcNow;
            var testCart = new Cart(new CartId(Guid.NewGuid()), now, now);

            testCart.AddToCart(product1, 1);
            testCart.AddToCart(product1, 1);
            testCart.RemoveFromCart(product1, -1);
            testCart.AddToCart(product1, 10);
            testCart.AddToCart(product2, 10);
            testCart.RemoveFromCart(product2, -10);
            testCart.AddToCart(product3, 5);
            testCart.AddToCart(product4, 3);
            testCart.RemoveFromCart(product4, -2);

            testCart.Checkout();

            var repository = container.GetInstance<IRepository>();
            repository.Save(testCart);
        }
    }
}
