using OpenOrderFramework.Models;
using System.ComponentModel;

namespace AdunbiKiddies.Models
{
    public class BusinessAddress
    {
        public int BusinessAddressId { get; set; }
        [DisplayName("Merchant Name")]
        public string MerchantId { get; set; }
        [DisplayName("Address Type")]
        public string AddressType { get; set; }
        [DisplayName("Building Number")]
        public string BuildingNo { get; set; }
        [DisplayName("Street Name")]
        public string StreetName { get; set; }
        [DisplayName("Town Name")]
        public string TownName { get; set; }
        [DisplayName("State")]
        public string StateName { get; set; }
        public virtual Merchant Merchant { get; set; }
    }
    public class ProfessionalAddress
    {
        public string ProfessionalAddressId { get; set; }
        [DisplayName("Professional Name")]
        public string ProfessionalWorkerId { get; set; }
        [DisplayName("Address Type")]
        public string AddressType { get; set; }
        [DisplayName("Building Number")]
        public string BuildingNo { get; set; }
        [DisplayName("Street Name")]
        public string StreetName { get; set; }
        [DisplayName("Town Name")]
        public string TownName { get; set; }
        [DisplayName("State")]
        public string StateName { get; set; }
        public virtual ProfessionalWorker ProfessionalWorker { get; set; }
    }
}