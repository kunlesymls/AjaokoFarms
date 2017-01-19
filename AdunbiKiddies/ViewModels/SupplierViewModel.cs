using System.ComponentModel.DataAnnotations;

namespace AdunbiKiddies.ViewModels
{
    public class SupplierViewModel
    {
        [Display(Name = "Supplier Name")]
        [Required(ErrorMessage = "Your First name is required")]
        [StringLength(40, ErrorMessage = "Your First name is too long")]
        public string SupplierName { get; set; }

        [Display(Name = "Address")]
        [DataType(DataType.MultilineText)]
        [Required(ErrorMessage = "Your Address is required")]
        [StringLength(50, ErrorMessage = "Your Address name is too long")]
        public string Address { get; set; }
    }
}