using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using web_shop_app.Data;
using web_shop_app.Models;
using web_shop_app.ViewModels;

namespace web_shop_app.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/Products
        public async Task<IActionResult> Index()
        {
            var products = await _context.Products.Include(p=> p.ProductCategories).ToListAsync();

            foreach (var product in products)
            {
                foreach (var category in product.ProductCategories)
                {
                    var categoryDetails = _context.Categories.FirstOrDefault(c=>c.Id == category.CategoryId);

                    if(categoryDetails != null)
                    {
                        category.CategoryTitle = categoryDetails.Title;
                    }
                }
            }

            return View(products);
        }

        // GET: Admin/Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Admin/Products/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product)
        {
            try
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction("AssignCategoryToProduct", "ProductCategories", new { productId = product.Id });
            }
            catch (Exception)
            {
                //u praksi obavezno je logiranje grešaka

                return View(product);
            }

        }

        // GET: Admin/Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dbProduct = await _context.Products.FindAsync(id);
            if (dbProduct == null)
            {
                return NotFound();
            }

            var productCategory = await _context.ProductCategories.FirstOrDefaultAsync(c => c.ProductId == id);
            var categoryId = productCategory  == null ? "0" : productCategory.CategoryId.ToString();

            var productViewModel = new ProductViewModel()
            {
                Id = dbProduct.Id,
                Title = dbProduct.Title,
                Description = dbProduct.Description,
                Quantity = dbProduct.Quantity,
                Price = dbProduct.Price,
                CategoryId = categoryId
            };

            var defaultSelectItem = new SelectListItem() { Value = "0", Text = "---Select category---", Disabled = true };
            var categories = _context.Categories.Select(cat =>
            new SelectListItem()
            {
                Value = cat.Id.ToString(),
                Text = cat.Title

            }).ToList();

            categories.Add(defaultSelectItem);

            ViewBag.Categories = categories;

            return View(productViewModel);
        }

        // POST: Admin/Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProductViewModel productViewModel)
        {

            try
            {
                var dbProduct = await _context.Products.SingleAsync(p => p.Id == productViewModel.Id);

                dbProduct.Title = productViewModel.Title;
                dbProduct.Description = productViewModel.Description;
                dbProduct.Quantity = productViewModel.Quantity;
                dbProduct.Price = productViewModel.Price;

                var productCategory = await _context.ProductCategories.FirstOrDefaultAsync(c => c.ProductId == dbProduct.Id);

                if (productCategory != null)
                {
                    productCategory.CategoryId = Convert.ToInt32(productViewModel.CategoryId);

                    _context.Update(productCategory);
                }
                else
                {
                    var newProductCategory = new ProductCategory()
                    { 
                        CategoryId = Convert.ToInt32(productViewModel.CategoryId),
                        ProductId = dbProduct.Id
                    };

                    _context.Add(newProductCategory);
                }
                  

                _context.Update(dbProduct);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                /// DZ ,napravit logiranje :D
            }
            return RedirectToAction(nameof(Index));
        }

            // GET: Admin/Products/Delete/5
            public async Task<IActionResult> Delete(int? id)
            {
                if (id == null)
                {
                    return NotFound();
                }

                var product = await _context.Products
                    .FirstOrDefaultAsync(m => m.Id == id);
                if (product == null)
                {
                    return NotFound();
                }

                return View(product);
            }

            // POST: Admin/Products/Delete/5
            [HttpPost, ActionName("Delete")]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> DeleteConfirmed(int id)
            {
                var product = await _context.Products.FindAsync(id);
                if (product != null)
                {
                    _context.Products.Remove(product);
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            private bool ProductExists(int id)
            {
                return _context.Products.Any(e => e.Id == id);
            }
        }
    }
