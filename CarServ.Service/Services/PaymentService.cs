using CarServ.Domain.Entities;
using CarServ.Repository.Repositories.DTO.Payment;
using CarServ.Repository.Repositories.Interfaces;
using CarServ.service.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarServ.service.Services
{
    public class Paymentervice : IPaymentService
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
            return await _paymentRepository.GetPaymentsByAppointmentIdAsync(appointmentId);
        }

        public async Task<List<Payment>> GetPaymentByCustomerIdAsync(int customerId)
        {
            return await _paymentRepository.GetPaymentsByCustomerIdAsync(customerId);
        }

        public async Task<List<Payment>> GetAllPaymentAsync()
        {
            return await _paymentRepository.GetAllPaymentsAsync();
        }

        public async Task<List<Payment>> GetPaymentByMethodAsync(string method)
        {
            return await _paymentRepository.GetPaymentsByMethodAsync(method);
        }

        public async Task<List<Payment>> SortPaymentByMethodAsync()
        {
            return await _paymentRepository.SortPaymentsByMethodAsync();
        }

        public async Task<List<Payment>> GetPaymentByAmountRangeAsync(decimal minAmount, decimal maxAmount)
        {
            return await _paymentRepository.GetPaymentsByAmountRangeAsync(minAmount, maxAmount);
        }

        public async Task<List<Payment>> GetPaymentByPaidDateAsync(DateTime paidDate)
        {
            return await _paymentRepository.GetPaymentsByPaidDateAsync(paidDate);
        }

        public async Task<List<Payment>> GetPaymentsByStatus(string status)
        {
            return await _paymentRepository.GetPaymentsByStatus(status);
        }

        public async Task<List<Payment>> GetPendingPayments(string status = "Pending")
        {
            return await _paymentRepository.GetPendingPayments(status);
        }

        public async Task<List<Payment>> GetPaidPayments(string status = "Paid")
        {
            return await _paymentRepository.GetPaidPayments(status);
        }

        public async Task<Payment> CreatePayment(PaymentDto dto)
        {
            return await _paymentRepository.CreatePayment(dto);
        }
    }
}
