using CarServ.Domain.Entities;
using CarServ.Repository.Repositories.DTO.Payment;
using CarServ.Repository.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using static CarServ.Repository.Repositories.PaymentRepository;

namespace CarServ.Repository.Repositories
{
    public class PaymentRepository : GenericRepository<Payment>, IPaymentRepository
    {
        private readonly CarServicesManagementSystemContext _context;

        public PaymentRepository(CarServicesManagementSystemContext context) : base(context)
        {
            _context = context;
        }
        
        public async Task<Payment> GetPaymentByIdAsync(int paymentId)
        {
            return await _context.Payments
                .FirstOrDefaultAsync(p => p.PaymentId == paymentId);
        }

        public async Task<List<Payment>> GetPaymentByAppointmentIdAsync(int appointmentId)
        {
            return await _context.Payments
                .Where(p => p.AppointmentId == appointmentId)
                .ToListAsync();
        }

        public async Task<List<Payment>> GetPaymentByCustomerIdAsync(int customerId)
        {
            return await _context.Payments
                .Where(p => p.Appointment.CustomerId == customerId)
                .ToListAsync();
        }

        public async Task<List<Payment>> GetAllPaymentAsync()
        {
            return await _context.Payments.ToListAsync();
        }

        public async Task<List<Payment>> GetPaymentByMethodAsync(string method)
        {
            return await _context.Payments
                .Where(p => p.PaymentMethod != null && 
                    p.PaymentMethod.ToLower().Contains(method.ToLower()))
                .ToListAsync();
        }

        public async Task<List<Payment>> SortPaymentByMethodAsync()
        {
            return await _context.Payments
                .OrderBy(p => p.PaymentMethod)
                .ToListAsync();
        }

        public async Task<List<Payment>> GetPaymentByAmountRangeAsync(decimal minAmount, decimal maxAmount)
        {
            return await _context.Payments
                .Where(p => p.Amount >= minAmount && p.Amount <= maxAmount)
                .ToListAsync();
        }

        public async Task<List<Payment>> GetPaymentByPaidDateAsync(DateTime paidDate)
        {
            return await _context.Payments
                .Where(p => p.PaidAt == paidDate.Date)
                .ToListAsync();
        }

        public async Task<Payment> CreatePayment(Payment payment)
        {
            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();
            return payment;
        }

        

            public async Task<Payment> ProcessPayment(PaymentDto dto)
            {
                // Validate the input data
                if (string.IsNullOrEmpty(dto.PaymentMethod) ||
                    (dto.PaymentMethod != "Online Banking" && dto.PaymentMethod != "Cash"))
                {
                    throw new ArgumentException("Invalid payment method provided.");
                }

                // Retrieve the appointment to calculate the total price
                var appointment = await _context.Appointments
                    .Include(a => a.Package)
                    .Include(a => a.AppointmentServices)
                    .ThenInclude(s => s.Service)
                    .FirstOrDefaultAsync(a => a.AppointmentId == dto.AppointmentId);

                if (appointment == null)
                {
                    throw new InvalidOperationException("Appointment not found.");
                }

                decimal totalAmount = 0;
                
                if (appointment.PackageId.HasValue)
                {
                    totalAmount += appointment.Package.Price ?? 0;
                    if (appointment.PromotionId != null)
                    {
                        totalAmount = (decimal)(totalAmount * appointment.Promotion.DiscountPercentage);
                    }
                }

                foreach (var appointmentService in appointment.AppointmentServices)
                {
                    totalAmount += appointmentService.Service.Price ?? 0;
                    if (appointment.PromotionId != null)
                    {
                        totalAmount = (decimal)(totalAmount * appointment.Promotion.DiscountPercentage);
                    }
                }

                var payment = new Payment
                {
                    AppointmentId = appointment.AppointmentId,
                    Amount = totalAmount,
                    PaymentMethod = dto.PaymentMethod,
                    PaidAt = DateTime.Now
                };

                var order = new Order
                {
                    AppointmentId = appointment.AppointmentId,
                    CreatedAt = DateTime.Now,
                };

                if (appointment.PackageId.HasValue)
                {
                    order.OrderDetails.Add(new OrderDetail
                    {
                        PackageId = appointment.PackageId,
                        Quantity = 1,
                        UnitPrice = appointment.Package.Price,
                        LineTotal = appointment.Package.Price
                    });
                }

                foreach (var appointmentService in appointment.AppointmentServices)
                {
                    order.OrderDetails.Add(new OrderDetail
                    {
                        ServiceId = appointmentService.ServiceId,
                        Quantity = appointmentService.Quantity,
                        UnitPrice = appointmentService.Service.Price,
                        LineTotal = appointmentService.Service.Price * appointmentService.Quantity
                    });
                }

                // Add the payment and order to the context
                _context.Payments.Add(payment);
                _context.Orders.Add(order);

                // Save changes to the database
                await _context.SaveChangesAsync();

                return payment;
            }
        

    }
}
