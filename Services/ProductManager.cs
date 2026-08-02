using AutoMapper;
using Entities.DataTransferObject;
using Entities.Exceptions;
using Entities.Models;
using Entities.RequestFeatures;
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

        public async Task<ProductDto> CreateOneProductAsync(ProductDtoForInsertion productDto)
        {      
            var entity= _mapper.Map<Product>(productDto);
            _manager.Product.CreateOneProduct(entity);
            await _manager.SaveAsync();
            return _mapper.Map<ProductDto>(entity);
        }

        public async Task DeleteOneProductAsync(int id, bool trackChanges)
        {
           var entity= await GetOneProductAndCheckExits(id,trackChanges);
            _manager.Product.DeleteOneProduct(entity);
            await _manager.SaveAsync();
        }

        public async Task<(IEnumerable<ProductDto> product, MetaData metaData)>
            GetAllProductAsync(ProductParameters productParameters,
            bool trackChanges)
        {
            if(!productParameters.ValidPriceRange)
                throw new PriceOutOfRangeBadRequestException();

            var productsWithMetaData= await _manager.Product.GetAllProductAsync(productParameters
                ,trackChanges);
            var productDto= _mapper.Map<IEnumerable<ProductDto>>(productsWithMetaData);
            return(productDto,productsWithMetaData.MetaData);
        }

        public async Task<ProductDto> GetOneProductByIdAsync(int id, bool trackChanges)
        {
           var product= await GetOneProductAndCheckExits(id, trackChanges);
            return _mapper.Map<ProductDto>(product);
        }

        public async Task<(ProductDtoForUpdate productDtoForUpdate, Product product)> GetOneProductForPatchAsync(int id, bool trackChanges)
        {
            var product = await GetOneProductAndCheckExits(id, trackChanges);
          
            var productDtoForUpdate=_mapper.Map<ProductDtoForUpdate>(product);

            return (productDtoForUpdate, product);  
        }

        public async Task SaveChangesForPatchAsync(ProductDtoForUpdate productDtoForUpdate, Product product)
        {
            _mapper.Map(productDtoForUpdate, product);
            await _manager.SaveAsync();
        }

        public async Task UpdateOneProductAsync(int id, ProductDtoForUpdate productDto, bool trackChanges)
        {
            var entity=await GetOneProductAndCheckExits(id,trackChanges);
            entity=_mapper.Map<Product>(productDto);
            
            _manager.Product.Update(entity);
            await _manager.SaveAsync();
        }

        private async Task<Product> GetOneProductAndCheckExits(int id,bool trackChanges)
        {
            var entity = await _manager.Product.GetOneProductByIdAsync(id, trackChanges);
            if (entity is null)
                throw new ProductNotFoundException(id);
            return entity;
        }
    }
}
