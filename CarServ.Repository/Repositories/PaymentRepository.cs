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
    public class PaymentRepository : GenericRepository<Payments>, IPaymentRepository
    {
        private readonly CarServicesManagementSystemContext _context;

        public PaymentRepository(CarServicesManagementSystemContext context) : base(context)
        {
            _context = context;
        }
        
        public async Task<Payments> GetPaymentByIdAsync(int paymentId)
        {
            return await _context.Payments
                .FirstOrDefaultAsync(p => p.PaymentId == paymentId);
        }

        public async Task<List<Payments>> GetPaymentsByAppointmentIdAsync(int appointmentId)
        {
            return await _context.Payments
                .Where(p => p.AppointmentId == appointmentId)
                .ToListAsync();
        }

        public async Task<List<Payments>> GetPaymentsByCustomerIdAsync(int customerId)
        {
            return await _context.Payments
                .Where(p => p.Appointment.CustomerId == customerId)
                .ToListAsync();
        }

        public async Task<List<Payments>> GetAllPaymentsAsync()
        {
            return await _context.Payments
                .Include(p => p.Appointment)
                .ToListAsync();
        }

        public async Task<List<Payments>> GetPaymentsByMethodAsync(string method)
        {
            return await _context.Payments
                .Where(p => p.PaymentMethod.Equals(method, StringComparison.OrdinalIgnoreCase))
                .ToListAsync();
        }

        public async Task<List<Payments>> SortPaymentsByMethodAsync()
        {
            return await _context.Payments
                .OrderBy(p => p.PaymentMethod)
                .ToListAsync();
        }

        public async Task<List<Payments>> GetPaymentsByAmountRangeAsync(decimal minAmount, decimal maxAmount)
        {
            return await _context.Payments
                .Where(p => p.Amount >= minAmount && p.Amount <= maxAmount)
                .ToListAsync();
        }

        public async Task<List<Payments>> GetPaymentsByPaidDateAsync(DateTime paidDate)
        {
            return await _context.Payments
                .Where(p => p.PaidAt == paidDate.Date)
                .ToListAsync();
        }

        public async Task<Payments> CreatePayment(Payments payment)
        {
            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();
            return payment;
        }
    }
}
