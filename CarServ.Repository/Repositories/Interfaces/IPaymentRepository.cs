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
        Task<List<Payment>> GetPaymentByAppointmentIdAsync(int appointmentId);
        Task<List<Payment>> GetPaymentByCustomerIdAsync(int customerId);
        Task<List<Payment>> GetAllPaymentAsync();
        Task<List<Payment>> GetPaymentByMethodAsync(string method);
        Task<List<Payment>> SortPaymentByMethodAsync();
        Task<List<Payment>> GetPaymentByAmountRangeAsync(decimal minAmount, decimal maxAmount);
        Task<List<Payment>> GetPaymentByPaidDateAsync(DateTime paidDate);
        Task<Payment> CreatePayment(Payment payment);
        Task<Payment> ProcessPayment(PaymentDto dto);
    }
}
