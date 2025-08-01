using CarServ.Domain.Entities;
using CarServ.Service.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CarServ.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Orders>>> GetOrders()
        {
            var orders = await _orderService.GetAllOrdersAsync();
            return Ok(orders);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Orders>> GetOrderById(int id)
        {
            var order = await _orderService.GetOrderByIdAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            return Ok(order);
        }

        [HttpGet("customer/{customerId}")]
        public async Task<ActionResult<IEnumerable<Orders>>> GetOrdersByCustomerId(int customerId)
        {
            var orders = await _orderService.GetOrdersByCustomerIdAsync(customerId);
            if (orders == null || !orders.Any())
            {
                return NotFound();
            }
            return Ok(orders);
        }

        [HttpPost]
        public async Task<ActionResult<Orders>> CreateOrder(
            int appointmentId,
            int? promotionId,
            DateTime createdAt)
        {
            var createdOrder = await _orderService.CreateOrderAsync(appointmentId, promotionId, createdAt);
            if (createdOrder == null)
            {
                return BadRequest("Invalid order data.");
            }
            return CreatedAtAction(nameof(GetOrderById), new { id = createdOrder.OrderId }, createdOrder);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrder(
            int orderId,
            int appointmentId,
            int? promotionId,
            DateTime createdAt)
        {
            var updatedOrder = await _orderService.UpdateOrderAsync(orderId, appointmentId, promotionId, createdAt);
            if ( !await OrderExists(orderId))
            {
                return BadRequest("Invalid order to update.");
            }
            return NoContent();
        }

        private async Task<bool> OrderExists(int id)
        {
            var order = await _orderService.GetOrderByIdAsync(id);
            return order != null;
        }
    }
}
