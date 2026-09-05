using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Services.Contracts;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Services
{
    // İstemcinin sadece talep ettiği alanları seçebilmesi için Data Shaping işlemlerini bu sınıfta gerçekleştiriyoruz.
    public class DataShaper<T> : IDataShaper<T>
    {
        public PropertyInfo[] Properties { get; set; }

        public DataShaper()
        {
            // İlgili nesneye ait tüm public özellikleri Reflection kullanarak bellekten alıp dizimize atıyoruz.
            Properties = typeof(T).GetProperties(BindingFlags.Public
                | BindingFlags.Instance);
        }

        // İstemciden gelen alan isteklerine göre veri koleksiyonumuzun sadece ilgili özelliklerini dinamik bir yapıya çevirip dönüyoruz.
        public IEnumerable<ExpandoObject> ShapeData(IEnumerable<T> entities, string fieldsString)
        {
            var requiredFileds = GetRequiredProperties(fieldsString);
            return FetchData(entities, requiredFileds);
        }

        // İstemciden gelen alan isteklerine göre tek bir nesnenin sadece ilgili özelliklerini şekillendirip dönüyoruz.
        public ExpandoObject ShapeData(T entity, string fieldsString)
        {
            var requiredProperties = GetRequiredProperties(fieldsString);
            return FetchDataFrEntity(entity, requiredProperties);
        }

        // İstemcinin gönderdiği metin tabanlı alan listesini virgüllerden ayırıp, sınıfımızın gerçek özellikleriyle eşleştiriyoruz.
        private IEnumerable<PropertyInfo> GetRequiredProperties(string fieldsString)
        {
            var requiredfields = new List<PropertyInfo>();
            if (!string.IsNullOrWhiteSpace(fieldsString))
            {
                var fields = fieldsString.Split(',', StringSplitOptions.RemoveEmptyEntries);
                foreach (var field in fields)
                {
                    var property = Properties.
                        FirstOrDefault(pi => pi.Name.Equals(field.Trim(),
                        StringComparison.InvariantCultureIgnoreCase));
                    if (property is null)
                        continue;
                    requiredfields.Add(property);
                }
            }
            else
            {
                requiredfields = Properties.ToList();
            }
            return requiredfields;
        }

        // Belirlenen property değerlerini reflection ile okuyup dinamik nesneye ekliyoruz.

        private ExpandoObject FetchDataFrEntity(T entity,
            IEnumerable<PropertyInfo> requiredPropies)
        {
            var shapedObject = new ExpandoObject();
            foreach (var property in requiredPropies)
            {
                var objectPropertyValue = property.GetValue(entity);
                shapedObject.TryAdd(property.Name, objectPropertyValue);
            }           
            return shapedObject;
        }

        // Tüm liste üzerinde dönerek her bir nesne için şekillendirme işlemini uyguluyoruz.
        private IEnumerable<ExpandoObject> FetchData(IEnumerable<T> entities,
            IEnumerable<PropertyInfo> requiredPropies)
        {
            var shapedData = new List<ExpandoObject>();
            foreach (var entity in entities)
            {
                var shapedObject = FetchDataFrEntity(entity, requiredPropies);
                shapedData.Add(shapedObject);
            }
            return shapedData;
        }
    }
}

