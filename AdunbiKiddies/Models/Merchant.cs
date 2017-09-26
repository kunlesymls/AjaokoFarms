using OpenOrderFramework.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace AdunbiKiddies.Models
{
    public class Merchant : Person
    {

        [DisplayName("Enter a Username")]
        public string MerchantId { get; set; }
        [DisplayName("Display Name")]
        public string CompanyName { get; set; }
        [DisplayName("Preferred Store")]
        public string PreferedStoreName { get; set; }
        [DisplayName("Were you refered ?")]
        public bool IsReffered { get; set; }
        [DisplayName("Business Entry")]
        public string BusinessEntry { get; set; }

        [DisplayName("Mode of Identification")]
        public string IdCardName { get; set; }

        [DisplayName("Company Registration Number")]
        public string RegistrationNo { get; set; }

        public bool Haspayed { get; set; }
        public DateTime ExpiryDate { get; set; }
        public bool IsVerified { get; set; }
        public virtual PartnerShipAgreement PartnerShipAgreement { get; set; }

        public virtual ICollection<BusinessRegistration> BusinessRegistrations { get; set; }
        public virtual ICollection<BusinessAddress> BusinessAddresses { get; set; }
        public virtual ICollection<Product> Products { get; set; }
        public virtual ICollection<MerchantPayment> MerchantPayments { get; set; }
    }
}