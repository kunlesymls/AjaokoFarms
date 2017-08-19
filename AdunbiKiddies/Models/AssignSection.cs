using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdunbiKiddies.Models
{
    public class AssignSection
    {
        public int AssignSectionId { get; set; }
        public int StoreSectionId { get; set; }
        public string UserName { get; set; }
        public virtual StoreSection StoreSection { get; set; }
    }
}