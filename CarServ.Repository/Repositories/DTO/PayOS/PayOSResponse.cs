using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;

namespace CarServ.Repository.Repositories.DTO.PayOS
{
    public class PayOSPaymentResponse
    {
        public string PaymentUrl { get; set; }
        public string TransactionId { get; set; }
    }
}
