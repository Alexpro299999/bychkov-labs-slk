using System;

namespace WatchStore.DataAccess.Models
{
    public class Stock
    {
        public int ID { get; set; }
        public int Quantity { get; set; }
        public DateTime DeliveryDate { get; set; }
        public int WatchID { get; set; }
        public virtual Watch Watch { get; set; }
    }
}