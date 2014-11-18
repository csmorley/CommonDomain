using CommonDomain.Aggregates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CartExample.Domain.Products;
using CartExample.Domain.Carts;

namespace CartExample.Domain.Cart
{
    public class Cart : AggregateBase
    {
        Dictionary<Guid, int> products = new Dictionary<Guid, int>();
        public bool IsCheckedOut { get; private set; }
        public string Username {get; private set;}

        public Cart(Guid id, string username) : base(id)
        {
            RaiseEvent(new CartCreated(username, this.Id));
        }

        public void AddToCart(Product product, int quantity)
        {
            AssertCanModifyCart();

            if (quantity < 1)
                throw new InvalidOperationException("quantity must be positive");

            RaiseEvent(new ProductAddedToCart(product.Id, quantity));
        }
        public void RemoveFromCart(Product product, int quantity)
        {
            AssertCanModifyCart();

            if (quantity > -1)
                throw new InvalidOperationException("quantity must be negative");

            if(!this.products.ContainsKey(product.Id))
                throw new InvalidOperationException("can't remove from cart, product not in cart");

            if (this.products[product.Id] - quantity < 0)
                throw new InvalidOperationException("cant remove more than is in cart");            

            RaiseEvent(new ProductRemovedFromCart(product.Id, quantity));
        }

        void AssertCanModifyCart()
        {
            if (this.IsCheckedOut == true)
                throw new InvalidOperationException("cart already checked out, cannot be modified");
        }

        public void Checkout()
        {
            RaiseEvent(new CartCheckedOut(this.Id));
        }

        private void Apply(ProductAddedToCart e)
        {
            int count = 0;
            this.products.TryGetValue(e.Sku, out count);
            this.products[e.Sku] = count + e.Quantity;
        }

        private void Apply(ProductRemovedFromCart e)
        {
            int count = this.products[e.Id];

            if (count + e.Quantity == 0)
                this.products.Remove(e.Id);
            else
                this.products[e.Id] += e.Quantity;
        }

        private void Apply(CartCheckedOut e)
        {
            this.IsCheckedOut = true;
        }

        private void Apply(CartCreated e)
        {
            this.Username = e.Username;               
        }
    }
}