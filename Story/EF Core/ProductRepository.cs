using Entities.Models;
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


        public IQueryable<Product> GetAllProduct(bool trackChanges) => FindAll(trackChanges);

        public Product GetOneProductById(int id, bool trackChanges) => FindByCondition(b => b.Id.Equals(id), trackChanges).SingleOrDefault();
        

        public void UpdateOneProduct(Product product)=>Update(product); 
       
    }
}
