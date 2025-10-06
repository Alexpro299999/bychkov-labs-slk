using Microsoft.EntityFrameworkCore;
using WatchStore.DataAccess.Models;

namespace WatchStore.DataAccess
{
    public class WatchDbContext : DbContext
    {
        public DbSet<Manufacturer> Manufacturers { get; set; }
        public DbSet<Watch> Watches { get; set; }
        public DbSet<Stock> Stock { get; set; }
        public DbSet<Feature> Features { get; set; }

        private static readonly string ConnectionString = "Data Source=WatchStore_EF.db";

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(ConnectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Manufacturer>().HasData(
                new Manufacturer { ID = 1, ManufacturerName = "Casio", Country = "Япония" },
                new Manufacturer { ID = 2, ManufacturerName = "Tissot", Country = "Швейцария" },
                new Manufacturer { ID = 3, ManufacturerName = "Rolex", Country = "Швейцария" }
            );

            modelBuilder.Entity<Watch>().HasData(
                new Watch { ID = 1, WatchModel = "G-Shock GA-2100", WatchType = "кварцевые", Price = 12000, ManufacturerID = 1 },
                new Watch { ID = 2, WatchModel = "T-Classic", WatchType = "механические", Price = 45000, ManufacturerID = 2 },
                new Watch { ID = 3, WatchModel = "Submariner", WatchType = "механические", Price = 850000, ManufacturerID = 3 }
            );

            modelBuilder.Entity<Stock>().HasData(
            new Stock { ID = 1, WatchID = 1, Quantity = 50, DeliveryDate = new System.DateTime(2023, 10, 27) },
            new Stock { ID = 2, WatchID = 2, Quantity = 20, DeliveryDate = new System.DateTime(2023, 10, 25) },
            new Stock { ID = 3, WatchID = 3, Quantity = 5, DeliveryDate = new System.DateTime(2023, 09, 01) }
            );
        }
    }
}