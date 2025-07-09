using CarServ.Domain.Entities;
using CarServ.Repository.Repositories.DTO.PayOS;
using CarServ.Repository.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarServ.Service.Services.Interfaces
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;

        public PaymentService(IPaymentRepository paymentRepository)
        {
            _paymentRepository = paymentRepository;
        }

        public async Task<Payments> GetPaymentByIdAsync(int paymentId)
        {
            return await _paymentRepository.GetPaymentByIdAsync(paymentId);
        }

        public async Task<List<Payments>> GetPaymentsByAppointmentIdAsync(int appointmentId)
        {
            return await _paymentRepository.GetPaymentsByAppointmentIdAsync(appointmentId);
        }

        public async Task<List<Payments>> GetPaymentsByCustomerIdAsync(int customerId)
        {
            return await _paymentRepository.GetPaymentsByCustomerIdAsync(customerId);
        }

        public async Task<List<Payments>> GetAllPaymentsAsync()
        {
            return await _paymentRepository.GetAllPaymentsAsync();
        }

        public async Task<List<Payments>> GetPaymentsByMethodAsync()
        {
            return await _paymentRepository.GetPaymentsByMethodAsync();
        }

        public async Task<List<Payments>> SortPaymentsByMethodAsync()
        {
            return await _paymentRepository.SortPaymentsByMethodAsync();
        }

        //public async Task<Payments> CreateOnlinePaymentAsync(PayOSPaymentRequest request)
        //{
        //    return await _paymentRepository.CreateOnlinePaymentAsync(request);
        //}
    }
}
