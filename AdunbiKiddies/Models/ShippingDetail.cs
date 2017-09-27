using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdunbiKiddies.Models
{
    public class ShippingDetail
    {
        [Key, ForeignKey("Customer")]
        public string CustomerId { get; set; }
        public string AddressType { get; set; }
        [Required]
        public string BuildingNo { get; set; }
        [Required]
        public string StreetName { get; set; }
        [Required]
        public string TownName { get; set; }
        [Required]
        public string StateName { get; set; }
        public virtual Customer Customer { get; set; }

    }

    public class Customer : Person
    {
        public string CustomerId { get; set; }
        public virtual ShippingDetail ShippingDetail { get; set; }
        public virtual ICollection<Sale> Sales { get; set; }
    }
}