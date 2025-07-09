using CarServ.Domain.Entities;
using CarServ.Repository.Repositories.DTO.PayOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarServ.Repository.Repositories.Interfaces
{
    public interface IPaymentRepository : IGenericRepository<Payments>
    {
        Task<Payments> GetPaymentByIdAsync(int paymentId);
        Task<List<Payments>> GetPaymentsByAppointmentIdAsync(int appointmentId);
        Task<List<Payments>> GetPaymentsByCustomerIdAsync(int customerId);
        Task<List<Payments>> GetAllPaymentsAsync();
        Task<List<Payments>> GetPaymentsByMethodAsync();
        Task<List<Payments>> SortPaymentsByMethodAsync();
        //Task<PayOSPaymentResponse> CreateOnlinePaymentAsync(PayOSPaymentRequest request);
    }
}
