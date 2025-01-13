using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using web_shop_app.Data;
using web_shop_app.Models;

namespace web_shop_app.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class OrderItemController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrderItemController(ApplicationDbContext context)
        {
            _context = context;
        }


        // GET: Admin/OrderItem/Create
        public IActionResult Create(int orderId)
        {
            ViewBag.OrderId = orderId;  

            ViewBag.Products = _context.Products.Select(
            product => new SelectListItem
            {
                Value = product.Id.ToString(),
                Text = product.Title

            }).ToList();

            return View();
        }

        // POST: Admin/OrderItem/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( OrderItem orderItem)
        {
            try
            {
                var productDetails = _context.Products
                    .Where(p => p.Id == orderItem.ProductId)
                    .SingleOrDefault();

                if (productDetails == null){ 

                    return NotFound();
                }

                var order = await _context.Orders.FirstOrDefaultAsync(o => o.Id == orderItem.OrderId);

                if (order == null)
                {
                    return NotFound();
                }

                orderItem.Total = orderItem.Quantity * productDetails.Price;
                order.Total = order.Total + orderItem.Total;

                // order.Total += orderItem.Total --- kraće----


                await _context.AddAsync(orderItem);
                 _context.Update(order);
                
                await _context.SaveChangesAsync();
                return RedirectToAction("Details","Order", new { id =  orderItem.OrderId});
            }
            catch (Exception)
            {

                return View(orderItem);
            }
          
        }


        // GET: Admin/OrderItem/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderItem = await _context.OrderItems
                .FirstOrDefaultAsync(m => m.Id == id);
            if (orderItem == null)
            {
                return NotFound();
            }

            return View(orderItem);
        }

        // POST: Admin/OrderItem/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
             var orderItem = await _context.OrderItems.FindAsync(id);

            if(orderItem == null)
            {
                return NotFound();
            }

            var order = await _context.Orders.FindAsync(orderItem.OrderId);

            if (order == null)
            {
                return NotFound();
            }

            order.Total = order.Total - orderItem.Total;

            if (orderItem != null)
            {
                _context.OrderItems.Remove(orderItem);
            }

            _context.Update(order);

            await _context.SaveChangesAsync();
            return RedirectToAction("Details", "Order", new { id = orderItem.OrderId });
        }

        private bool OrderItemExists(int id)
        {
            return _context.OrderItems.Any(e => e.Id == id);
        }
    }
}
