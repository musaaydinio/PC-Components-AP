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
    // Tüm servis sınıflarımızı tek bir çatı altında toplayarak  Controller tarafında bağımlılıkları sadeleştirdiğimiz yönetici sınıfımız.
    public class ServicesManager : IServiceManager
    {
        private readonly IProductServices _productServices;
        private readonly IAuthenticationService _authenticationService;
        private readonly ICategoryService _categoryService;

        public ServicesManager(IProductServices productServices, IAuthenticationService authenticationService, ICategoryService categoryService)
        {
            _productServices = productServices;
            _authenticationService = authenticationService;
            _categoryService = categoryService;
        }
        public IProductServices ProductServices => _productServices;

        public IAuthenticationService AuthenticationService => _authenticationService;

        public ICategoryService CategoryService => _categoryService;
    }
}
