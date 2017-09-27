using AdunbiKiddies.Models;

namespace AdunbiKiddies.ViewModels
{
    public class OrderPaymentVm
    {
        public string CustomerId { get; set; }
        public string CustomerName { get; set; }
        public decimal TotalAmount { get; set; }
        public string Email { get; set; }
       // public int SaleId { get; set; }
        public ShippingDetail ShippingDetail { get; set; }
    }
}