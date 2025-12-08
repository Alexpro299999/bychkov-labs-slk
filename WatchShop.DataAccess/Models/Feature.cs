using System.Collections.Generic;

namespace WatchShop.DataAccess.Models
{
    public class Feature
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Watch> Watches { get; set; }
    }
}