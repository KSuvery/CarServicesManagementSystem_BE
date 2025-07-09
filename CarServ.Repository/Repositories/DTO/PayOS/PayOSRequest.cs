using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;

namespace CarServ.Repository.Repositories.DTO.PayOS
{
    public class PayOSPaymentRequest
    {
        public decimal Amount { get; set; }
        public string Description { get; set; }
        public string ReturnUrl { get; set; }
        public string CancelUrl { get; set; }
    }
}
