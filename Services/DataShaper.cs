using Services.Contracts;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class DataShaper<T> : IDataShaper<T>
    {
        public PropertyInfo[] Properties { get; set; }

        public DataShaper()
        {
            Properties = typeof(T).GetProperties(BindingFlags.Public
                | BindingFlags.Instance);
        }

        public IEnumerable<ExpandoObject> ShapeData(IEnumerable<T> entities, string fieldsString)
        {
            var requiredFileds = GetRequiredProperties(fieldsString);
            return FetchData(entities, requiredFileds);
        }

        public ExpandoObject ShapeData(T entity, string fieldsString)
        {
            var requiredProperties = GetRequiredProperties(fieldsString);
            return FetchDataFrEntity(entity, requiredProperties);
        }

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

