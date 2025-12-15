using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using WatchShop.DataAccess.Models;

namespace WatchShop.DataAccess.Repositories
{
    public class WatchRepository
    {
        public List<Watch> GetAll()
        {
            using (var context = new WatchDbContext()) { return context.Watches.ToList(); }
        }

        public void Add(Watch watch)
        {
            using (var context = new WatchDbContext()) { context.Watches.Add(watch); context.SaveChanges(); }
        }

        public void Update(Watch watch)
        {
            using (var context = new WatchDbContext()) { context.Watches.Update(watch); context.SaveChanges(); }
        }

        public void Delete(int id)
        {
            using (var context = new WatchDbContext())
            {
                var watch = context.Watches.Find(id);
                if (watch != null) { context.Watches.Remove(watch); context.SaveChanges(); }
            }
        }

        public List<Watch> GetWatchesByType(string watchType)
        {
            using (var context = new WatchDbContext())
            {
                return context.Watches.Where(w => w.WatchType.ToLower() == watchType.ToLower()).ToList();
            }
        }

        public List<Watch> GetMechanicalWatchesCheaperThan(decimal price)
        {
            using (var context = new WatchDbContext())
            {
                return context.Watches.Where(w => w.WatchType == "механические" && w.Price < price).ToList();
            }
        }

        public List<string> GetWatchModelsByCountry(string country)
        {
            using (var context = new WatchDbContext())
            { 
                var allWatchesWithManufacturers = context.Watches
                    .Include(w => w.Manufacturer)
                    .ToList();
                var filteredModels = allWatchesWithManufacturers
                    .Where(w => w.Manufacturer.Country.Equals(country, StringComparison.OrdinalIgnoreCase))
                    .Select(w => w.WatchModel)
                    .ToList();

                return filteredModels;
            }
        }

        public List<Manufacturer> GetManufacturersByTotalValue(decimal maxValue)
        {
            using (var context = new WatchDbContext())
            {
                return context.Manufacturers
                    .Include(m => m.Watches)
                    .Where(m => m.Watches.Sum(w => w.Price) < maxValue)
                    .ToList();
            }
        }

        public List<object> GetWatchesWithTotalValue()
        {
            using (var context = new WatchDbContext())
            {
                return context.Stock
                    .Include(s => s.Watch)
                    .Select(s => new {
                        Model = s.Watch.WatchModel,
                        TotalValue = (s.Watch.Price * s.Quantity).ToString("C")
                    })
                    .ToList<object>();
            }
        }
    }
}