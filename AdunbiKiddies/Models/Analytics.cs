using AdunbiKiddies.ViewModels;
using System.Collections.Generic;

namespace AdunbiKiddies.Models
{
    public class AnalyticsViewModel
    {
        public List<SaleDateGroup> SaleDate { get; set; }

        public List<SaleDateGroup> SaleDataForToday { get; set; }
    }
}