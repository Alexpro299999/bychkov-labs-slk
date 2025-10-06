using System.Collections.Generic;

namespace WatchStore.DataAccess.Models
{
    public class Manufacturer
    {
        public int ID { get; set; }
        public string ManufacturerName { get; set; }
        public string Country { get; set; }

        public virtual ICollection<Watch> Watches { get; set; }
    }
}