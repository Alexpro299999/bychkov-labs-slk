using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using WatchStore.DataAccess.Models;

namespace WatchStore.DataAccess.Repositories
{
    public class ManufacturerRepository
    {
        public List<Manufacturer> GetAll()
        {
            var manufacturers = new List<Manufacturer>();
            using (var connection = DbConnectionHelper.GetConnection())
            {
                var command = new SqliteCommand("SELECT ID, ManufacturerName, Country FROM Manufacturers", connection);
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        manufacturers.Add(new Manufacturer
                        {
                            ID = reader.GetInt32(0),
                            ManufacturerName = reader.GetString(1),
                            Country = reader.GetString(2)
                        });
                    }
                }
            }
            return manufacturers;
        }

        public void Add(Manufacturer manufacturer)
        {
            using (var connection = DbConnectionHelper.GetConnection())
            {
                var command = new SqliteCommand("INSERT INTO Manufacturers (ManufacturerName, Country) VALUES (@name, @country)", connection);
                command.Parameters.AddWithValue("@name", manufacturer.ManufacturerName);
                command.Parameters.AddWithValue("@country", manufacturer.Country);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void Update(Manufacturer manufacturer)
        {
            using (var connection = DbConnectionHelper.GetConnection())
            {
                var command = new SqliteCommand("UPDATE Manufacturers SET ManufacturerName = @name, Country = @country WHERE ID = @id", connection);
                command.Parameters.AddWithValue("@name", manufacturer.ManufacturerName);
                command.Parameters.AddWithValue("@country", manufacturer.Country);
                command.Parameters.AddWithValue("@id", manufacturer.ID);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void Delete(int id)
        {
            using (var connection = DbConnectionHelper.GetConnection())
            {
                var command = new SqliteCommand("DELETE FROM Manufacturers WHERE ID = @id", connection);
                command.Parameters.AddWithValue("@id", id);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public List<Manufacturer> GetManufacturersByTotalValue(decimal maxValue)
        {
            // Шаг 1: Получаем словарь, где ключ - это ID производителя,
            // а значение - это общая сумма цен его часов.
            var manufacturerTotals = new Dictionary<int, decimal>();
            string pricesSql = "SELECT ManufacturerID, SUM(Price) FROM Watches GROUP BY ManufacturerID";

            using (var connection = DbConnectionHelper.GetConnection())
            {
                var command = new SqliteCommand(pricesSql, connection);
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        // Проверяем на DBNull на всякий случай
                        if (!reader.IsDBNull(0) && !reader.IsDBNull(1))
                        {
                            manufacturerTotals[reader.GetInt32(0)] = reader.GetDecimal(1);
                        }
                    }
                }
            }

            // Шаг 2: Получаем ВСЕХ производителей
            var allManufacturers = GetAll();

            // Шаг 3: Фильтруем список производителей в C#
            var filteredManufacturers = allManufacturers
                .Where(manufacturer =>
                    // Проверяем, есть ли вообще у производителя часы
                    manufacturerTotals.ContainsKey(manufacturer.ID) &&
                    // И если есть, проверяем, что их общая стоимость меньше заданного значения
                    manufacturerTotals[manufacturer.ID] < maxValue)
                .ToList();

            return filteredManufacturers;
        }

        public List<DailyProductionCost> GetGroupedProductionReportData()
        {
            var results = new List<DailyProductionCost>();
            string sql = @"
        SELECT m.ManufacturerName, s.DeliveryDate, SUM(w.Price * s.Quantity) AS DailyTotal
        FROM Manufacturers m
        JOIN Watches w ON m.ID = w.ManufacturerID
        JOIN Stock s ON w.ID = s.WatchID
        GROUP BY m.ManufacturerName, s.DeliveryDate
        ORDER BY m.ManufacturerName, s.DeliveryDate";

            using (var connection = DbConnectionHelper.GetConnection())
            {
                var command = new SqliteCommand(sql, connection);
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        results.Add(new DailyProductionCost
                        {
                            ManufacturerName = reader.GetString(0),
                            DeliveryDate = Convert.ToDateTime(reader.GetString(1)),
                            TotalCost = reader.GetDecimal(2)
                        });
                    }
                }
            }
            return results;
        }

    }
}