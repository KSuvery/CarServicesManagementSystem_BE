using CarServ.Domain.Entities;
using CarServ.Repository.Repositories.DTO;
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
        public async Task<PaginationResult<List<Order>>> GetOrders(int currentPage = 1, int pageSize = 5)
        {
            return await _orderService.GetAllOrdersWithPaging(currentPage, pageSize);            
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrderById(int id)
        {
            var order = await _orderService.GetOrderByIdAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            return Ok(order);
        }

        [HttpGet("customer/{customerId}")]
        public async Task<PaginationResult<List<Order>>> GetOrdersByCustomerId(int customerId, int currentPage = 1, int pageSize = 5)
        {
            return await _orderService.GetAllOrdersByCustomerIdWithPaging(customerId, currentPage, pageSize);
            /*if (orders == null)
            {
                return NotFound();
            }
            return Ok(orders);*/
        }

        [HttpPost("create")]
        public async Task<ActionResult<Order>> CreateOrder(
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

        [HttpPut("update")]
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
