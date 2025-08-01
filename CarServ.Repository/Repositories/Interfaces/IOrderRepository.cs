using CarServ.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarServ.Repository.Repositories.Interfaces
{
    public interface IOrderRepository : IGenericRepository<Orders>
    {
        Task<Orders> GetOrderByIdAsync(int orderId);
        Task<List<Orders>> GetOrdersByCustomerIdAsync(int customerId);
        Task<List<Orders>> GetAllOrdersAsync();
        Task<Orders> CreateOrderAsync(int appointmentId, int? promotionId, DateTime createdAt);
        Task<Orders> UpdateOrderAsync(int orderId, int appointmentId, int? promotionId, DateTime createdAt);
    }
}
