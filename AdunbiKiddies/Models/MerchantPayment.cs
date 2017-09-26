using System;

namespace AdunbiKiddies.Models
{
    public class MerchantPayment
    {
        public int MerchantPaymentId { get; set; }
        public string MerchantId { get; set; }
        public decimal AmountPayed { get; set; }
        public decimal ExpectedAmount { get; set; }
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

        public virtual Merchant Merchant { get; set; }

    }

    public class MerchantPaymentVm
    {
        public decimal Amount { get; set; }
        public string MerchantName { get; set; }
        public string MerchantId { get; set; }
    }
}