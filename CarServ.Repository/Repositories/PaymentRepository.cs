using CarServ.Domain.Entities;
using CarServ.Repository.Repositories.DTO.Payment;
using CarServ.Repository.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

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

        public async Task<Payment> GetPaymentByOrderIdAsync(int orderId)
        {
            return await _context.Payments
                .FirstOrDefaultAsync(p => p.OrderId == orderId);
        }

        public async Task<List<Payment>> GetPaymentsByAppointmentIdAsync(int appointmentId)
        {
            return await _context.Payments
                .Where(p => p.AppointmentId == appointmentId)
                .ToListAsync();
        }

        public async Task<List<Payment>> GetPaymentsByCustomerIdAsync(int customerId)
        {
            return await _context.Payments
                .Where(p => p.Appointment.CustomerId == customerId)
                .ToListAsync();
        }

        public async Task<List<Payment>> GetAllPaymentsAsync()
        {
            return await _context.Payments.ToListAsync();
        }

        public async Task<List<Payment>> GetPaymentsByMethodAsync(string method)
        {
            return await _context.Payments
                .Where(p => p.PaymentMethod != null &&
                    p.PaymentMethod.ToLower().Contains(method.ToLower()))
                .ToListAsync();
        }

        public async Task<List<Payment>> SortPaymentsByMethodAsync()
        {
            return await _context.Payments
                .OrderBy(p => p.PaymentMethod)
                .ToListAsync();
        }

        public async Task<List<Payment>> GetPaymentsByAmountRangeAsync(decimal minAmount, decimal maxAmount)
        {
            return await _context.Payments
                .Where(p => p.Amount >= minAmount && p.Amount <= maxAmount)
                .ToListAsync();
        }

        public async Task<List<Payment>> GetPaymentsByPaidDateAsync(DateTime paidDate)
        {
            return await _context.Payments
                .Where(p => p.PaidAt == paidDate.Date)
                .ToListAsync();
        }

        public async Task<List<Payment>> GetPaymentsByStatus(string status)
        {
            return await _context.Payments
                .Where(p => p.Status.ToLower().Contains(status.ToLower()))
                .ToListAsync();
        }

        public async Task<List<Payment>> GetPendingPayments(string status = "Pending")
        {
            return await _context.Payments
                .Where(p => p.Status.ToLower() == status.ToLower())
                .ToListAsync();
        }

        public async Task<List<Payment>> GetPaidPayments(string status = "Paid")
        {
            return await _context.Payments
                .Where(p => p.Status.ToLower() == status.ToLower())
                .ToListAsync();
        }

        public async Task<Payment> CreatePayment(PaymentDto dto)
        {
            try
            {
                var payment = new Payment
                {
                    AppointmentId = dto.AppointmentId,
                    Amount = dto.Amount,
                    PaymentMethod = dto.PaymentMethod,
                    PaidAt = null,
                    Status = dto.Status = "Pending",
                    OrderId = dto.OrderId
                };

                _context.Payments.Add(payment);
                await _context.SaveChangesAsync();
                return payment;
            }
            catch (Exception ex)
            {
                throw new Exception("Error creating payment", ex);
            }
        }

        public async Task<Payment> UpdatePaymentStatus(int paymentId, string status)
        {
            var payment = await GetPaymentByIdAsync(paymentId);

            if (payment == null)
            {
                throw new Exception($"Payment with ID {paymentId} not found.");
            }

            string formattedStatus = string.IsNullOrWhiteSpace(status)
                ? status
                : char.ToUpper(status.Trim()[0]) + status.Trim().Substring(1).ToLower();

            payment.Status = formattedStatus;

            if (formattedStatus.Equals("Paid", StringComparison.OrdinalIgnoreCase))
            {
                payment.PaidAt = DateTime.UtcNow;
            }

            _context.Payments.Update(payment);
            await _context.SaveChangesAsync();
            return payment;
        }
    }
}
