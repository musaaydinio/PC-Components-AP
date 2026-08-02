using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.EF_Core
{
    public static class ProductRepositoryExctensions
    {
        public static IQueryable<Product> FilterProduct(this IQueryable<Product> products,
            uint minPrice, uint maxPrice) =>
            products.Where(product => (product.Price >= minPrice) &&
            (product.Price <= maxPrice));

        public static IQueryable<Product>Search(this IQueryable<Product> products,
            string searchTerm)
        {
            if(string.IsNullOrWhiteSpace(searchTerm))
                return products;

            var lowerCaseTerm =searchTerm.Trim().ToLower();
            return products.Where(b=>b.Name.ToLower()
            .Contains(searchTerm));
        }
    }
}
