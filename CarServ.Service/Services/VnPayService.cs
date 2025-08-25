using CarServ.Repository.Repositories.DTO.Payment;
using CarServ.Repository.Repositories.Interfaces;
using CarServ.service.Services.ApiModels.VNPay;
using CarServ.service.Services.Configuration;
using CarServ.service.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;


namespace CarServ.service.Services
{
    public class VnPayService : IVnPayService
    {
        private readonly VnPaySetting _vnPaySetting;
        private readonly IOrderRepository _orderRepository;
        private readonly IPaymentRepository _paymentRepository;
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<VnPayService> _logger;

        public VnPayService(IServiceProvider serviceProvider)
        {
            _vnPaySetting = VnPaySetting.Instance;
            _orderRepository = serviceProvider.GetRequiredService<IOrderRepository>();
            _paymentRepository = serviceProvider.GetRequiredService<IPaymentRepository>();
            _appointmentRepository = serviceProvider.GetRequiredService<IAppointmentRepository>();
            _unitOfWork = serviceProvider.GetRequiredService<IUnitOfWork>();
        }

        public async Task<string> CreatePaymentUrl(HttpContext context, VnPaymentRequest request)
        {
            string returnUrl = $"http://localhost:5110/api/Payment/payment/vnpay/payment-execute";
            string hostName = System.Net.Dns.GetHostName();
            string clientIPAddress = System.Net.Dns.GetHostAddresses(hostName).GetValue(0).ToString();

            var order = await _orderRepository.GetOrderByIdAsync(request.OrderId);

            if (order == null)
            {
                throw new InvalidOperationException("Order not found.");
            }

            _unitOfWork.BeginTransaction();
            var orderId = order.OrderId;
            var vnpay = new VnPayLibrary();

            vnpay.AddRequestData("vnp_Version", "2.1.0"); // Version
            vnpay.AddRequestData("vnp_Command", "pay"); // Command for create token
            vnpay.AddRequestData("vnp_TmnCode", _vnPaySetting.TmnCode); // Merchant code
            vnpay.AddRequestData("vnp_BankCode", "");
            vnpay.AddRequestData("vnp_Locale", "vn");
            var amount = ((int)(request.Amount * 100));

            //Ensure amount is a whole number(integer), not a decimal or float
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
            vnpay.AddRequestData("vnp_TxnRef", orderId.ToString());
            vnpay.AddRequestData("vnp_OrderInfo", $"Thanh toán đơn hàng ID: {orderId}, Tổng giá trị: {request.Amount} VND");
            vnpay.AddRequestData("vnp_ReturnUrl", returnUrl);
            vnpay.AddRequestData("vnp_IpAddr", clientIPAddress);
            vnpay.AddRequestData("vnp_OrderType", "other");

            try
            {
                // Create payment entity in the repository
                var payment = new PaymentDto
                {
                    AppointmentId = order.AppointmentId,
                    Amount = request.Amount,
                    PaymentMethod = "VNPay",
                    PaidAt = createDate,
                    Status = "Pending",
                    OrderId = request.OrderId
                };

                await _paymentRepository.CreatePayment(payment);
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();

                // Generate the payment URL
                var paymentUrl = vnpay.CreateRequestUrl(_vnPaySetting.BaseUrl, _vnPaySetting.HashSecret);

                return paymentUrl;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating payment URL for order ID {OrderId}", request.OrderId);
                await _unitOfWork.RollbackTransactionAsync();
                throw;

                //throw new InvalidOperationException("Error creating payment URL", ex);
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

            var order = await _orderRepository.GetOrderByIdAsync(vnpOrderId);
            var payment = await _paymentRepository.GetPaymentByOrderIdAsync(vnpOrderId);
            var appointment = await _appointmentRepository.GetAppointmentByOrderIdAsync(vnpOrderId);
            if (order == null)
            {
                await _unitOfWork.RollbackTransactionAsync();
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
                await _paymentRepository.RemoveAsync(payment);
                return new VnPaymentResponse()
                {
                    Success = false,
                    Message = $"Payment failed with response code: {vnpResponseCode}"
                };
            }
            else
            {
                // Update relevant entities on successful payment
                if (payment != null)
                {
                    payment.Status = "Paid";
                    payment.PaidAt = DateTime.Now;
                    appointment.Status = "Completed";
                    await _paymentRepository.UpdateAsync(payment);
                    await _appointmentRepository.UpdateAsync(appointment);
                }
            }

            await _unitOfWork.SaveChangesAsync();
            await _unitOfWork.CommitTransactionAsync();
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
