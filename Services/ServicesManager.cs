using AutoMapper;
using Entities.DataTransferObject;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
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
        private readonly Lazy<IAuthenticationService> _authenticationService;
        public ServicesManager(IRepositoryManager repositoryManager,ILoggerServices loggerServices,
            IMapper mapper, UserManager<User> userManager,
            IDataShaper<ProductDto> shaper, IConfiguration configuration)
        {
            _productServices= new Lazy<IProductServices>(()=>new ProductManager(repositoryManager, 
                loggerServices,mapper,shaper));

            _authenticationService = new Lazy<IAuthenticationService>(() =>
            new AuthenticationManager(loggerServices, mapper, userManager, configuration));
        }
        public IProductServices ProductServices => _productServices.Value;

        public IAuthenticationService AuthenticationService => _authenticationService.Value;
    }
}
