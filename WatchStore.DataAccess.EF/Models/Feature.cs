using System.Collections.Generic;

namespace WatchStore.DataAccess.Models
{
    public class Feature
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Watch> Watches { get; set; }
    }
}