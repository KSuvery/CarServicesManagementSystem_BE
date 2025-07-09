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
        Task<List<Payments>> GetPaymentsByAppointmentIdAsync(int appointmentId);
        Task<List<Payments>> GetPaymentsByCustomerIdAsync(int customerId);
        Task<List<Payments>> GetAllPaymentsAsync();
        Task<List<Payments>> GetPaymentsByMethodAsync();
        Task<List<Payments>> SortPaymentsByMethodAsync();
        //Task<Payments> CreateOnlinePaymentAsync(PayOSPaymentRequest request);
    }
}
