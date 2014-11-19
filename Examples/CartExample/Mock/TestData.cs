using CartExample.Domain.Carts;
using CartExample.Domain.Products;
using CartExample.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CartExample.Mock
{
    public class TestData
    {
        List<Product> products;

        public TestData(List<Product> products)
        {
            this.products = products;
        }
        public List<Cart> Create()
        {
            var items = new List<Cart>();
            var day = new DateTime(2014, 1, 1);
            var today = DateTime.Now.Date;

            for (var i = 0; day != today; i++ )
            {
                items.AddRange(CreateForDay(day));
                day = day.AddDays(1);
            }
            return items;
        }

        List<Cart> CreateForDay(DateTime date)
        {
            Random random = new Random();
            int numberToCreate;

            if (date.DayOfWeek == DayOfWeek.Friday || date.DayOfWeek == DayOfWeek.Saturday)
            {
                numberToCreate = random.Next(500, 1000);
            }
            else if (date.DayOfWeek == DayOfWeek.Sunday)
            {
                numberToCreate = random.Next(100, 200);
            }
            else
            {
                numberToCreate = random.Next(200, 500);
            }

            var items = new List<Cart>();
            for (var i = 0; i < numberToCreate; i++ )
            {
                items.Add(CreateRandomCart(date));
            }
            return items;
        }

        Cart CreateRandomCart(DateTime date)
        {
            var cart = new Cart(new CartId(Guid.NewGuid()), date, date);
            int steps = RandomProvider.Next(1,10);

            while (cart.CountUniqueProducts() == 0) // loop till we create a valid cart
            {
                for (var i = 0; i < steps; i++)
                {
                    int quantity = RandomProvider.Next(1, 10);

                    var product = this.products[RandomProvider.Next(0, this.products.Count - 1)];

                    // 90% of the time, add
                    var addOperation = RandomProvider.Next(1, 100) < 90 ? true : false;

                    if (addOperation == true)
                    {
                        cart.AddToCart(product, quantity);
                    }
                    else
                    {
                        quantity = cart.CountFor(product) < quantity ? cart.CountFor(product) : quantity;

                        if (quantity > 0) // only remove if there are items in the cart!
                        {
                            cart.RemoveFromCart(product, 0 - quantity);
                        }
                    }

                }
            }

            cart.Checkout();

            if (cart.CountUniqueProducts() == 0)
            {
                Console.WriteLine("hmmm");
            }
            

            return cart;
        }

    }
}