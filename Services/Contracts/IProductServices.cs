using Entities.DataTransferObject;
using Entities.Models;
using Entities.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Contracts
{
    // Ürünlerle ilgili tüm iş kurallarını (business logic) ve Data Transfer Object dönüşümlerini yönettiğimiz servis arayüzümüz.
    public interface IProductServices
    {
        // Ürünleri sayfalama, filtreleme parametreleriyle ve veri şekillendirme (Data Shaping) uygulanmış olarak getiriyoruz.
        Task<(IEnumerable<ExpandoObject> product, MetaData metaData)> GetAllProductAsync(ProductParameters productParameters,
            bool trackChanges);

        // Belirtilen ID'ye sahip ürünü bulup, istemciye sunulmak üzere DTO olarak döndürüyoruz.
        Task<ProductDto> GetOneProductByIdAsync(int id, bool trackChanges);

        // Yeni bir ürünü sisteme ekliyoruz ve sonucunu DTO olarak dönüyoruz.
        Task<ProductDto> CreateOneProductAsync(ProductDtoForInsertion product);

        // Var olan bir ürünü dışarıdan gelen DTO verileriyle güncelliyoruz .
        Task UpdateOneProductAsync(int id, ProductDtoForUpdate productDto, bool trackChanges);

        // Belirtilen ID'ye sahip ürünü sistemden siliyoruz.
        Task DeleteOneProductAsync(int id, bool trackChanges);

        // PATCH işlemi için ürünü ve ona ait DTO kopyasını hazırlayıp getiriyoruz.
        Task<(ProductDtoForUpdate productDtoForUpdate, Product product)> GetOneProductForPatchAsync(int id, bool trackChanges);

        // Kısmi güncelleme işlemi tamamlandıktan sonra değişen verileri kaydediyoruz.
        Task SaveChangesForPatchAsync(ProductDtoForUpdate productDtoForUpdate, Product product);

        // Sayfalama kullanmadan tüm ürünleri liste halinde getiriyoruz.
        Task<List<Product>> GetAllProductAsync(bool trackChanges);

        // Ürünleri, ilişkili olduğu diğer detaylarla birlikte getiriyoruz.
        Task<IEnumerable<Product>> GetAllProductWithDetails(bool trackChanges);
    }
}
