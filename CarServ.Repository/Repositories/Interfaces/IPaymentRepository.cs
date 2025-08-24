using CarServ.Domain.Entities;
using CarServ.Repository.Repositories.DTO.Payment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarServ.Repository.Repositories.Interfaces
{
    public interface IPaymentRepository : IGenericRepository<Payment>
    {
        Task<Payment> GetPaymentByIdAsync(int paymentId);
        Task<Payment> GetPaymentByOrderIdAsync(int orderId);
        Task<List<Payment>> GetPaymentsByAppointmentIdAsync(int appointmentId);
        Task<List<Payment>> GetPaymentsByCustomerIdAsync(int customerId);
        Task<List<Payment>> GetAllPaymentsAsync();
        Task<List<Payment>> GetPaymentsByMethodAsync(string method);
        Task<List<Payment>> SortPaymentsByMethodAsync();
        Task<List<Payment>> GetPaymentsByAmountRangeAsync(decimal minAmount, decimal maxAmount);
        Task<List<Payment>> GetPaymentsByPaidDateAsync(DateTime paidDate);
        Task<List<Payment>> GetPaymentsByStatus(string status);
        Task<List<Payment>> GetPendingPayments(string status = "Pending");
        Task<List<Payment>> GetPaidPayments(string status = "Paid");
        Task<Payment> CreatePayment(PaymentDto dto);
    }
}
