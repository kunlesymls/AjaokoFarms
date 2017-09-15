using System;
using System.ComponentModel.DataAnnotations;

namespace AdunbiKiddies.Models
{
    public class Stock
    {
        public int StockId { get; set; }

        [Display(Name = "Product Name")]
        public int ProductId { get; set; }
        public String Name { get; set; }

        [Display(Name = "Quantity")]
        public int Quantity { get; set; }

        [Display(Name = "Date Of Creation")]
        [Required(ErrorMessage = "The Date is required")]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        public PopUp.Status Status { get; set; }

        [Display(Name = "Staff ID")]
        public string StaffName { get; set; }

        public virtual Product Product { get; set; }
    }
}