using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace AdunbiKiddies.Models
{
    [Bind(Exclude = "OrderId")]
    public partial class Sale
    {
        public int SaleId { get; set; }

        public DateTime SaleDate { get; set; }
        public string CustomerId { get; set; }
        public decimal Total { get; set; }
        public bool IsPayed { get; set; }
        public virtual Customer Customer { get; set; }
        public List<SaleDetail> SaleDetails { get; set; }


    }
}