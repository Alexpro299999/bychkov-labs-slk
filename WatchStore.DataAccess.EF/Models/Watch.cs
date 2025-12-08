using System.Collections.Generic;

namespace WatchShop.DataAccess.Models
{
    public class Watch
    {
        public int ID { get; set; }
        public string WatchModel { get; set; }
        public string WatchType { get; set; }
        public decimal Price { get; set; }

        public int ManufacturerID { get; set; }
        public virtual Manufacturer Manufacturer { get; set; }

        public virtual ICollection<Stock> StockEntries { get; set; }

        public virtual ICollection<Feature> Features { get; set; }
    }
}