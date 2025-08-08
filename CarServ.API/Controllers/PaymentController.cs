using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CarServ.Domain.Entities;
using CarServ.Service.Services.Interfaces;
using Service.ApiModels.VNPay;

namespace CarServ.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        private readonly IVnPayService _vnPayService;

        public PaymentController(IPaymentService paymentService, IVnPayService vnPayService)
        {
            _paymentService = paymentService;
            _vnPayService = vnPayService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Payment>>> GetPayments()
        {
            var payments = await _paymentService.GetAllPaymentsAsync();
            return Ok(payments);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Payment>> GetPaymentById(int id)
        {
            var payment = await _paymentService.GetPaymentByIdAsync(id);
            if (payment == null)
            {
                return NotFound();
            }
            return Ok(payment);
        }

        [HttpGet("order/{orderId}")]
        public async Task<ActionResult<Payment>> GetPaymentByOrderId(int orderId)
        {
            var payment = await _paymentService.GetPaymentByOrderIdAsync(orderId);
            if (payment == null)
            {
                return NotFound();
            }
            return Ok(payment);
        }

        [HttpGet("customer/{customerId}")]
        public async Task<ActionResult<IEnumerable<Payment>>> GetPaymentsByCustomerId(int customerId)
        {
            var payments = await _paymentService.GetPaymentsByCustomerIdAsync(customerId);
            if (payments == null || !payments.Any())
            {
                return NotFound();
            }
            return Ok(payments);
        }

        [HttpGet("appointment/{appointmentId}")]
        public async Task<ActionResult<IEnumerable<Payment>>> GetPaymentsByAppointmentId(int appointmentId)
        {
            var payments = await _paymentService.GetPaymentsByAppointmentIdAsync(appointmentId);
            if (payments == null || !payments.Any())
            {
                return NotFound();
            }
            return Ok(payments);
        }

        [HttpGet("method")]
        public async Task<ActionResult<IEnumerable<Payment>>> GetPaymentsByMethod([FromQuery] string method)
        {
            var payments = await _paymentService.GetPaymentsByMethodAsync(method);
            if (payments == null || !payments.Any())
            {
                return NotFound();
            }
            return Ok(payments);
        }

        [HttpGet("sort/method")]
        public async Task<ActionResult<IEnumerable<Payment>>> SortPaymentsByMethod()
        {
            var payments = await _paymentService.SortPaymentsByMethodAsync();
            if (payments == null || !payments.Any())
            {
                return NotFound();
            }
            return Ok(payments);
        }

        [HttpGet("amount-range")]
        public async Task<ActionResult<IEnumerable<Payment>>> GetPaymentsByAmountRange([FromQuery] decimal minAmount, [FromQuery] decimal maxAmount)
        {
            var payments = await _paymentService.GetPaymentsByAmountRangeAsync(minAmount, maxAmount);
            if (payments == null || !payments.Any())
            {
                return NotFound();
            }
            return Ok(payments);
        }

        [HttpGet("paid-date")]
        public async Task<ActionResult<IEnumerable<Payment>>> GetPaymentsByPaidDate([FromQuery] DateTime paidDate)
        {
            var payments = await _paymentService.GetPaymentsByPaidDateAsync(paidDate);
            if (payments == null || !payments.Any())
            {
                return NotFound();
            }
            return Ok(payments);
        }

        [HttpPost("create")]
        public async Task<ActionResult<Payment>> CreatePayment([FromBody] Payment payment)
        {
            if (payment == null)
            {
                return BadRequest("Payment cannot be null.");
            }
            var createdPayment = await _paymentService.CreatePayment(payment);
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

        private async Task<bool> PaymentsExists(int id)
        {
            return await _paymentService.GetPaymentByIdAsync(id) != null;
        }
    }
}
