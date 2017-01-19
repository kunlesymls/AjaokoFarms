using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace AdunbiKiddies.Models
{
    public class Categories
    {
        [Key]
        [DisplayName("Categories ID")]
        public int ID { get; set; }

        [DisplayName("Categories")]
        public string Name { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}