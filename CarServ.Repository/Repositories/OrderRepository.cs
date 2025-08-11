using CarServ.Domain.Entities;
using CarServ.Repository.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarServ.Repository.Repositories
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        private readonly CarServicesManagementSystemContext _context;
        public OrderRepository(CarServicesManagementSystemContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Order> GetOrderByIdAsync(int orderId)
        {
            return await _context.Orders
                .FirstOrDefaultAsync(o => o.OrderId == orderId);
        }

        public async Task<List<Order>> GetOrdersByCustomerIdAsync(int customerId)
        {
            return await _context.Orders
                .Where(o => o.Appointment.CustomerId == customerId)
                .ToListAsync();
        }

        public async Task<List<Order>> GetAllOrdersAsync()
        {
            return await _context.Orders.ToListAsync();
        }

        public async Task<Order> CreateOrderAsync(
            int appointmentId,
            int? promotionId,
            DateTime createdAt)
        {
            createdAt = DateTime.Now;
            var order = new Order
            {
                AppointmentId = appointmentId,
                PromotionId = promotionId,
                CreatedAt = createdAt,
            };
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            return order;
        }

        public async Task<Order> UpdateOrderAsync(
            int orderId,
            int appointmentId,
            int? promotionId,
            DateTime createdAt)
        {
            var order = await GetOrderByIdAsync(orderId);
            if (order == null)
            {
                return null;
            }
            order.AppointmentId = appointmentId;
            order.PromotionId = promotionId;
            order.CreatedAt = createdAt;
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
            return order;
        }
    }
}
