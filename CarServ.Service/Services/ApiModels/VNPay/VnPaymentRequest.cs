namespace Service.ApiModels.VNPay
{
    public class VnPaymentRequest
    {
        public decimal Amount { get; set; }
        public int OrderId { get; set; }

    }
}
