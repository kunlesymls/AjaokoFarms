using AdunbiKiddies.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OpenOrderFramework.Models
{
    public class PartnerShipAgreement
    {
        [Key, ForeignKey("Merchant")]
        public string MerchantId { get; set; }
        public int CategoryId { get; set; }
        [DisplayName("Can you Drop your product with Ajaoko?")]
        public bool CanDropProduct { get; set; }
        [DisplayName("Can you Stock Product with Ajaoko?")]
        public bool CanStockProduct { get; set; }
        [DisplayName("Are you selling the Product Elsewhere?")]
        public bool IsSellingElsewhere { get; set; }
        public string WebisteLink { get; set; }
        public int ProductList { get; set; }
        public virtual Category Category { get; set; }
        public virtual Merchant Merchant { get; set; }

    }
}