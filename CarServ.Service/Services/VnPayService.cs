using CarServ.Service.Services.Interfaces;
using Service.ApiModels.VNPay;
using CarServ.Service.Services.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CarServ.Service.Services.Configuration.SystemSettingModel;
using CarServ.Repository.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using CarServ.Service.Services.ApiModels.VNPay;
using CarServ.Domain.Entities;


namespace CarServ.Service.Services
{
    public class VnPayService : IVnPayService
    {
        private readonly VnPaySetting _vnPaySetting;
        private readonly IPaymentRepository _paymentRepository;

        public VnPayService(VnPaySetting vnPaySetting, IPaymentRepository paymentRepository)
        {
            _vnPaySetting = vnPaySetting;
            _paymentRepository = paymentRepository;
        }

        public async Task<string> CreatePaymentUrl(HttpContext context, VnPaymentRequest request)
        {
            string returnUrl = $"https://localhost:5110/api/paymentvnpay/payment-execute";
            string hostName = System.Net.Dns.GetHostName();
            string clientIPAddress = System.Net.Dns.GetHostAddresses(hostName).GetValue(0).ToString();

            var order = await _paymentRepository.GetPaymentByIdAsync(request.OrderId);

            if (order == null)
            {
                throw new InvalidOperationException("Order not found.");
            }

            var tick = order.PaymentId;
            var vnpay = new VnPayLibrary();

            vnpay.AddRequestData("vnp_Version", "2.1.0"); // Version
            vnpay.AddRequestData("vnp_Command", "pay"); // Command for create token
            vnpay.AddRequestData("vnp_TmnCode", _vnPaySetting.TmnCode); // Merchant code
            vnpay.AddRequestData("vnp_BankCode", "");
            vnpay.AddRequestData("vnp_Locale", "vn");
            var amount = ((int)(request.Amount * 100));

            // Ensure amount is a whole number (integer), not a decimal or float
            int amountInCents = (int)amount;  // Convert to integer

            if (amountInCents <= 0)
            {
                throw new InvalidOperationException("Invalid amount.");
            }

            vnpay.AddRequestData("vnp_Amount", amountInCents.ToString());
            DateTime createDate = DateTime.Now;
            vnpay.AddRequestData("vnp_CreateDate", createDate.ToString("yyyyMMddHHmmss"));



            // Order information
            vnpay.AddRequestData("vnp_CurrCode", "VND");
            vnpay.AddRequestData("vnp_TxnRef", tick.ToString());
            vnpay.AddRequestData("vnp_OrderInfo", $"Thanh toán đơn hàng ID: {request.OrderId}, Tổng giá trị: {order.Amount} VND");
            vnpay.AddRequestData("vnp_ReturnUrl", returnUrl);
            vnpay.AddRequestData("vnp_IpAddr", clientIPAddress);
            vnpay.AddRequestData("vnp_OrderType", "other");

            try
            {
                // Create payment entity in the repository
                var payment = new Payments
                {
                    PaymentId = tick,
                    AppointmentId = request.OrderId,
                    Amount = amountInCents,
                    PaymentMethod = "VNPay",
                    PaidAt = createDate,
                };

                _paymentRepository.CreatePayment(payment);


                // Generate the payment URL
                var paymentUrl = vnpay.CreateRequestUrl(_vnPaySetting.BaseUrl, _vnPaySetting.HashSecret);
                return paymentUrl;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error creating payment URL", ex);
            }
        }

        public async Task<VnPaymentResponse> PaymentExecute(HttpContext context)
        {
            var vnpay = new VnPayLibrary();

            // Retrieve vnp_TxnRef from the query string directly
            var vnpOrderIdStr = context.Request.Query["vnp_TxnRef"].ToString();
            int vnpOrderId = 0;

            // Validate vnpOrderId
            if (string.IsNullOrEmpty(vnpOrderIdStr) || !int.TryParse(vnpOrderIdStr, out vnpOrderId))
            {
                return new VnPaymentResponse()
                {
                    Success = false,
                    Message = "Invalid or missing order ID"
                };
            }

            var payment = await _paymentRepository.GetPaymentByIdAsync(vnpOrderId);
            if (payment == null)
            {
                return new VnPaymentResponse()
                {
                    Success = false,
                    Message = "Order not found"
                };
            }

            var request = context.Request;
            var collections = request.Query;

            // Add all parameters starting with "vnp_" to the vnpay instance
            foreach (var (key, value) in collections)
            {
                if (!string.IsNullOrEmpty(key) && key.StartsWith("vnp_"))
                {
                    vnpay.AddResponseData(key, value.ToString());
                }
            }

            // Extract necessary data from the response
            var vnpTransactionId = Convert.ToInt64(vnpay.GetResponseData("vnp_TransactionNo"));
            var vnpSecureHash = collections.FirstOrDefault(p => p.Key == "vnp_SecureHash").Value;
            var vnpResponseCode = vnpay.GetResponseData("vnp_ResponseCode");
            var vnpOrderInfo = vnpay.GetResponseData("vnp_OrderInfo");

            // Validate the signature
            bool checkSignature = vnpay.ValidateSignature(vnpSecureHash, _vnPaySetting.HashSecret);
            if (!checkSignature)
            {
                return new VnPaymentResponse()
                {
                    Success = false,
                    Message = "Invalid signature"
                };
            }

            // Handle the response based on the VNPay response code
            if (vnpResponseCode != "00") // Failed
            {
                return new VnPaymentResponse()
                {
                    Success = false,
                    Message = $"Payment failed with response code: {vnpResponseCode}"
                };
            }

            return new VnPaymentResponse()
            {
                Success = true,
                PaymentMethod = "VnPay",
                OrderDescription = vnpOrderInfo,
                OrderId = vnpOrderId.ToString(),
                TransactionId = vnpTransactionId.ToString(),
                Token = vnpSecureHash,
                PaymentId = request.QueryString.ToString(),
                VnPayResponseCode = vnpResponseCode
            };
        }
    }
}
