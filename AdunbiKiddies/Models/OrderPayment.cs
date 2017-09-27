using System;

namespace AdunbiKiddies.Models
{
    public class OrderPayment
    {
        public int OrderPaymentId { get; set; }
        public string CustomerId { get; set; }
        public int SaleId { get; set; }
        public DateTime PaymentDateTime { get; set; }
        public bool IsPayed { get; set; }

        public string Status
        {
            get
            {
                if (IsPayed)
                {
                    return "Payment Confirmed";
                }
                return "Payment Not Received";
            }
            set { }
        }
        public decimal Amount { get; set; }
        public decimal AmountPaid { get; set; }
        public virtual Customer Customer { get; set; }

    }
}