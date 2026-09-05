using Entities.Models;
using Entities.RequestFeatures;
using Story.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Contracts
{
    // Ürünlerle ilgili veritabanı işlemlerini (CRUD, sayfalama, filtreleme) tanımladığımız arayüzümüz.
    public interface IProductRepository : IRepositoryBase<Product>
    {
        // Parametrelere göre filtrelenmiş ve sayfalanmış ürün listesini getiriyoruz.
        Task<PageList<Product>> GetAllProductAsync(ProductParameters productParameters,
            bool trackChanges);
        Task<Product> GetOneProductByIdAsync(int id , bool trackChanges);
        void CreateOneProduct(Product product);
        void UpdateOneProduct(Product product);
        void DeleteOneProduct(Product product);
        Task<List<Product>> GetAllProductAsync(bool trackChanges);
        Task<IEnumerable<Product>> GetAllProductWithDetails(bool trackChanges);
    }
}
