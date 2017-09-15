using System.Collections.Generic;

namespace AdunbiKiddies.Models
{
    public class BusinessRegistration
    {
        public int BusinessRegistrationId { get; set; }
        public string MerchantId { get; set; }
        public string RegistrationNo { get; set; }
        public string LegalName { get; set; }
        public Merchant Merchant { get; set; }
        public ICollection<BusinessDocument> BusinessDocuments { get; set; }

    }
}