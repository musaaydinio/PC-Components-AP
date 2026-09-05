using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Repository.Contracts;
using Repository.EF_Core;

namespace Story.EF_Core
{
    // E-ticaret sistemimizdeki kategori veritabanı operasyonlarını yürüttüğümüz sınıfımız.
    public class CateagoryRespository : RepositoryBase<Category>, ICategoryRepositroy
    {
        public CateagoryRespository(StoreDbcontex contex) : base(contex)
        {

        }

        public void CreateOneCteagory(Category category) => Create(category);

        public void DeleteOneCteagory(Category category) => Delete(category);

        // Tüm kategorileri isimlerine göre alfabetik olarak sıralayıp listeliyoruz.
        public async Task<IEnumerable<Category>> GetAllCategoriesAsync(bool trackChanges)
        {
            return await FindAll(trackChanges)
                .OrderBy(n => n.CategoryName)
                .ToListAsync();
        }

        // Dışarıdan gelen ID değerine sahip tek bir kategori kaydını bulup getiriyoruz.
        public async Task<Category> GETOneCategoryById(int id, bool trackChanges)
        {
            return await FindByCondition(c => c.CategoryId.Equals(id), trackChanges)
        .Include(c => c.Products) // EF Core Eager Loading: Ürünleri sorguya dahil eder
        .SingleOrDefaultAsync();
        }

        public void UpdateCteagory(Category category) => Update(category);
    }
}
