using CarServ.Domain.Entities;
using CarServ.Repository.Repositories.DTO;
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
        Task<PaginationResult<List<Order>>> GetAllOrdersByCustomerIdWithPaging(int currentPage, int pageSize, int customerId);
        Task<List<Order>> GetAllOrdersAsync();
        Task<PaginationResult<List<Order>>> GetAllOrdersWithPaging(int currentPage, int pageSize);
        Task<Order> CreateOrderAsync(int appointmentId, int? promotionId, DateTime createdAt);
        Task<Order> UpdateOrderAsync(int orderId, int appointmentId, int? promotionId, DateTime createdAt);
    }
}
