using Entities.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Story.EF_Core
{
    // Veritabanı bağlantımızı ve Identity altyapımızı sağlayan merkez Entity Framework Core sınıfımız.
    public class StoreDbcontex : IdentityDbContext<User>
    {
        public StoreDbcontex(DbContextOptions options):
            base (options)
        {
            
        }
        // Ürünler tablomuza karşılık gelen alanımız.
        public DbSet<Product> Products { get; set; }

        // Kategoriler tablomuza karşılık gelen alanımız.
        public DbSet<Category> Categories { get; set; }

        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Order> Orders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Config klasöründeki tüm Entity yapılandırma dosyalarımızı otomatik olarak bulup projeye dahil ediyoruz.
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            // Geliştirme aşamasında beklemedeki model değişiklikleri (PendingModelChanges) uyarısını gizliyoruz.
            optionsBuilder.ConfigureWarnings(w =>
                w.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.PendingModelChangesWarning));
        }
    }
}
