using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CarServ.Domain.Entities;
using CarServ.Service.Services.Interfaces;
using CarServ.Repository.Repositories.DTO.PayOS;

namespace CarServ.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Payments>>> GetPayments()
        {
            var payments = await _paymentService.GetAllPaymentsAsync();
            return Ok(payments);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Payments>> GetPaymentById(int id)
        {
            var payment = await _paymentService.GetPaymentByIdAsync(id);
            if (payment == null)
            {
                return NotFound();
            }
            return Ok(payment);
        }

        [HttpGet("customer/{customerId}")]
        public async Task<ActionResult<IEnumerable<Payments>>> GetPaymentsByCustomerId(int customerId)
        {
            var payments = await _paymentService.GetPaymentsByCustomerIdAsync(customerId);
            if (payments == null || !payments.Any())
            {
                return NotFound();
            }
            return Ok(payments);
        }

        [HttpGet("appointment/{appointmentId}")]
        public async Task<ActionResult<IEnumerable<Payments>>> GetPaymentsByAppointmentId(int appointmentId)
        {
            var payments = await _paymentService.GetPaymentsByAppointmentIdAsync(appointmentId);
            if (payments == null || !payments.Any())
            {
                return NotFound();
            }
            return Ok(payments);
        }

        //[HttpPost]
        //public async Task<ActionResult<Payments>> CreateOnlinePayment(PayOSPaymentRequest request)
        //{
        //    if (request == null)
        //    {
        //        return BadRequest("Payment request cannot be null.");
        //    }

        //    var payment = await _paymentService.CreateOnlinePaymentAsync(request);
        //    if (payment == null)
        //    {
        //        return BadRequest("Failed to create payment.");
        //    }

        //    return CreatedAtAction(nameof(GetPaymentById), new { id = payment.PaymentId }, payment);
        //}

        private async Task<bool> PaymentsExists(int id)
        {
            return await _paymentService.GetPaymentByIdAsync(id) != null;
        }
    }
}
