namespace WatchStore.DataAccess.Models
{
    public class Watch
    {
        public int ID { get; set; }
        public string WatchModel { get; set; }
        public string WatchType { get; set; }
        public decimal Price { get; set; }
        public int ManufacturerID { get; set; }
    }
}