using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CarServ.Domain.Entities;
using CarServ.service.Services.Interfaces;
using CarServ.Repository.Repositories.DTO.Payment;
using CarServ.service.Services;
using CarServ.service.Services.ApiModels.VNPay;

namespace CarServ.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _Paymentervice;
        private readonly IVnPayService _vnPayService;

        public PaymentController(IPaymentService Paymentervice, IVnPayService vnPayService)
        {
            _Paymentervice = Paymentervice;
            _vnPayService = vnPayService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Payment>>> GetPayment()
        {
            var Payment = await _Paymentervice.GetAllPaymentAsync();
            return Ok(Payment);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Payment>> GetPaymentById(int id)
        {
            var payment = await _Paymentervice.GetPaymentByIdAsync(id);
            if (payment == null)
            {
                return NotFound();
            }
            return Ok(payment);
        }

        [HttpGet("customer/{customerId}")]
        public async Task<ActionResult<IEnumerable<Payment>>> GetPaymentByCustomerId(int customerId)
        {
            var Payment = await _Paymentervice.GetPaymentByCustomerIdAsync(customerId);
            if (Payment == null || !Payment.Any())
            {
                return NotFound();
            }
            return Ok(Payment);
        }

        [HttpGet("appointment/{appointmentId}")]
        public async Task<ActionResult<IEnumerable<Payment>>> GetPaymentByAppointmentId(int appointmentId)
        {
            var Payment = await _Paymentervice.GetPaymentByAppointmentIdAsync(appointmentId);
            if (Payment == null || !Payment.Any())
            {
                return NotFound();
            }
            return Ok(Payment);
        }

        [HttpGet("method")]
        public async Task<ActionResult<IEnumerable<Payment>>> GetPaymentByMethod([FromQuery] string method)
        {
            var Payment = await _Paymentervice.GetPaymentByMethodAsync(method);
            if (Payment == null || !Payment.Any())
            {
                return NotFound();
            }
            return Ok(Payment);
        }

        [HttpGet("sort/method")]
        public async Task<ActionResult<IEnumerable<Payment>>> SortPaymentByMethod()
        {
            var Payment = await _Paymentervice.SortPaymentByMethodAsync();
            if (Payment == null || !Payment.Any())
            {
                return NotFound();
            }
            return Ok(Payment);
        }

        [HttpGet("amount-range")]
        public async Task<ActionResult<IEnumerable<Payment>>> GetPaymentByAmountRange([FromQuery] decimal minAmount, [FromQuery] decimal maxAmount)
        {
            var Payment = await _Paymentervice.GetPaymentByAmountRangeAsync(minAmount, maxAmount);
            if (Payment == null || !Payment.Any())
            {
                return NotFound();
            }
            return Ok(Payment);
        }

        [HttpGet("paid-date")]
        public async Task<ActionResult<IEnumerable<Payment>>> GetPaymentByPaidDate([FromQuery] DateTime paidDate)
        {
            var Payment = await _Paymentervice.GetPaymentByPaidDateAsync(paidDate);
            if (Payment == null || !Payment.Any())
            {
                return NotFound();
            }
            return Ok(Payment);
        }

        [HttpPost("create")]
        public async Task<ActionResult<Payment>> CreatePayment([FromBody] Payment payment)
        {
            if (payment == null)
            {
                return BadRequest("Payment cannot be null.");
            }
            var createdPayment = await _Paymentervice.CreatePayment(payment);
            return CreatedAtAction(nameof(GetPaymentById), new { id = createdPayment.PaymentId }, createdPayment);
        }

        [HttpPost("payment/vnpay/payment-url")]
        public async Task<IActionResult> VnPayCreatePaymentUrl([FromBody] VnPaymentRequest request)
        {
            var response = await _vnPayService.CreatePaymentUrl(HttpContext, request);
            return Ok(response);
        }

        [HttpPost("payment/vnpay/payment-execute")]
        public async Task<IActionResult> VnPayPaymentExecute()
        {
            var response = await _vnPayService.PaymentExecute(HttpContext);

            return Ok(response);
        }

        private async Task<bool> PaymentExists(int id)
        {
            return await _Paymentervice.GetPaymentByIdAsync(id) != null;
        }
    }
}
