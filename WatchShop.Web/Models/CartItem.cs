using System.ComponentModel.DataAnnotations;

namespace WatchShop.Web.Models
{
    public class CartItem
    {
        public int WatchId { get; set; }
        public string WatchName { get; set; }
        public string ImageUrl { get; set; }
        public decimal Price { get; set; }

        [Range(1, 100, ErrorMessage = "Количество должно быть от 1 до 100")]
        public int Quantity { get; set; }

        public decimal Total => Price * Quantity;
    }
}