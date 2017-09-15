using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AdunbiKiddies.Models;

namespace OpenOrderFramework.Models
{
    public class PartnerShipAgreement
    {
        [Key, ForeignKey("Merchant")]
        public string MerchantId { get; set; }
        public int CategoryId { get; set; }
        public bool CanDropProduct { get; set; }
        public bool CanStockProduct { get; set; }
        public bool IsSellingElsewhere { get; set; }
        public string WebisteLink { get; set; }
        public int ProductList { get; set; }
        public virtual Category Category { get; set; }
        public virtual Merchant Merchant { get; set; }

    }
}