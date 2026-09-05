using Entities.Models;

namespace Services.Contracts
{
    // Kategori verileriyle ilgili iş kurallarını tanımladığımız servis arayüzümüz.
    public interface ICategoryService
    {
        Task<IEnumerable<Category>> GetAllCategoryiesAsync(bool trackChanges);
        Task<Category> GetOneCategoryByIdAsync(int id, bool trackChanges);
    }
}
