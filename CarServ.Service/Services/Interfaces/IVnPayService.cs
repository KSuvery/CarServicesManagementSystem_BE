using Microsoft.AspNetCore.Http;
using Service.ApiModels.VNPay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarServ.Service.Services.Interfaces
{
    public interface IVnPayService
    {
        Task<string> CreatePaymentUrl(HttpContext context, VnPaymentRequest request);
        Task<VnPaymentResponse> PaymentExecute(HttpContext context);
    }
}
