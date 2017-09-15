using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AdunbiKiddies.Models
{
    public class Category
    {
        [Key]
        [DisplayName("Category ID")]
        public int CategoryId { get; set; }

        public int StoreSectionId { get; set; }

        [DisplayName("Category")]
        public string Name { get; set; }

        public virtual ICollection<Product> Products { get; set; }
        public virtual StoreSection StoreSection { get; set; }
    }
}