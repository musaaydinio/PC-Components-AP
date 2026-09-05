using Entities.DataTransferObject;
using Microsoft.AspNetCore.Mvc;
using Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    // API v2 versiyonu için ürün isteklerini karşıladığımız controller sınıfımız.
    [Route("api/product")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "V2")]
    public class ProductV2Controller : ControllerBase
    {
        private readonly IServiceManager _serviceManager;
        public ProductV2Controller(IServiceManager serviceManager)
        {            
            _serviceManager = serviceManager;   
        }
        // V2 versiyonuna özel olarak tüm ürünlerin sadece Id ve Name bilgilerini DTO üzerinden dönüyoruz.
        [HttpGet]
        public async Task<IActionResult> GetAllProductAsync()
        {
            var product = await _serviceManager.ProductServices.GetAllProductAsync(false);
            var ptoductV2 = product.Select(b => new ProductDto
            {
                Name = b.Name,
                Id = b.Id
            }).ToList();
            return Ok(ptoductV2);
        }
    }
}
