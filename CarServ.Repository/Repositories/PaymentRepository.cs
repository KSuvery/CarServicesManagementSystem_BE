using CarServ.Domain.Entities;
using CarServ.Repository.Repositories.DTO.PayOS;
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

        public async Task<List<Payments>> GetPaymentsByMethodAsync()
        {
            return await _context.Payments
                .Include(p => p.PaymentMethod)
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

        //public async Task<PayOSPaymentResponse> CreateOnlinePaymentAsync(PayOSPaymentRequest request)
        //{
        //    using var httpClient = new HttpClient();
        //    var apiKey = "YOUR_PAYOS_API_KEY";

        //    httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

        //    var response = await httpClient.PostAsJsonAsync("payOsEndpoint", request);

        //    if (!response.IsSuccessStatusCode)
        //    {
        //        var error = await response.Content.ReadAsStringAsync();
        //        throw new Exception($"PayOS payment failed: {error}");
        //    }

        //    var payOsResponse = await response.Content.ReadFromJsonAsync<PayOSPaymentResponse>();
        //    return payOsResponse!;
        //}
    }
}
