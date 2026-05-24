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
        public ServicesManager(IRepositoryManager repositoryManager,ILoggerServices loggerServices)
        {
            _productServices= new Lazy<IProductServices>(()=>new ProductManager(repositoryManager, loggerServices));    
        }
        public IProductServices ProductServices => _productServices.Value;
    }
}
