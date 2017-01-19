using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AdunbiKiddies.ViewModels
{
    public class SaleDateGroup
    {
        [DataType(DataType.Date)]
        public DateTime? SaleDate { get; set; }

        public int SaleCount { get; set; }
    }
}