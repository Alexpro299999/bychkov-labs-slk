using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;
using WatchShop.DataAccess.Models;

namespace WatchShop.DataAccess.Repositories
{
    public class StockRepository
    {
        public List<Stock> GetAll()
        {
            var stockItems = new List<Stock>();
            using (var connection = DbConnectionHelper.GetConnection())
            {
                var command = new SqliteCommand("SELECT ID, WatchID, Quantity, DeliveryDate FROM Stock", connection);
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        stockItems.Add(new Stock
                        {
                            ID = reader.GetInt32(0),
                            WatchID = reader.GetInt32(1),
                            Quantity = reader.GetInt32(2),
                            DeliveryDate = Convert.ToDateTime(reader.GetString(3)) // В SQLite даты хранятся как строки
                        });
                    }
                }
            }
            return stockItems;
        }

        public void Add(Stock stockItem)
        {
            using (var connection = DbConnectionHelper.GetConnection())
            {
                var command = new SqliteCommand("INSERT INTO Stock (WatchID, Quantity, DeliveryDate) VALUES (@watchId, @qty, @date)", connection);
                command.Parameters.AddWithValue("@watchId", stockItem.WatchID);
                command.Parameters.AddWithValue("@qty", stockItem.Quantity);
                command.Parameters.AddWithValue("@date", stockItem.DeliveryDate.ToString("yyyy-MM-dd"));
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void Update(Stock stockItem)
        {
            using (var connection = DbConnectionHelper.GetConnection())
            {
                var command = new SqliteCommand("UPDATE Stock SET WatchID = @watchId, Quantity = @qty, DeliveryDate = @date WHERE ID = @id", connection);
                command.Parameters.AddWithValue("@watchId", stockItem.WatchID);
                command.Parameters.AddWithValue("@qty", stockItem.Quantity);
                command.Parameters.AddWithValue("@date", stockItem.DeliveryDate.ToString("yyyy-MM-dd"));
                command.Parameters.AddWithValue("@id", stockItem.ID);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void Delete(int id)
        {
            using (var connection = DbConnectionHelper.GetConnection())
            {
                var command = new SqliteCommand("DELETE FROM Stock WHERE ID = @id", connection);
                command.Parameters.AddWithValue("@id", id);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }
    }
}