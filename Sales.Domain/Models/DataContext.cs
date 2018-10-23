using Sales.Common.Models;
using System.Data.Entity;

namespace Sales.Domain.Models
{
    public class DataContext: DbContext
    {
        public DataContext()
            :base("DefaultConnection")
        {
            
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
    }
}
