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

        public static IQueryable<Product>Sort(this IQueryable<Product> products,
            string orderNyQuerString)
        {
            if(string.IsNullOrWhiteSpace(orderNyQuerString))
                return products.OrderBy(b=>b.Id);

           var orderQuery=OrderQueryBuilder.CreateOrderQuery<Product>(orderNyQuerString);

            if (orderQuery is null)
                return products.OrderBy(b => b.Id);
            return products.OrderBy(orderQuery);
        }
    }
}
