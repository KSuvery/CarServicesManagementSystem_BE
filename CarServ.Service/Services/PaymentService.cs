using CarServ.Domain.Entities;
using CarServ.Repository.Repositories.DTO.Payment;
using CarServ.Repository.Repositories.Interfaces;
using CarServ.Service.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarServ.Service.Services
{
    public class Paymentervice : IPaymentervice
    {
        private readonly IPaymentRepository _paymentRepository;

        public Paymentervice(IPaymentRepository paymentRepository)
        {
            _paymentRepository = paymentRepository;
        }

        public async Task<Payment> GetPaymentByIdAsync(int paymentId)
        {
            return await _paymentRepository.GetPaymentByIdAsync(paymentId);
        }

        public async Task<List<Payment>> GetPaymentByAppointmentIdAsync(int appointmentId)
        {
            return await _paymentRepository.GetPaymentByAppointmentIdAsync(appointmentId);
        }

        public async Task<List<Payment>> GetPaymentByCustomerIdAsync(int customerId)
        {
            return await _paymentRepository.GetPaymentByCustomerIdAsync(customerId);
        }

        public async Task<List<Payment>> GetAllPaymentAsync()
        {
            return await _paymentRepository.GetAllPaymentAsync();
        }

        public async Task<List<Payment>> GetPaymentByMethodAsync(string method)
        {
            return await _paymentRepository.GetPaymentByMethodAsync(method);
        }

        public async Task<List<Payment>> SortPaymentByMethodAsync()
        {
            return await _paymentRepository.SortPaymentByMethodAsync();
        }

        public async Task<List<Payment>> GetPaymentByAmountRangeAsync(decimal minAmount, decimal maxAmount)
        {
            return await _paymentRepository.GetPaymentByAmountRangeAsync(minAmount, maxAmount);
        }

        public async Task<List<Payment>> GetPaymentByPaidDateAsync(DateTime paidDate)
        {
            return await _paymentRepository.GetPaymentByPaidDateAsync(paidDate);
        }

        public async Task<Payment> CreatePayment(Payment payment)
        {
            return await _paymentRepository.CreatePayment(payment);
        }

        public async Task<Payment> ProcessPayment(PaymentDto dto)
        {
            return await _paymentRepository.ProcessPayment(dto);
        }
    }
}
