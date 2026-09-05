using Entities.Models;
using Story.Contracts;

namespace Repository.Contracts
{
    // Kategori tablomuza özel veritabanı işlemlerini tanımladığımız arayüzümüz.
    public interface ICategoryRepositroy : IRepositoryBase<Category>
    {
        Task<IEnumerable<Category>> GetAllCategoriesAsync(bool trackChanges);
        Task<Category> GETOneCategoryById(int id, bool trackChanges);
        void CreateOneCteagory(Category category);
        void DeleteOneCteagory(Category category);
        void UpdateCteagory(Category category);
    }
}
