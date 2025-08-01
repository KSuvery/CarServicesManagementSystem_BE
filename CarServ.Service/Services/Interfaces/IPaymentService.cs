using CarServ.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarServ.Service.Services.Interfaces
{
    public interface IPaymentService
    {
        Task<Payments> GetPaymentByIdAsync(int paymentId);
        Task<Payments> GetPaymentByOrderIdAsync(int orderId);
        Task<List<Payments>> GetPaymentsByAppointmentIdAsync(int appointmentId);
        Task<List<Payments>> GetPaymentsByCustomerIdAsync(int customerId);
        Task<List<Payments>> GetAllPaymentsAsync();
        Task<List<Payments>> GetPaymentsByMethodAsync(string method);
        Task<List<Payments>> SortPaymentsByMethodAsync();
        Task<List<Payments>> GetPaymentsByAmountRangeAsync(decimal minAmount, decimal maxAmount);
        Task<List<Payments>> GetPaymentsByPaidDateAsync(DateTime paidDate);
        Task<Payments> CreatePayment(Payments payment);
    }
}
