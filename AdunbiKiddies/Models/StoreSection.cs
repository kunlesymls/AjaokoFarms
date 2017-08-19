using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdunbiKiddies.Models
{
    public class StoreSection
    {
        public int StoreSectionId { get; set; }
        public string SectionName { get; set; }
        public virtual ICollection<Category> Categories { get; set; }
        public virtual ICollection<AssignSection> AssignSections { get; set; }
    }
}