using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repository.EF_Core.Config
{
    // E-ticaret sistemimizdeki kategori tablosunun veritabanı ayarlarını ve başlangıç verilerini yapılandırıyoruz.
    public class CategoryConfig : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            // CategoryId alanımızı Primary Key (Birincil Anahtar) olarak belirliyoruz.
            builder.HasKey(n => n.CategoryId);
            // Kategori adının veritabanında boş geçilemez (zorunlu) olmasını sağlıyoruz.
            builder.Property(n => n.CategoryName).IsRequired();
            // Veritabanı ilk oluştuğunda tablomuza eklenecek varsayılan e-ticaret kategorilerini tanımlıyoruz.
            builder.HasData(
               new Category()
               {
                   CategoryId = 1,
                   CategoryName = "Ekran Kartı"
               },
               new Category()
               {
                   CategoryId = 2,
                   CategoryName = "Monitor"
               },
               new Category()
               {
                   CategoryId = 3,
                   CategoryName = "İşlemci"
               },
               new Category()
               {
                   CategoryId = 4,
                   CategoryName = "Klavye,Mouse,Kulaklık"
               },
                new Category()
                {
                    CategoryId = 5,
                    CategoryName = "Ram"
                },
                 new Category()
                 {
                     CategoryId = 6,
                     CategoryName = "Anakart"
                 }
            );
        }
    }
}
