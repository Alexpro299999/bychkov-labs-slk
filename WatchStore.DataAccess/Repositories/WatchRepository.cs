using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using WatchStore.DataAccess.Models;

namespace WatchStore.DataAccess.Repositories
{
    public class WatchRepository
    {
        private Watch MapReaderToWatch(SqliteDataReader reader)
        {
            return new Watch
            {
                ID = reader.GetInt32(reader.GetOrdinal("ID")),
                WatchModel = reader.GetString(reader.GetOrdinal("WatchModel")),
                WatchType = reader.GetString(reader.GetOrdinal("WatchType")),
                Price = reader.GetDecimal(reader.GetOrdinal("Price")),
                ManufacturerID = reader.GetInt32(reader.GetOrdinal("ManufacturerID"))
            };
        }

        public List<Watch> GetAll()
        {
            var watches = new List<Watch>();
            using (var connection = DbConnectionHelper.GetConnection())
            {
                var command = new SqliteCommand("SELECT * FROM Watches", connection);
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        watches.Add(MapReaderToWatch(reader));
                    }
                }
            }
            return watches;
        }

        public void Add(Watch watch)
        {
            using (var connection = DbConnectionHelper.GetConnection())
            {
                var command = new SqliteCommand("INSERT INTO Watches (WatchModel, WatchType, Price, ManufacturerID) VALUES (@model, @type, @price, @manId)", connection);
                command.Parameters.AddWithValue("@model", watch.WatchModel);
                command.Parameters.AddWithValue("@type", watch.WatchType);
                command.Parameters.AddWithValue("@price", watch.Price);
                command.Parameters.AddWithValue("@manId", watch.ManufacturerID);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void Update(Watch watch)
        {
            using (var connection = DbConnectionHelper.GetConnection())
            {
                var command = new SqliteCommand("UPDATE Watches SET WatchModel = @model, WatchType = @type, Price = @price, ManufacturerID = @manId WHERE ID = @id", connection);
                command.Parameters.AddWithValue("@model", watch.WatchModel);
                command.Parameters.AddWithValue("@type", watch.WatchType);
                command.Parameters.AddWithValue("@price", watch.Price);
                command.Parameters.AddWithValue("@manId", watch.ManufacturerID);
                command.Parameters.AddWithValue("@id", watch.ID);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void Delete(int id)
        {
            using (var connection = DbConnectionHelper.GetConnection())
            {
                var command = new SqliteCommand("DELETE FROM Watches WHERE ID = @id", connection);
                command.Parameters.AddWithValue("@id", id);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public int CountWatchesByManufacturer(int manufacturerId)
        {
            using (var connection = DbConnectionHelper.GetConnection())
            {
                var command = new SqliteCommand("SELECT COUNT(*) FROM Watches WHERE ManufacturerID = @manId", connection);
                command.Parameters.AddWithValue("@manId", manufacturerId);
                connection.Open();

                return Convert.ToInt32(command.ExecuteScalar());
            }
        }

        public List<string> GetWatchModelsByType(string watchType)
        {
            var models = new List<string>();
            using (var connection = DbConnectionHelper.GetConnection())
            {
                var command = new SqliteCommand("SELECT WatchModel FROM Watches WHERE WatchType = @type", connection);
                command.Parameters.AddWithValue("@type", watchType);
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read()) { models.Add(reader.GetString(0)); }
                }
            }
            return models;
        }

        public List<Watch> GetMechanicalWatchesCheaperThan(decimal price)
        {
            var watches = new List<Watch>();
            using (var connection = DbConnectionHelper.GetConnection())
            {
                var command = new SqliteCommand("SELECT * FROM Watches WHERE WatchType = 'механические' AND Price < @price", connection);
                command.Parameters.AddWithValue("@price", price);
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read()) { watches.Add(MapReaderToWatch(reader)); }
                }
            }
            return watches;
        }

        public List<string> GetWatchModelsByCountry(string country)
        {
            var models = new List<string>();
            using (var connection = DbConnectionHelper.GetConnection())
            {
                var command = new SqliteCommand(
                    "SELECT w.WatchModel FROM Watches w INNER JOIN Manufacturers m ON w.ManufacturerID = m.ID WHERE m.Country = @country",
                    connection);
                command.Parameters.AddWithValue("@country", country);
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read()) { models.Add(reader.GetString(0)); }
                }
            }
            return models;
        }

        public List<object> GetWatchesWithTotalValue()
        {
            var results = new List<object>();
            using (var connection = DbConnectionHelper.GetConnection())
            {
                var command = new SqliteCommand(
                    "SELECT w.WatchModel, (w.Price * s.Quantity) AS TotalValue FROM Watches w INNER JOIN Stock s ON w.ID = s.WatchID",
                    connection);
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        results.Add(new { Model = reader.GetString(0), TotalValue = reader.GetDecimal(1).ToString("C") });
                    }
                }
            }
            return results;
        }

    }
}