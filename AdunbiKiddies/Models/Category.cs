using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

       // public ICollection<SubCategory> SubCategories { get; set; }

        public virtual ICollection<Product> Products { get; set; }
        public virtual StoreSection StoreSection { get; set; }
    }

   
    //public  class SubCategory
    //{
    //    [Key, ForeignKey("Category")]
    //    public int CategoryId { get; set; }

    //    [DisplayName("Sub Category")]
    //    public string Name { get; set; }

    //    public int StoreSectionId { get; set; }    
    //    public virtual ICollection<Product> Products { get; set; }
    //    public virtual StoreSection StoreSection { get; set; }

    //    public virtual Category Category { get; set; }


    //}
}