using Azure;
using Entities.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IServiceManager s_manager;
        public ProductController(IServiceManager manager)
        {
            s_manager = manager;
        }

        [HttpGet]
        public IActionResult GetAllProducts()
        {
            var products = s_manager.ProductServices.GetAllProduct(false);
            return Ok(products);
        }

        [HttpGet("{id:int}")]
        public IActionResult GetOneProduct([FromRoute(Name = "id")] int id)
        {

            var product = s_manager.ProductServices
                .GetOneProductById(id, false);
            if (product is null)
                return NotFound();
            return Ok(product);
        }

        [HttpPost]
        public IActionResult CreateOneProduct([FromBody] Product product)
        {
            if (product is null)
                return BadRequest();
            s_manager.ProductServices.CreateOneProduct(product);
            return StatusCode(201, product);
        }

        [HttpPut("{id:int}")]
        public IActionResult UpdateOneProduct([FromRoute(Name ="id")] int id, [FromBody] Product product)
        {
            if(product is null)
                return BadRequest();

            s_manager.ProductServices.UpdateOneProduct(id, product,true);
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public IActionResult DeleteOneBook([FromRoute(Name ="id")] int id)
        {
            s_manager.ProductServices.DeleteOneProduct(id, false);
            return NoContent();
        }

        [HttpPatch("{id:int}")]
        public IActionResult PartiallyUpdateOneProduct([FromRoute(Name ="id")]int id,
            [FromBody] JsonPatchDocument<Product> productPatch)
        {
            var entity = s_manager.ProductServices
                .GetOneProductById(id, true);
            if(entity is null)
                return NotFound();
            productPatch.ApplyTo(entity);
            s_manager.ProductServices.UpdateOneProduct(id,entity,true);
            return NoContent();
        }
    }
}
