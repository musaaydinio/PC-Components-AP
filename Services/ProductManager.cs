using Entities.Models;
using Repository.Contracts;
using Services.Contracts;
using System.ComponentModel.DataAnnotations;

namespace Services
{
    public class ProductManager:IProductServices
    {
        private readonly ILoggerServices _logger;
        private readonly IRepositoryManager _manager;

        public ProductManager(IRepositoryManager manager,ILoggerServices logger)
        {
            _logger = logger;
            _manager = manager; 
        }

        public Product CreateOneProduct(Product product)
        {
            if(product is null)
                throw new ArgumentNullException(nameof(product));

            _manager.Product.CreateOneProduct(product);
            _manager.Save();
            return product;
        }

        public void DeleteOneProduct(int id, bool trackChanges)
        {
            var entity = _manager.Product.GetProductById(id, trackChanges);
            if (entity is null)
                throw new Exception($"Product with id:{id}could not found");
            _manager.Product.DeleteOneProduct(entity);
            _manager.Save();
        }

        public IEnumerable<Product> GetAllProduct(bool trackChanges)
        {
            return _manager.Product.GetAllProduct(trackChanges);
        }

        public Product GetOneProductById(int id, bool trackChanges)
        {
            return _manager.Product.GetProductById(id, trackChanges);
        }

        public void UpdateOneProduct(int id, Product product, bool trackChanges)
        {
            var entity=_manager.Product.GetProductById(id, trackChanges);
            if(entity is null)
                throw new Exception($"Product with id:{id}could not found");
            if(product is null)
                throw new ArgumentNullException(nameof(product));
            entity.Name = product.Name;
            entity.Price = product.Price;
            entity.StockQuantity = product.StockQuantity;

            _manager.Product.Update(entity);
            _manager.Save();
        }
    }
}
