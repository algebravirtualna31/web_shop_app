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
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrderController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/Order
        public async Task<IActionResult> Index()
        {
            return View(await _context.Orders.ToListAsync());
        }

        // GET: Admin/Order/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .FirstOrDefaultAsync(m => m.Id == id);

            if (order == null)
            {
                return NotFound();
            }

            order.OrderItems = (
                from orderItem in _context.OrderItems
                where orderItem.OrderId == order.Id
                select new OrderItem
                {
                    Id = orderItem.OrderId,
                    OrderId = orderItem.OrderId,
                    ProductId = orderItem.ProductId,
                    Quantity = orderItem.Quantity,
                    Total = orderItem.Total,
                    ProductTitle = (from product in _context.Products where product.Id == orderItem.ProductId select product.Title).FirstOrDefault()
                }
               ).ToList();

            

            return View(order);
        }

        // GET: Admin/Order/Create
        public IActionResult Create()
        {
            ViewBag.Users = _context.Users.Select(
                 user => new SelectListItem
                 {
                     Value = user.Id.ToString(),
                     Text = user.FirstName + " " + user.LastName,
                 }
             ).ToList();

            return View();
        }

        // POST: Admin/Order/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(OrderViewModel orderViewModel)
        {
            try
            {
                var order = new Order()
                {   
                    Message = orderViewModel.Message,
                    UserId = orderViewModel.UserId,
                    Total = orderViewModel.Total,
                    BillingAddress = orderViewModel.OrderAddress.BillingAddress,
                    BillingCity = orderViewModel.OrderAddress.BillingCity,
                    BillingEmail = orderViewModel.OrderAddress.BillingEmail,
                    BillingCountry = orderViewModel.OrderAddress.BillingCountry,
                    BillingFirstName = orderViewModel.OrderAddress.BillingFirstName,
                    BillingLastName = orderViewModel.OrderAddress.BillingLastName,
                    BillingPhone = orderViewModel.OrderAddress.BillingPhone,
                    BillingZip = orderViewModel.OrderAddress.BillingZip
                };


                _context.Add(order);
                await _context.SaveChangesAsync();

                return RedirectToAction("Create","OrderItem", new {orderId = order.Id});
            }
            catch (Exception)
            {
                return View(orderViewModel);
            }
              
       
        }

        // GET: Admin/Order/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders.FindAsync(id);
            var userOnOrder = await _context.Users.FirstOrDefaultAsync(user => user.Id == order.UserId);

            var orderViewModel = new OrderViewModel()
            {
                Id = order.Id,
                Total = order.Total,
                UserId = order.UserId,
                DateCreated = order.DateCreated,
                Message = order.Message,
                OrderAddress = new OrderAddress()
                {
                    BillingAddress = order.BillingAddress,
                    BillingCity = order.BillingCity,
                    BillingCountry = order.BillingCountry,
                    BillingZip = order.BillingZip,
                    BillingEmail = order.BillingEmail,
                    BillingPhone = order.BillingPhone,
                    BillingFirstName = order.BillingFirstName,
                    BillingLastName = order.BillingLastName
                }
            };

            ViewBag.User = userOnOrder.FirstName + " " + userOnOrder.LastName;  

            if (order == null)
            {
                return NotFound();
            }
            return View(orderViewModel);
        }

        // POST: Admin/Order/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, OrderViewModel orderViewModel)
        {
            if (id != orderViewModel.Id)
            {
                return NotFound();
            }

            Order order = new Order();

            try
            {
                order = _context.Orders.FirstOrDefault(order => order.Id == id);

                if (order == null)
                {
                    return NotFound();
                }

                order.BillingAddress = orderViewModel.OrderAddress.BillingAddress;
                order.BillingCity = orderViewModel.OrderAddress.BillingCity;
                order.BillingZip = orderViewModel.OrderAddress.BillingZip;
                order.BillingCountry = orderViewModel.OrderAddress.BillingCountry;
                order.BillingPhone = orderViewModel.OrderAddress.BillingPhone;
                order.BillingEmail = orderViewModel.OrderAddress.BillingEmail;
                order.BillingLastName = orderViewModel.OrderAddress.BillingLastName;
                order.BillingFirstName = orderViewModel.OrderAddress.BillingFirstName;


                _context.Update(order);
                await _context.SaveChangesAsync();

            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(id))
                {
                    return NotFound();
                }
                else
                {
                    return View(order);
                }
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Admin/Order/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Admin/Order/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order != null)
            {
                _context.Orders.Remove(order);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.Id == id);
        }
    }
}
