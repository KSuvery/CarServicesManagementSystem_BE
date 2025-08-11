using CarServ.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarServ.Service.Services.Interfaces
{
    public interface IOrderService
    {
        Task<Order> GetOrderByIdAsync(int orderId);
        Task<List<Order>> GetOrdersByCustomerIdAsync(int customerId);
        Task<List<Order>> GetAllOrdersAsync();
        Task<Order> CreateOrderAsync(int appointmentId, int? promotionId, DateTime createdAt);
        Task<Order> UpdateOrderAsync(int orderId, int appointmentId, int? promotionId, DateTime createdAt);
    }
}
