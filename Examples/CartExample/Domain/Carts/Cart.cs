﻿using CommonDomain.Aggregates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CartExample.Domain.Products;
using CartExample.Domain.Carts;

namespace CartExample.Domain.Carts
{
    public class Cart : AggregateBase
    {
        Dictionary<ProductId, int> products = new Dictionary<ProductId, int>();
        DateTime mockCheckoutDate; // for testing so date can be injected by test data generator
        public bool IsCheckedOut { get; private set; }

        public CartId Id { get; protected set; } 

        public Cart(CartId id, DateTime mockStartDate, DateTime mockCheckoutDate)
        {
            RaiseEvent(new CartCreated(id, mockStartDate));
            this.mockCheckoutDate = mockCheckoutDate;
        }

        // needed for repository creation
        protected Cart()
        {
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

        public int CountFor(Product product)
        {
            if (!this.products.ContainsKey(product.Id))
                return 0;

            return this.products[product.Id];
        }

        public int CountUniqueProducts()
        {
            return this.products.Keys.Count();
        }

        public void RemoveAllFromCart(Product product)
        {
            AssertCanModifyCart();

            var qty = CountFor(product);

            if (qty > 0)
            {
                RemoveFromCart(product, qty);
            }
        }

        void AssertCanModifyCart()
        {
            if (this.IsCheckedOut == true)
                throw new InvalidOperationException("cart already checked out, cannot be modified");
        }

        public void Checkout()
        {
            RaiseEvent(new CartCheckedOut(this.products, this.mockCheckoutDate));
        }

        private void Apply(ProductAddedToCart e)
        {
            int count = 0;
            this.products.TryGetValue(e.ProductId, out count);
            this.products[e.ProductId] = count + e.Quantity;
        }

        private void Apply(ProductRemovedFromCart e)
        {
            int count = this.products[e.ProductId];

            if (count + e.Quantity == 0)
                this.products.Remove(e.ProductId);
            else
                this.products[e.ProductId] += e.Quantity;
        }

        private void Apply(CartCheckedOut e)
        {
            this.IsCheckedOut = true;
        }

        private void Apply(CartCreated e)
        {
            this.Id = e.CartId;          
        }

        public override IIdentity GetId()
        {
            return this.Id;
        }
    }
}