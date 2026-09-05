using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Repository.EF_Core.Extensions
{
    // İstemciden gelen sıralama parametrelerini alıp, dinamik sorgu metnine dönüştürdüğümüz yardımcı sınıfımız.
    public static class OrderQueryBuilder
    {
        public static String CreateOrderQuery<T>(String orderByQueryString)
        {
            // Gelen parametreleri virgülle ayırarak birden fazla alana göre sıralama desteği sağlıyoruz.
            var orderParams = orderByQueryString.Trim().Split(',');

            // Reflection kullanarak T modelinin sahip olduğu tüm public özellikleri çekiyoruz.
            var propertyInsfos = typeof(T)
                .GetProperties(BindingFlags.Public | BindingFlags.Instance);

            var orderQueryBuilder = new StringBuilder();

            foreach (var param in orderParams)
            {
                if (string.IsNullOrWhiteSpace(param))
                    continue;

                var propertyFromQueryName = param.Split(' ')[0];

                // İstemcinin gönderdiği alan adının modelimizde gerçekten var olup olmadığını kontrol ediyoruz.
                var objectProperty = propertyInsfos.FirstOrDefault(pi => pi.Name
                .Equals(propertyFromQueryName, StringComparison.InvariantCultureIgnoreCase));

                if (objectProperty is null)
                    continue;

                // Parametrenin sonuna bakarak artan (ascending) veya azalan (descending) sıralama yönünü belirliyoruz.
                var direction = param.EndsWith("desc") ? "descending" : "ascending";
                orderQueryBuilder.Append($"{objectProperty.Name.ToString()} {direction},");
            }
           
            var orderQuery = orderQueryBuilder.ToString().TrimEnd(',', ' ');

            return orderQuery;
        }
    }
}
