using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AdunbiKiddies.Models
{
    public class Supplier
    {
        public int Id { get; set; }

        [Display(Name = "Supplier's Name")]
        public string SupplierName { get; set; }

        [Display(Name = "Mobile Number")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        [Display(Name = "Address")]
        [DataType(DataType.MultilineText)]
        [Required(ErrorMessage = "Your Address is required")]
        [StringLength(50, ErrorMessage = "Your Address name is too long")]
        public string Address { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
}