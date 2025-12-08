using System.Collections.Generic;
using System.Linq;
using WatchShop.DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace WatchShop.DataAccess.Repositories
{
    public class ManufacturerRepository
    {
        public List<Manufacturer> GetAll()
        {
            using (var context = new WatchDbContext()) { return context.Manufacturers.ToList(); }
        }

        public void Add(Manufacturer manufacturer)
        {
            using (var context = new WatchDbContext()) { context.Manufacturers.Add(manufacturer); context.SaveChanges(); }
        }

        public void Update(Manufacturer manufacturer)
        {
            using (var context = new WatchDbContext()) { context.Manufacturers.Update(manufacturer); context.SaveChanges(); }
        }

        public void Delete(int id)
        {
            using (var context = new WatchDbContext())
            {
                var manufacturer = context.Manufacturers.Find(id);
                if (manufacturer != null) { context.Manufacturers.Remove(manufacturer); context.SaveChanges(); }
            }
        }

        public int CountWatchesByManufacturer(int manufacturerId)
        {
            using (var context = new WatchDbContext())
            {
                return context.Watches.Count(w => w.ManufacturerID == manufacturerId);
            }
        }

        public List<DailyProductionCost> GetGroupedProductionReportData()
        {
            using (var context = new WatchDbContext())
            {
                return context.Stock
                    .Include(s => s.Watch)
                    .ThenInclude(w => w.Manufacturer)
                    .GroupBy(s => new { s.Watch.Manufacturer.ManufacturerName, s.DeliveryDate })
                    .Select(g => new DailyProductionCost
                    {
                        ManufacturerName = g.Key.ManufacturerName,
                        DeliveryDate = g.Key.DeliveryDate,
                        TotalCost = g.Sum(s => s.Quantity * s.Watch.Price)
                    })
                    .OrderBy(r => r.ManufacturerName).ThenBy(r => r.DeliveryDate)
                    .ToList();
            }
        }
    }
}