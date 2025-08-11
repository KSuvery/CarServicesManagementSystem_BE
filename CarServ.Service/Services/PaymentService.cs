using CarServ.Domain.Entities;
using CarServ.Repository.Repositories.Interfaces;
using CarServ.Service.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarServ.Service.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;

        public PaymentService(IPaymentRepository paymentRepository)
        {
            _paymentRepository = paymentRepository;
        }

        public async Task<Payment> GetPaymentByIdAsync(int paymentId)
        {
            return await _paymentRepository.GetPaymentByIdAsync(paymentId);
        }

        public async Task<Payment> GetPaymentByOrderIdAsync(int orderId)
        {
            return await _paymentRepository.GetPaymentByOrderIdAsync(orderId);
        }

        public async Task<List<Payment>> GetPaymentsByAppointmentIdAsync(int appointmentId)
        {
            return await _paymentRepository.GetPaymentsByAppointmentIdAsync(appointmentId);
        }

        public async Task<List<Payment>> GetPaymentsByCustomerIdAsync(int customerId)
        {
            return await _paymentRepository.GetPaymentsByCustomerIdAsync(customerId);
        }

        public async Task<List<Payment>> GetAllPaymentsAsync()
        {
            return await _paymentRepository.GetAllPaymentsAsync();
        }

        public async Task<List<Payment>> GetPaymentsByMethodAsync(string method)
        {
            return await _paymentRepository.GetPaymentsByMethodAsync(method);
        }

        public async Task<List<Payment>> SortPaymentsByMethodAsync()
        {
            return await _paymentRepository.SortPaymentsByMethodAsync();
        }

        public async Task<List<Payment>> GetPaymentsByAmountRangeAsync(decimal minAmount, decimal maxAmount)
        {
            return await _paymentRepository.GetPaymentsByAmountRangeAsync(minAmount, maxAmount);
        }

        public async Task<List<Payment>> GetPaymentsByPaidDateAsync(DateTime paidDate)
        {
            return await _paymentRepository.GetPaymentsByPaidDateAsync(paidDate);
        }

        public async Task<Payment> CreatePayment(Payment payment)
        {
            return await _paymentRepository.CreatePayment(payment);
        }
    }
}
