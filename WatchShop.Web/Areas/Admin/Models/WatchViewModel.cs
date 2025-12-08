using System.ComponentModel.DataAnnotations;

namespace WatchShop.Web.Areas.Admin.Models
{
    public class WatchViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Название обязательно")]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(1000)]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Цена обязательна")]
        [Range(0.01, 1000000.00, ErrorMessage = "Цена должна быть положительной")]
        public decimal Price { get; set; }

        public string? ImageUrl { get; set; }

        public IFormFile? ImageFile { get; set; } 

        [Required(ErrorMessage = "Категория обязательна")]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Производитель обязателен")]
        public int ManufacturerId { get; set; }
    }
}