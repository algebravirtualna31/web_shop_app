using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;
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

        public IActionResult Products()
        {
            ViewBag.Categories = _context.Categories.ToList();

            var products = _context.Products.ToList();

            return View(products);
        }

        [HttpGet]
        public List<Product> FilterProductsByCategory(int? categoryId)
        {
            if(categoryId != null)
            {
                return _context.Products.ToList();
            }
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
