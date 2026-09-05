using Azure;
using Entities.DataTransferObject;
using Entities.Exceptions;
using Entities.Models;
using Entities.RequestFeatures;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Presentation.ActionFilters;
using Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    // Ürünlerle ilgili tüm HTTP isteklerini karşıladığımız ana controller sınıfımız.
    [Authorize]
    [ServiceFilter(typeof(LogFilterAttribute))]
    [Route("api/product")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "V1")]
    public class ProductController : ControllerBase
    {
        private readonly IServiceManager s_manager;
        public ProductController(IServiceManager manager)
        {
            s_manager = manager;
        }
        // Ürünleri sayfalama ve filtreleme parametreleriyle çekip, sayfalama bilgisini header'a ekleyerek dönüyoruz.
        [AllowAnonymous]
        [HttpHead]
        [HttpGet(Name ="GetAllProduct")]        
        public async Task<IActionResult> GetAllProducts([FromQuery] ProductParameters productParameters)
        {
            var pagedResult = await s_manager.ProductServices.GetAllProductAsync(productParameters, false);
            Response.Headers.Add("X-Pagination",JsonSerializer
                .Serialize(pagedResult.metaData));
            return Ok(pagedResult.product);
        }
        // Ürünleri ilişkili detaylarıyla birlikte getiren HTTP GET metodumuz.
        [AllowAnonymous]
        [HttpGet("details")]
        public async Task<IActionResult> GetAllBooksWithDetailsAsync()
        {
            return Ok(await s_manager
                .ProductServices
                .GetAllProductWithDetails(false));
        }
        // URL'den gelen ID'ye göre tek bir ürün getiriyoruz; ürün veritabanında yoksa hatayı fırlatıyoruz.
        [AllowAnonymous]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetOneProduct([FromRoute(Name = "id")] int id)
        {
            var product = await s_manager.ProductServices
                .GetOneProductByIdAsync(id, false);
            if (product is null)
                throw new ProductNotFoundException(id);
            return Ok(product);
        }

        // ValidationFilter doğrulamasından geçen DTO ile yeni ürün ekleyip 201 Created yanıtı dönüyoruz.
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [HttpPost]
        public async Task<IActionResult> CreateOneProduct([FromBody] ProductDtoForInsertion productDto)
        {         
            var product =await s_manager.ProductServices.CreateOneProductAsync(productDto);
            return StatusCode(201, product);
        }

        // Belirtilen ID'deki ürünü gelen DTO verileriyle tamamen güncelliyoruz.
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateOneProduct([FromRoute(Name ="id")] int id, [FromBody] ProductDtoForUpdate productDto)
        {           

            await s_manager.ProductServices.UpdateOneProductAsync(id, productDto,true);
            return NoContent();
        }
        // ID bilgisi verilen ürünü veritabanından siliyoruz.
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteOneBook([FromRoute(Name ="id")] int id)
        {
            await s_manager.ProductServices.DeleteOneProductAsync(id, false);
            return NoContent();
        }
        // JsonPatch kullanarak ürünün sadece belirtilen alanlarını kısmi olarak güncelliyoruz.
        [HttpPatch("{id:int}")]
        public async Task<IActionResult> PartiallyUpdateOneProduct([FromRoute(Name ="id")]int id,
            [FromBody] JsonPatchDocument<ProductDtoForUpdate> productPatch)
        {
            if(productPatch is null)
                return BadRequest();

            var result = await s_manager.ProductServices.GetOneProductForPatchAsync(id, false);           
            
            productPatch.ApplyTo(result.productDtoForUpdate,ModelState);

            TryValidateModel(result.productDtoForUpdate);

            if(!ModelState.IsValid)
                return UnprocessableEntity(ModelState);

            await s_manager.ProductServices.SaveChangesForPatchAsync(result.productDtoForUpdate, result.product);
           
            return NoContent();
        }
        // İstemciye bu endpoint üzerinde desteklenen HTTP yöntemlerini (Allow header) bildiriyoruz.
        [HttpOptions]
        public IActionResult GetProductsOptions()
        {
            Response.Headers.Add("Allow", "GET, PUT, POST, PUTCH, DELETE, HEAD, OPTIONS");
            return Ok();
        }
    }
}
