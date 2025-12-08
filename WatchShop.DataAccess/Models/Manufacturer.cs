using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WatchShop.DataAccess.Models
{
    public class Manufacturer
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(50)]
        public string Country { get; set; }

        public ICollection<Watch> Watches { get; set; }
    }
}