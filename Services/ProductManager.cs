using AutoMapper;
using Entities.DataTransferObject;
using Entities.Exceptions;
using Entities.Models;
using Entities.RequestFeatures;
using Repository.Contracts;
using Services.Contracts;
using System.ComponentModel.DataAnnotations;
using System.Dynamic;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Services
{
    // Ürünlerle ilgili iş kurallarını işlettiğimiz, DTO dönüşümlerini ve sayfalama/filtreleme mantığını yönettiğimiz servis sınıfımız.
    public class ProductManager:IProductServices
    {
        private readonly ICategoryService _categoryService;
        private readonly ILoggerServices _logger;
        private readonly IRepositoryManager _manager;
        private readonly IMapper _mapper;
        private readonly IDataShaper<ProductDto> _dataShaper;
        public ProductManager(IRepositoryManager manager, ILoggerServices logger, IMapper mapper,
            ICategoryService categoryService, IDataShaper<ProductDto> dataShaper)
        {
            _logger = logger;
            _manager = manager;
            _mapper = mapper;
            _categoryService = categoryService;
            _dataShaper = dataShaper;
        }

        // Kategori varlığını kontrol ettikten sonra DTO'yu entity'ye dönüştürüp yeni ürünü veritabanına kaydediyoruz.
        public async Task<ProductDto> CreateOneProductAsync(ProductDtoForInsertion productDto)
        {
            var category = await _categoryService.GetOneCategoryByIdAsync(productDto.CategoryId, false);

            var entity= _mapper.Map<Product>(productDto);
            _manager.Product.CreateOneProduct(entity);
            await _manager.SaveAsync();
            return _mapper.Map<ProductDto>(entity);
        }

        // Silinecek ürünün varlığını doğrulayıp veritabanından siliyoruz.
        public async Task DeleteOneProductAsync(int id, bool trackChanges)
        {
           var entity= await GetOneProductAndCheckExits(id,trackChanges);
            _manager.Product.DeleteOneProduct(entity);
            await _manager.SaveAsync();
        }

        // Fiyat aralığı geçerliliğini kontrol ettikten sonra ürünleri filtreli, sayfalanmış ve şekillendirilmiş olarak getiriyoruz.
        public async Task<(IEnumerable<ExpandoObject> product, MetaData metaData)>
            GetAllProductAsync(ProductParameters productParameters,
            bool trackChanges)
        {
            if (!productParameters.ValidPriceRange)
                throw new PriceOutOfRangeBadRequestException();

            var productsWithMetaData = await _manager.Product.GetAllProductAsync(productParameters, trackChanges);

            // 2. KONTROL: İstenen sayfada ürün kalmadıysa veya liste boşsa hata fırlatılır
            if (!productsWithMetaData.Any())
                throw new ProductNotFoundForParametersException(productParameters.PageNumber);

            // DTO Mmapping ve DataShaper adımların aynen korunuyor
            var productDto = _mapper.Map<IEnumerable<ProductDto>>(productsWithMetaData);
            var shapedData = _dataShaper.ShapeData(productDto, productParameters.Fields);

            return (product: shapedData, metaData: productsWithMetaData.MetaData);
        }

        // Tüm ürünleri sayfalama yapmadan düz bir liste halinde getiriyoruz.
        public async Task<List<Product>> GetAllProductAsync(bool trackChanges)
        {
            var product= await _manager.Product.GetAllProductAsync(trackChanges);
            return product;
        }

        // Ürünleri ilişkili olduğu kategori detaylarıyla birlikte veritabanından çekiyoruz.
        public async Task<IEnumerable<Product>> GetAllProductWithDetails(bool trackChanges)
        {
            return await _manager.Product.GetAllProductWithDetails(trackChanges);
        }

        // Tekil ürün varlığını kontrol edip DTO formatına dönüştürerek sunuyoruz.
        public async Task<ProductDto> GetOneProductByIdAsync(int id, bool trackChanges)
        {
           var product= await GetOneProductAndCheckExits(id, trackChanges);
            return _mapper.Map<ProductDto>(product);
        }

        // Patch işlemi öncesinde ürünü bulup güncellemeye uygun DTO kopyasını hazırlıyoruz.
        public async Task<(ProductDtoForUpdate productDtoForUpdate, Product product)> GetOneProductForPatchAsync(int id, bool trackChanges)
        {
            var product = await GetOneProductAndCheckExits(id, trackChanges);
          
            var productDtoForUpdate=_mapper.Map<ProductDtoForUpdate>(product);

            return (productDtoForUpdate, product);  
        }
        // Patch işlemi sonrası değişen DTO verilerini entity üzerine yansıtıp kaydediyoruz.
        public async Task SaveChangesForPatchAsync(ProductDtoForUpdate productDtoForUpdate, Product product)
        {
            _mapper.Map(productDtoForUpdate, product);
            await _manager.SaveAsync();
        }
        // Ürünün varlığını doğruladıktan sonra gelen DTO verileriyle güncelleme işlemini yapıyoruz.
        public async Task UpdateOneProductAsync(int id, ProductDtoForUpdate productDto, bool trackChanges)
        {
            var entity=await GetOneProductAndCheckExits(id,trackChanges);
            _mapper.Map(productDto, entity);
            
            _manager.Product.Update(entity);
            await _manager.SaveAsync();
        }
        // Tekrarlı kod yazmamak için ürünün varlığını kontrol eden ve bulamazsa özel Exception fırlatan yardımcı metodumuz.
        private async Task<Product> GetOneProductAndCheckExits(int id,bool trackChanges)
        {
            var entity = await _manager.Product.GetOneProductByIdAsync(id, trackChanges);
            if (entity is null)
                throw new ProductNotFoundException(id);
            return entity;
        }
    }
}
