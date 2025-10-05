using Microsoft.Data.Sqlite;
using System.IO;

namespace WatchStore.DataAccess
{
    public static class DatabaseInitializer
    {
        public static void InitializeDatabase()
        {
            string dbFile = "WatchStore.db";
            if (File.Exists(dbFile))
            {
                return; 
            }

            using (var connection = DbConnectionHelper.GetConnection())
            {
                connection.Open();

                string script = @"
                    -- Создание таблиц
                    CREATE TABLE Manufacturers (
                       ID INTEGER PRIMARY KEY AUTOINCREMENT,
                       ManufacturerName TEXT NOT NULL,
                       Country TEXT NOT NULL
                    );

                    CREATE TABLE Watches (
                       ID INTEGER PRIMARY KEY AUTOINCREMENT,
                       WatchModel TEXT NOT NULL,
                       WatchType TEXT NOT NULL,
                       Price REAL NOT NULL,
                       ManufacturerID INTEGER NOT NULL,
                       FOREIGN KEY (ManufacturerID) REFERENCES Manufacturers(ID)
                    );

                    CREATE TABLE Stock (
                       ID INTEGER PRIMARY KEY AUTOINCREMENT,
                       WatchID INTEGER NOT NULL,
                       Quantity INTEGER NOT NULL,
                       DeliveryDate TEXT,
                       FOREIGN KEY (WatchID) REFERENCES Watches(ID)
                    );

                    -- Наполнение данными
                    INSERT INTO Manufacturers (ManufacturerName, Country) VALUES ('Casio', 'Япония');
                    INSERT INTO Manufacturers (ManufacturerName, Country) VALUES ('Tissot', 'Швейцария');
                    INSERT INTO Manufacturers (ManufacturerName, Country) VALUES ('Rolex', 'Швейцария');

                    INSERT INTO Watches (WatchModel, WatchType, Price, ManufacturerID) VALUES ('G-Shock GA-2100', 'кварцевые', 12000, 1);
                    INSERT INTO Watches (WatchModel, WatchType, Price, ManufacturerID) VALUES ('T-Classic', 'механические', 45000, 2);
                    INSERT INTO Watches (WatchModel, WatchType, Price, ManufacturerID) VALUES ('Submariner', 'механические', 850000, 3);

                    INSERT INTO Stock (WatchID, Quantity, DeliveryDate) VALUES (1, 50, '2023-10-27');
                    INSERT INTO Stock (WatchID, Quantity, DeliveryDate) VALUES (2, 20, '2023-10-25');
                ";

                var command = new SqliteCommand(script, connection);
                command.ExecuteNonQuery();
            }
        }
    }
}