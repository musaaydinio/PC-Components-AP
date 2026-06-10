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

        public ProductDto CreateOneProduct(ProductDtoForInsertion productDto)
        {      
            var entity= _mapper.Map<Product>(productDto);
            _manager.Product.CreateOneProduct(entity);
            _manager.Save();
            return _mapper.Map<ProductDto>(entity);
        }

        public void DeleteOneProduct(int id, bool trackChanges)
        {
            var entity = _manager.Product.GetOneProductById(id, trackChanges);
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

        public ProductDto GetOneProductById(int id, bool trackChanges)
        {
            var product= _manager.Product.GetOneProductById(id, trackChanges);
            if(product is null)
                throw new ProductNotFoundException(id)
;
            return _mapper.Map<ProductDto>(product);
        }

        public (ProductDtoForUpdate productDtoForUpdate, Product product) GetOneProductForPatch(int id, bool trackChanges)
        {
            var product= _manager.Product.GetOneProductById(id, trackChanges);
            if (product is null)
                throw new ProductNotFoundException(id);

            var productDtoForUpdate=_mapper.Map<ProductDtoForUpdate>(product);

            return (productDtoForUpdate, product);  
        }

        public void SaveChangesForPatch(ProductDtoForUpdate productDtoForUpdate, Product product)
        {
            _mapper.Map(productDtoForUpdate, product);
            _manager.Save();
        }

        public void UpdateOneProduct(int id, ProductDtoForUpdate productDto, bool trackChanges)
        {
            var entity=_manager.Product.GetOneProductById(id, trackChanges);
            if(entity is null)
                throw new ProductNotFoundException(id);

            entity=_mapper.Map<Product>(productDto);
            
            _manager.Product.Update(entity);
            _manager.Save();
        }
    }
}
