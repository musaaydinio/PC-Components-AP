using Repository.Contracts;
using Story.EF_Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.EF_Core
{
     
    public class RepositoryManager:IRepositoryManager
    {
        private readonly StoreDbcontex _contex;
        private readonly Lazy<IProductRepository> _productRepository;
        public RepositoryManager(StoreDbcontex contex)
        {
            _contex = contex;
            _productRepository = new Lazy<IProductRepository>(() => new ProductRepository(_contex));
        }
        public IProductRepository Product=> _productRepository.Value;

        public void Save()
        {
           _contex.SaveChanges();
        }
    }
}
