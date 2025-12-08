namespace WatchShop.Web.Models
{
    public class ShoppingCartViewModel
    {
        public List<CartItem> CartItems { get; set; } = new List<CartItem>();
        public decimal Total { get; set; }
    }
}