using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using WatchStore.DataAccess.Models;

namespace WatchStore.DataAccess.Repositories
{
    public class StockRepository
    {
        public List<Stock> GetAll()
        {
            using (var context = new WatchDbContext())
            {
                return context.Stock.Include(s => s.Watch).ToList(); 
            }
        }

        public Stock GetById(int id)
        {
            using (var context = new WatchDbContext())
            {
                return context.Stock.Include(s => s.Watch).FirstOrDefault(s => s.ID == id);
            }
        }

        public void Add(Stock stockItem)
        {
            using (var context = new WatchDbContext())
            {
                context.Stock.Add(stockItem);
                context.SaveChanges();
            }
        }

        public void Update(Stock stockItem)
        {
            using (var context = new WatchDbContext())
            {
                context.Stock.Update(stockItem);
                context.SaveChanges();
            }
        }

        public void Delete(int id)
        {
            using (var context = new WatchDbContext())
            {
                var stockItem = context.Stock.Find(id);
                if (stockItem != null)
                {
                    context.Stock.Remove(stockItem);
                    context.SaveChanges();
                }
            }
        }
    }
}