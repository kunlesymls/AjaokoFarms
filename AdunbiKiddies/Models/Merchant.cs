using OpenOrderFramework.Models;
using System.Collections.Generic;

namespace AdunbiKiddies.Models
{
    public class Merchant : Person
    {
        public string MerchantId { get; set; }

        public string CompanyName { get; set; }
        public string PreferedStoreName { get; set; }
        public bool IsReffered { get; set; }
        public string BusinessEntry { get; set; }
        public string IdCardName { get; set; }
        public string RegistrationNo { get; set; }

        public bool IsVerified { get; set; }


        public virtual PartnerShipAgreement PartnerShipAgreement { get; set; }
        public virtual ICollection<BusinessRegistration> BusinessRegistrations { get; set; }
        public virtual ICollection<BusinessAddress> BusinessAddresses { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
}