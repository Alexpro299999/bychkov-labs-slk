using System;

namespace WatchStore.DataAccess.Models
{
    public class DailyProductionCost
    {
        public string ManufacturerName { get; set; }
        public DateTime DeliveryDate { get; set; }
        public decimal TotalCost { get; set; }
    }
}