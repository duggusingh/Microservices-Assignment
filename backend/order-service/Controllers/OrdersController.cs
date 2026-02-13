using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderService.Data;
using OrderService.Models;
using OrderService.Services;

namespace OrderService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController(IOrderService orderService, OrderDbContext context) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> CreateOrder(Order order)
        {
            var (success, message) = await orderService.PlaceOrderAsync(order);
            
            if (!success) 
                return BadRequest(new { Message = message });

            return Ok(new { Message = "Order Placed", OrderId = order.OrderId });
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetAllOrders()
        {
            return await context.Orders.Include(o => o.OrderItems).ToListAsync();
        }
        
    }
}
