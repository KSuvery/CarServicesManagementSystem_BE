using CarServ.Domain.Entities;
using CarServ.Repository.Repositories.Interfaces;
using CarServ.Service.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarServ.Service.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;

        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<Orders> GetOrderByIdAsync(int orderId)
        {
            return await _orderRepository.GetOrderByIdAsync(orderId);
        }

        public async Task<List<Orders>> GetOrdersByCustomerIdAsync(int customerId)
        {
            return await _orderRepository.GetOrdersByCustomerIdAsync(customerId);
        }

        public async Task<List<Orders>> GetAllOrdersAsync()
        {
            return await _orderRepository.GetAllOrdersAsync();
        }

        public async Task<Orders> CreateOrderAsync(int appointmentId, int? promotionId, DateTime createdAt)
        {
            return await _orderRepository.CreateOrderAsync(appointmentId, promotionId, createdAt);
        }

        public async Task<Orders> UpdateOrderAsync(int orderId, int appointmentId, int? promotionId, DateTime createdAt)
        {
            return await _orderRepository.UpdateOrderAsync(orderId, appointmentId, promotionId, createdAt);
        }
    }
}
