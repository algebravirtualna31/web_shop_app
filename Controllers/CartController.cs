﻿using Microsoft.AspNetCore.Mvc;
using web_shop_app.Data;
using web_shop_app.Extensions;
using web_shop_app.Models;

namespace web_shop_app.Controllers
{
    public class CartController : Controller
    {
        public const string SessionKeyName = "_cart";

        private readonly ApplicationDbContext _context;

        public CartController(ApplicationDbContext context)
        {
           _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }


        public IActionResult AddToCart(int productId, int selectedQuantity)
        {
            List<CartItem> sessionCartItems = 
                HttpContext.Session.GetObjectFromJson<List<CartItem>>(SessionKeyName) ?? new List<CartItem>();

            if(sessionCartItems.Count == 0)
            {
                CartItem cartItem = new CartItem
                {
                    Product =  _context.Products.Find(productId),
                    Quantity = selectedQuantity
                };

                sessionCartItems.Add(cartItem);
            }
            else
            {
                int productIndex = IsExistingInCart(productId);

                if(productIndex == -1)
                {
                    CartItem cartItem = new CartItem
                    {
                        Product = _context.Products.Find(productId),
                        Quantity = selectedQuantity
                    };

                    sessionCartItems.Add(cartItem);
                }
                else
                {
                    sessionCartItems[productIndex].IncreaseQuantity(selectedQuantity);
                }
            }

            HttpContext.Session.SetObjectAsJson(SessionKeyName, sessionCartItems);

            return RedirectToAction(nameof(Index));
        }

        private int IsExistingInCart(int productId)
        {
            List<CartItem> cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>(SessionKeyName);

            for (int i = 0; i < cart.Count; i++)
            {
                if (cart[i].Product.Id == productId)
                {
                    return i;
                }
            }

            return -1;
        }
    }
}
