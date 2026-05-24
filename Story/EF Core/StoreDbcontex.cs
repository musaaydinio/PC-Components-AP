using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Story.EF_Core
{
    public class StoreDbcontex : DbContext
    {
        public StoreDbcontex(DbContextOptions options):
            base (options)
        {
            
        }
        public DbSet<Product> Products { get; set; }

    }
}
