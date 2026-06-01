using AutoMapper;
using Entities.DataTransferObject;
using Entities.Exceptions;
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
        private readonly IMapper _mapper;
        public ProductManager(IRepositoryManager manager,ILoggerServices logger,IMapper mapper)
        {
            _logger = logger;
            _manager = manager;
            _mapper = mapper;
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
                throw new ProductNotFoundException(id);
            _manager.Product.DeleteOneProduct(entity);
            _manager.Save();
        }

        public IEnumerable<ProductDto> GetAllProduct(bool trackChanges)
        {
            var products= _manager.Product.GetAllProduct(trackChanges);
            return _mapper.Map<IEnumerable<ProductDto>>(products);
        }

        public Product GetOneProductById(int id, bool trackChanges)
        {
            var product= _manager.Product.GetProductById(id, trackChanges);
            if(product is null)
                throw new ProductNotFoundException(id);
            return product;
        }

        public void UpdateOneProduct(int id, ProductDtoForUpdate productDto, bool trackChanges)
        {
            var entity=_manager.Product.GetProductById(id, trackChanges);
            if(entity is null)
                throw new ProductNotFoundException(id);

            entity=_mapper.Map<Product>(productDto);
            
            _manager.Product.Update(entity);
            _manager.Save();
        }
    }
}
