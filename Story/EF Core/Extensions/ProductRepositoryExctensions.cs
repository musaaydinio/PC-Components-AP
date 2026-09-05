using Entities.Models;
using Repository.EF_Core.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;

namespace Repository.EF_Core
{
    // Veritabanı sorgularımızı (IQueryable) genişleterek filtreleme, arama ve sıralama işlemlerini arka arkaya zincirlenebilir
    //  hale getirdiğimiz sınıfımız.
    public static class ProductRepositoryExctensions
    {
        // Ürünleri, belirlenen minimum ve maksimum fiyat aralığına göre veritabanı seviyesinde filtreliyoruz.
        public static IQueryable<Product> FilterProduct(this IQueryable<Product> products,
            uint minPrice, uint maxPrice) =>
            products.Where(product => (product.Price >= minPrice) &&
            (product.Price <= maxPrice));

        // İstemciden gelen arama terimine göre ürün isimlerinde arama yapıyoruz.
        public static IQueryable<Product>Search(this IQueryable<Product> products,
            string searchTerm)
        {
            // Arama terimi boş gönderildiyse filtreleme yapmadan mevcut sorguyu döndürüyoruz.
            if (string.IsNullOrWhiteSpace(searchTerm))
                return products;

            var lowerCaseTerm =searchTerm.Trim().ToLower();
            return products.Where(b=>b.Name.ToLower()
            .Contains(searchTerm));
        }

        // İstemciden gelen sıralama metnine göre verileri dinamik olarak sıralıyoruz.
        public static IQueryable<Product>Sort(this IQueryable<Product> products,
            string orderNyQuerString)
        {
            // Sıralama parametresi gönderilmediyse, varsayılan olarak Id'ye göre artan şekilde sıralıyoruz.
            if (string.IsNullOrWhiteSpace(orderNyQuerString))
                return products.OrderBy(b=>b.Id);

           var orderQuery=OrderQueryBuilder.CreateOrderQuery<Product>(orderNyQuerString);

            // Dinamik sorgu oluşturucumuzdan null dönerse yine varsayılan olarak Id'ye göre sıralıyoruz.
            if (orderQuery is null)
                return products.OrderBy(b => b.Id);

            return products.OrderBy(orderQuery);
        }
    }
}
