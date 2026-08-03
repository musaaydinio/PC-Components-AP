using AutoMapper;
using Entities.DataTransferObject;
using Repository.Contracts;
using Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class ServicesManager : IServiceManager
    {
        private readonly Lazy<IProductServices> _productServices;
        public ServicesManager(IRepositoryManager repositoryManager,ILoggerServices loggerServices,
            IMapper mapper,IDataShaper<ProductDto> shaper)
        {
            _productServices= new Lazy<IProductServices>(()=>new ProductManager(repositoryManager, loggerServices,mapper,shaper));    
        }
        public IProductServices ProductServices => _productServices.Value;
    }
}
