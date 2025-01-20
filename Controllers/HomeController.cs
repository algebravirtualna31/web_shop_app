using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using web_shop_app.Data;
using web_shop_app.Models;

namespace web_shop_app.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
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


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
