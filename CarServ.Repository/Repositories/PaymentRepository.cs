using CarServ.Domain.Entities;
using CarServ.Repository.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

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
            return await _context.Payment
                .FirstOrDefaultAsync(p => p.PaymentId == paymentId);
        }

        public async Task<List<Payment>> GetPaymentByAppointmentIdAsync(int appointmentId)
        {
            return await _context.Payment
                .Where(p => p.AppointmentId == appointmentId)
                .ToListAsync();
        }

        public async Task<List<Payment>> GetPaymentByCustomerIdAsync(int customerId)
        {
            return await _context.Payment
                .Where(p => p.Appointment.CustomerId == customerId)
                .ToListAsync();
        }

        public async Task<List<Payment>> GetAllPaymentAsync()
        {
            return await _context.Payment.ToListAsync();
        }

        public async Task<List<Payment>> GetPaymentByMethodAsync(string method)
        {
            return await _context.Payment
                .Where(p => p.PaymentMethod != null && 
                    p.PaymentMethod.ToLower().Contains(method.ToLower()))
                .ToListAsync();
        }

        public async Task<List<Payment>> SortPaymentByMethodAsync()
        {
            return await _context.Payment
                .OrderBy(p => p.PaymentMethod)
                .ToListAsync();
        }

        public async Task<List<Payment>> GetPaymentByAmountRangeAsync(decimal minAmount, decimal maxAmount)
        {
            return await _context.Payment
                .Where(p => p.Amount >= minAmount && p.Amount <= maxAmount)
                .ToListAsync();
        }

        public async Task<List<Payment>> GetPaymentByPaidDateAsync(DateTime paidDate)
        {
            return await _context.Payment
                .Where(p => p.PaidAt == paidDate.Date)
                .ToListAsync();
        }

        public async Task<Payment> CreatePayment(Payment payment)
        {
            _context.Payment.Add(payment);
            await _context.SaveChangesAsync();
            return payment;
        }
    }
}
