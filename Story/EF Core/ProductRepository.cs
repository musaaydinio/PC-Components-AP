using Entities.Models;
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
    public class ProductRepository : RepositoryBase<Product> , IProductRepository
    {
        public ProductRepository(StoreDbcontex contex) : base(contex)
        {
            
        }

        public void CreateOneProduct(Product product)=>Create(product);
       

        public void DeleteOneProduct(Product product)=>Delete(product);


        public async Task<IEnumerable<Product>> GetAllProductAsync(bool trackChanges) => 
            await FindAll(trackChanges).OrderBy(b =>b.Id).ToListAsync();

        public async Task<Product>GetOneProductByIdAsync(int id, bool trackChanges) => 
            await FindByCondition(b => b.Id.Equals(id), trackChanges).SingleOrDefaultAsync();
        

        public void UpdateOneProduct(Product product)=>Update(product); 
       
    }
}
