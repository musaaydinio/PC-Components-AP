using Azure;
using Entities.DataTransferObject;
using Entities.Exceptions;
using Entities.Models;
using Entities.RequestFeatures;
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
    [ServiceFilter(typeof(LogFilterAttribute), Order =2)]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IServiceManager s_manager;
        public ProductController(IServiceManager manager)
        {
            s_manager = manager;
        }

        [HttpHead]
        [HttpGet(Name ="GetAllProduct")]        
        public async Task<IActionResult> GetAllProducts([FromQuery] ProductParameters productParameters)
        {
            var pagedResult = await s_manager.ProductServices.GetAllProductAsync(productParameters, false);
            Response.Headers.Add("X-Pagination",JsonSerializer
                .Serialize(pagedResult.metaData));
            return Ok(pagedResult.product);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetOneProduct([FromRoute(Name = "id")] int id)
        {

            var product = await s_manager.ProductServices
                .GetOneProductByIdAsync(id, false);
            if (product is null)
                throw new ProductNotFoundException(id);
            return Ok(product);
        }
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [HttpPost]
        public async Task<IActionResult> CreateOneProduct([FromBody] ProductDtoForInsertion productDto)
        {         
            var product =await s_manager.ProductServices.CreateOneProductAsync(productDto);
            return StatusCode(201, product);
        }
      
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateOneProduct([FromRoute(Name ="id")] int id, [FromBody] ProductDtoForUpdate productDto)
        {           

            await s_manager.ProductServices.UpdateOneProductAsync(id, productDto,true);
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteOneBook([FromRoute(Name ="id")] int id)
        {
            await s_manager.ProductServices.DeleteOneProductAsync(id, false);
            return NoContent();
        }

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

        [HttpOptions]
        public IActionResult GetProductsOptions()
        {
            Response.Headers.Add("Allow", "GET, PUT, POST, PUTCH, DELETE, HEAD, OPTİONS");
            return Ok();
        }
    }
}
