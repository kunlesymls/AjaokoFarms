using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AdunbiKiddies.Models
{
    public class Stock
    {
        public int ID { get; set; }

        [Display(Name = "Product Name")]
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

        public virtual List<Product> Products { get; set; }
    }
}