using System;

namespace WatchShop.DataAccess.Models
{
    // Этот класс не является таблицей в БД.
    // Он нужен только для красивого отображения данных из таблицы Stock на экране.
    public class StockView
    {
        public int StockID { get; set; }
        public string WatchModel { get; set; }
        public int Quantity { get; set; }
        public DateTime DeliveryDate { get; set; }
    }
}