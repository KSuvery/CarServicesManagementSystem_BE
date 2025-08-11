using CarServ.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarServ.Repository.Repositories.Interfaces
{
    public interface IOrderRepository : IGenericRepository<Order>
    {
        Task<Order> GetOrderByIdAsync(int orderId);
        Task<List<Order>> GetOrdersByCustomerIdAsync(int customerId);
        Task<List<Order>> GetAllOrdersAsync();
        Task<Order> CreateOrderAsync(int appointmentId, int? promotionId, DateTime createdAt);
        Task<Order> UpdateOrderAsync(int orderId, int appointmentId, int? promotionId, DateTime createdAt);
    }
}
