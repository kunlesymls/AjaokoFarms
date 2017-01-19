using AdunbiKiddies.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AdunbiKiddies.ViewModel
{
    public class ShoppingCartViewModel
    {
        [Key]
        public List<Cart> CartItems { get; set; }
        public decimal CartTotal { get; set; }
    }
}