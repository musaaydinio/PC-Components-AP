using Entities.Models;
using Entities.RequestFeatures;
using Microsoft.EntityFrameworkCore;
using Repository.Contracts;
using Story.EF_Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.EF_Core
{
    // Ürünlerle ilgili veritabanı işlemlerini, filtreleme ve sayfalama mantıklarıyla beraber yürüttüğümüz sınıfımız.
    public class ProductRepository : RepositoryBase<Product> , IProductRepository
    {
        public ProductRepository(StoreDbcontex contex) : base(contex)
        {
            
        }

        public void CreateOneProduct(Product product)=>Create(product);
       

        public void DeleteOneProduct(Product product)=>Delete(product);

        // İstemciden gelen parametrelere göre ürünleri filtreleyip sayfalanmış olarak döndürüyoruz.
        public async Task<PageList<Product>> GetAllProductAsync(ProductParameters productParameters,
            bool trackChanges)
        {
           var product= await FindAll(trackChanges)
            .FilterProduct(productParameters.MinPrice, productParameters.MaxPrice)
            .Search(productParameters.SearchTerm)
            .OrderBy(b => b.Id)
            .ToListAsync();
            return PageList<Product>.ToPagedList(product, productParameters.PageNumber,
                productParameters.PageSize);
        }

        // Sayfalama parametresi olmadan tüm ürünleri düz bir şekilde listeliyoruz.
        public async Task<List<Product>> GetAllProductAsync(bool trackChanges)
        {
           return await FindAll(trackChanges)
        .Include(p => p.Category)
        .ToListAsync();
        }

        // Ürünleri, ilişkili oldukları detaylarla birlikte çekiyoruz.
        public async Task<IEnumerable<Product>> GetAllProductWithDetails(bool trackChanges)
        {
            return await _contex.Products
        .Include(b => b.Category)
        .OrderBy(b => b.Id)
        .ToListAsync();
        }
        

        // İstediğimiz ID'ye sahip spesifik ürünü veritabanından çekiyoruz.
        public async Task<Product>GetOneProductByIdAsync(int id, bool trackChanges) => 
            await FindByCondition(b => b.Id.Equals(id), trackChanges).SingleOrDefaultAsync();
        

        public void UpdateOneProduct(Product product)=>Update(product); 
       
    }
}
