using CarServ.Domain.Entities;
using CarServ.Repository.Repositories.DTO.Payment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarServ.service.Services.Interfaces
{
    public interface IPaymentService
    {
        Task<Payment> GetPaymentByIdAsync(int paymentId);
        Task<List<Payment>> GetPaymentByAppointmentIdAsync(int appointmentId);
        Task<List<Payment>> GetPaymentByCustomerIdAsync(int customerId);
        Task<List<Payment>> GetAllPaymentAsync();
        Task<List<Payment>> GetPaymentByMethodAsync(string method);
        Task<List<Payment>> SortPaymentByMethodAsync();
        Task<List<Payment>> GetPaymentByAmountRangeAsync(decimal minAmount, decimal maxAmount);
        Task<List<Payment>> GetPaymentByPaidDateAsync(DateTime paidDate);
        Task<List<Payment>> GetPaymentsByStatus(string status);
        Task<List<Payment>> GetPendingPayments(string status = "Pending");
        Task<List<Payment>> GetPaidPayments(string status = "Paid");
        Task<Payment> CreatePayment(PaymentDto dto);
    }
}
