using Entities.Exceptions;
using Entities.Models;
using Repository.Contracts;
using Services.Contracts;

namespace Services
{
    // Kategori işlemleriyle ilgili business logic işlettiğimiz servis sınıfımız.
    public class CategoryManager : ICategoryService
    {
        private readonly IRepositoryManager _repositoryManager;

        public CategoryManager(IRepositoryManager repositoryManager)
        {
            _repositoryManager = repositoryManager;
        }

        // Repository üzerinden tüm kategorileri çekip Controller katmanına iletiyoruz.
        public async Task<IEnumerable<Category>> GetAllCategoryiesAsync(bool trackChanges)
        {
            return await _repositoryManager.Category
                .GetAllCategoriesAsync(trackChanges);
        }

        // İstenen ID'ye sahip kategoriyi veritabanından sorguluyor;
        // eğer bulamazsak sisteme hata fırlatıcıyı (Exception) devreye sokuyoruz.
        public async Task<Category> GetOneCategoryByIdAsync(int id, bool trackChanges)
        {

            var category = await _repositoryManager
                .Category
                .GETOneCategoryById(id, trackChanges);

            if (category is null)
                throw new CategoryNotFoundException(id);
            return category;
        }
    }
}

