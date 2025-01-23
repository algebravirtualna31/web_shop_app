using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using web_shop_app.Data;
using web_shop_app.Extensions;
using web_shop_app.Models;
using web_shop_app.ViewModels;

namespace web_shop_app.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public const string SessionKeyName = "_cart";

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index(string? message)
        {
            ViewBag.Message = message ?? "";

            return View();
        }

        public IActionResult Privacy()
        {
            
            return View();
        }

        public IActionResult Products(int? categoryId)
        {
            ViewBag.Categories = _context.Categories.ToList();

            List<Product> products = new List<Product>();

            if (categoryId == null)
            {
                products = _context.Products.ToList();
            }
            else
            {
                products = (
                    from product in _context.Products
                    join product_category in _context.ProductCategories
                        on product.Id equals product_category.ProductId
                    where product_category.CategoryId == categoryId
                     select new Product
                        {
                            Id = product.Id,
                            Title = product.Title,
                            Description = product.Description,
                            Quantity = product.Quantity,
                            Price = product.Price
                        }
               ).ToList();
            }

            

            return View(products);
        }

        public IActionResult Order()
        {
            List<CartItem> cart =
            HttpContext.Session.GetObjectFromJson<List<CartItem>>(SessionKeyName) ?? new List<CartItem>();

            if (cart.Count == 0)
            {
                RedirectToAction(nameof(Index));
            }

            decimal total = 0;

            ViewBag.Cart = cart;
            ViewBag.TotalPrice = cart.Sum(item => total + item.GetTotal());
            ViewBag.ShouldShowRemoveAction = false;

            return View();
        }

        [HttpPost]
        public IActionResult CreateOrder(UserOrderViewModel orderViewModel)
        {
            List<CartItem> cart =
            HttpContext.Session.GetObjectFromJson<List<CartItem>>(SessionKeyName) ?? new List<CartItem>();

            if (cart.Count == 0)
            {
                RedirectToAction(nameof(Index));
            }

            var order = new Order()
            {
                BillingAddress = orderViewModel.OrderAddress.BillingAddress,
                BillingCity = orderViewModel.OrderAddress.BillingCity,
                BillingCountry = orderViewModel.OrderAddress.BillingCountry,
                BillingEmail = orderViewModel.OrderAddress.BillingEmail,
                BillingFirstName = orderViewModel.OrderAddress.BillingFirstName,
                BillingLastName = orderViewModel.OrderAddress.BillingLastName,
                BillingPhone = orderViewModel.OrderAddress.BillingPhone,
                BillingZip = orderViewModel.OrderAddress.BillingZip,
                Message = orderViewModel.Message,
                Total = orderViewModel.TotalPrice,
            };

            _context.Add(order);
            _context.SaveChanges();

            var orderId = order.Id;

            foreach (var item in cart)
            {
                OrderItem orderItem = new OrderItem()
                {
                    OrderId = orderId,
                    ProductId = item.Product.Id,
                    Quantity = item.Quantity,
                    Total = item.GetTotal()
                };

                _context.Add(orderItem);
                _context.SaveChanges();
            }

            return RedirectToAction("Index", new {message = "Than you for your order! :)" }); 
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
