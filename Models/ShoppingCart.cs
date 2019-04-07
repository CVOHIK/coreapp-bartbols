using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplicationShop.Data;
using WebApplicationShop.Models;

namespace WebApplicationShop.Models
{
    public partial class ShoppingCart
    {

        //notes
        //ShoppingCart is the collection of ShoppingObjects, linked through the ShoppingcartId, ShoppingObjects do not store to DB
        //Orders and OrderObjects are the same, but DO store to DB
        //top is no longer true 

#pragma warning disable IDE0044 // Add readonly modifier
        ShoppingContext shopDb = new ShoppingContext();
#pragma warning restore IDE0044 // Add readonly modifier

        string ShoppingCartId { get; set; }

        public const string CartSessionKey = "CartId";


        public static ShoppingCart GetCart(HttpContextBase context)
        {
            var cart = new ShoppingCart();
            cart.ShoppingCartId = cart.GetCartId(context);
            return cart;
        }

        // Helper method to simplify shopping cart calls
        public static ShoppingCart GetCart(Controller controller)
        {
            return GetCart(controller.HttpContext);
        }

        public void AddToCart(Product product)
        {
            // Get the matching cart and album instances
            var cartItem = shopDb.Carts.SingleOrDefault(c => c.CartId == ShoppingCartId && c.ProductId == product.ProductId);
            //var cartItem = shopDb.Carts.SingleOrDefault(c => c.CartId == ShoppingCartId && c.ProductId == product.ProductId);

            if (cartItem == null)
            {
                // Create a new cart item if no cart item exists
                cartItem = new Cart
                {
                    ProductId = product.ProductId,
                    CartId = ShoppingCartId,
                    Count = 1,
                    DateCreated = DateTime.Now
                };


                shopDb.Carts.Add(cartItem);
            }
            else
            {
                // If the item does exist in the cart, then add one to the quantity
                cartItem.Count++;
            }

            // Save changes
            shopDb.SaveChanges();
        }

        public int RemoveFromCart(int id)
        {
            // Get the cart
            var cartItem = shopDb.Carts.Single(ShopObject => ShopObject.CartId == ShoppingCartId && ShopObject.ProductId == id);

            int itemCount = 0;

            if (cartItem != null)
            {
                if (cartItem.Count > 1)
                {
                    cartItem.Count--;
                    itemCount = cartItem.Count;
                }
                else
                {
                    shopDb.Carts.Remove(cartItem);
                }

                // Save changes
                shopDb.SaveChanges();
            }

            return itemCount;
        }

        public void EmptyCart()
        {
            var cartItems = shopDb.Carts.Where(ShopObject => ShopObject.CartId == ShoppingCartId);

            foreach (var cartItem in cartItems)
            {
                shopDb.Carts.Remove(cartItem);
            }

            // Save changes
            shopDb.SaveChanges();
        }

        public List<Cart> GetCartItems()
        {
            return shopDb.Carts.Where(cart => cart.CartId == ShoppingCartId).ToList();
        }

        public int GetCount()
        {
            int? count = (from cartItems in shopDb.Carts
                          where cartItems.CartId == ShoppingCartId
                          select (int?)cartItems.Count).Sum();

            // Return 0 if all entries are null
            return count ?? 0;
        }

        public decimal GetUnitPrice()
        {
            decimal? cost;
            if (shopDb.Carts.Where(cart => cart.CartId == ShoppingCartId).ToList().Count != 0)
            {
                var BulkQnt = (from cartItems in shopDb.Carts
                               where cartItems.CartId == ShoppingCartId
                               select cartItems.Product.BulkQnt).Sum();

                if (GetCount() >= BulkQnt)
                {
                    cost = (from cartItems in shopDb.Carts
                            where cartItems.CartId == ShoppingCartId
                            select cartItems.Product.BulkPrice).Sum();
                }
                else
                {
                    cost = (from cartItems in shopDb.Carts
                            where cartItems.CartId == ShoppingCartId
                            select cartItems.Product.BasePrice).Sum();
                }
            }
            else
            {
                cost = 0;
            }
            return cost ?? decimal.Zero;
        }
        public decimal GetTotal()
        {
            decimal? cost = GetUnitPrice();
            if (cost != 0) {
                cost = cost * (from cartItems in shopDb.Carts
                              where cartItems.CartId == ShoppingCartId
                              select cartItems.Count).Sum();
            }
            return cost ?? decimal.Zero;
        }

        //turn what is in the cart into an order
        public int CreateOrder(Order order)
        {
            decimal orderTotal = 0;

            var cartItems = GetCartItems();//Get what is in the cart

            // Iterate over the items in the cart, adding the order for each
            foreach (var cartItem in cartItems)
            {
                decimal RealPrice;
                Boolean isBulk;
                if (cartItem.Count >= cartItem.Product.BulkQnt)
                {
                    RealPrice = cartItem.Product.BulkPrice;
                    isBulk = true;
                }
                else
                {
                    RealPrice = cartItem.Product.BulkPrice;
                    isBulk = false;
                }
                var orderObject = new OrderObject
                {
                    ProductId = cartItem.ProductId,
                    OrderId = order.OrderId,
                    Price = RealPrice,
                    BulkPrice = isBulk,
                    ProductQnt = cartItem.Count
                };

                // Set the order total of the shopping cart
                orderTotal += (orderObject.ProductQnt * orderObject.Price);

                shopDb.OrderObjects.Add(orderObject);

            }

            // Set the order's total to the orderTotal count NO is now done in the class itself
            //order.TotalPrice = orderTotal; 

            // Save the order
            shopDb.SaveChanges();

            // Empty the shopping cart
            EmptyCart();

            // Return the OrderId as the confirmation number
            return order.OrderId;
        }

        // We're using HttpContextBase to allow access to cookies.
        public string GetCartId(HttpContextBase context)
        {
            if (context.Session[CartSessionKey] == null)
            {
                if (!string.IsNullOrWhiteSpace(context.User.Identity.Name))
                {
                    context.Session[CartSessionKey] = context.User.Identity.Name;
                }
                else
                {
                    // Generate a new random GUID using System.Guid class
                    Guid tempCartId = Guid.NewGuid();

                    // Send tempCartId back to client as a cookie
                    context.Session[CartSessionKey] = tempCartId.ToString();
                }
            }

            return context.Session[CartSessionKey].ToString();
        }

        // When a user has logged in, migrate their shopping cart to
        // be associated with their username
        public void MigrateCart(string userName)
        {
            var shoppingCart = shopDb.Carts.Where(c => c.CartId == ShoppingCartId);

            foreach (Cart item in shoppingCart)
            {
                item.CartId = userName;
            }
            shopDb.SaveChanges();
        }
    }
}