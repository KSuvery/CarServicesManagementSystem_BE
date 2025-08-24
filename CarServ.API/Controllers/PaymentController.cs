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
        private readonly IPaymentService _PaymentService;
        private readonly IVnPayService _vnPayService;

        public PaymentController(IPaymentService Paymentervice, IVnPayService vnPayService)
        {
            _PaymentService = Paymentervice;
            _vnPayService = vnPayService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Payment>>> GetPayment()
        {
            var Payment = await _PaymentService.GetAllPaymentAsync();
            return Ok(Payment);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Payment>> GetPaymentById(int id)
        {
            var payment = await _PaymentService.GetPaymentByIdAsync(id);
            if (payment == null)
            {
                return NotFound();
            }
            return Ok(payment);
        }

        [HttpGet("customer/{customerId}")]
        public async Task<ActionResult<IEnumerable<Payment>>> GetPaymentByCustomerId(int customerId)
        {
            var Payment = await _PaymentService.GetPaymentByCustomerIdAsync(customerId);
            if (Payment == null || !Payment.Any())
            {
                return NotFound();
            }
            return Ok(Payment);
        }

        [HttpGet("appointment/{appointmentId}")]
        public async Task<ActionResult<IEnumerable<Payment>>> GetPaymentByAppointmentId(int appointmentId)
        {
            var Payment = await _PaymentService.GetPaymentByAppointmentIdAsync(appointmentId);
            if (Payment == null || !Payment.Any())
            {
                return NotFound();
            }
            return Ok(Payment);
        }

        [HttpGet("method")]
        public async Task<ActionResult<IEnumerable<Payment>>> GetPaymentByMethod([FromQuery] string method)
        {
            var Payment = await _PaymentService.GetPaymentByMethodAsync(method);
            if (Payment == null || !Payment.Any())
            {
                return NotFound();
            }
            return Ok(Payment);
        }

        [HttpGet("sort/method")]
        public async Task<ActionResult<IEnumerable<Payment>>> SortPaymentByMethod()
        {
            var Payment = await _PaymentService.SortPaymentByMethodAsync();
            if (Payment == null || !Payment.Any())
            {
                return NotFound();
            }
            return Ok(Payment);
        }

        [HttpGet("amount-range")]
        public async Task<ActionResult<IEnumerable<Payment>>> GetPaymentByAmountRange([FromQuery] decimal minAmount, [FromQuery] decimal maxAmount)
        {
            var Payment = await _PaymentService.GetPaymentByAmountRangeAsync(minAmount, maxAmount);
            if (Payment == null || !Payment.Any())
            {
                return NotFound();
            }
            return Ok(Payment);
        }

        [HttpGet("paid-date")]
        public async Task<ActionResult<IEnumerable<Payment>>> GetPaymentByPaidDate([FromQuery] DateTime paidDate)
        {
            var Payment = await _PaymentService.GetPaymentByPaidDateAsync(paidDate);
            if (Payment == null || !Payment.Any())
            {
                return NotFound();
            }
            return Ok(Payment);
        }

        [HttpPost("create")]
        public async Task<ActionResult<Payment>> CreatePayment([FromBody] PaymentDto dto)
        {
            if (dto == null)
            {
                return BadRequest("Payment cannot be null.");
            }
            var createdPayment = await _PaymentService.CreatePayment(dto);
            return CreatedAtAction(nameof(GetPaymentById), new { id = createdPayment.PaymentId }, createdPayment);
        }

        [HttpGet("status")]
        public async Task<ActionResult<IEnumerable<Payment>>> GetPaymentsByStatus([FromQuery] string status)
        {
            var payments = await _PaymentService.GetPaymentsByStatus(status);
            if (payments == null || !payments.Any())
            {
                return NotFound();
            }
            return Ok(payments);
        }

        [HttpGet("status/pending")]
        public async Task<ActionResult<IEnumerable<Payment>>> GetPendingPayments()
        {
            var payments = await _PaymentService.GetPendingPayments();
            if (payments == null || !payments.Any())
            {
                return NotFound();
            }
            return Ok(payments);
        }

        [HttpGet("status/paid")]
        public async Task<ActionResult<IEnumerable<Payment>>> GetPaidPayments()
        {
            var payments = await _PaymentService.GetPaidPayments();
            if (payments == null || !payments.Any())
            {
                return NotFound();
            }
            return Ok(payments);
        }

        [HttpPost("payment/vnpay/payment-url")]
        public async Task<IActionResult> VnPayCreatePaymentUrl([FromBody] VnPaymentRequest request)
        {
            var response = await _vnPayService.CreatePaymentUrl(HttpContext, request);
            return Ok(response);
        }

        [HttpGet("payment/vnpay/payment-execute")]
        public async Task<IActionResult> VnPayPaymentExecute()
        {
            var response = await _vnPayService.PaymentExecute(HttpContext);

            if (response == null)
            {
                return BadRequest("Payment execution failed.");
            }

            return Ok(response);
        }

        private async Task<bool> PaymentExists(int id)
        {
            return await _PaymentService.GetPaymentByIdAsync(id) != null;
        }
    }
}
