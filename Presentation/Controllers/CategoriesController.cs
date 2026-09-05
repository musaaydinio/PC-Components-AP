using Microsoft.AspNetCore.Mvc;
using Services.Contracts;

namespace Presentation.Controllers
{
    // Kategori verilerini dış dünyaya sunduğumuz HTTP isteklerini yöneten controller sınıfımız.

    [ApiController]
    [Route("api/categories")]
    public class CategoriesController : ControllerBase
    {
        private readonly IServiceManager _services;

        public CategoriesController(IServiceManager services)
        {
            _services = services;
        }
        // Veritabanındaki tüm kategorileri getirmek için HTTP GET isteğini karşılıyoruz.
        [HttpGet]
        public async Task<IActionResult> GetAllCategoriesAsync()
        {
            return Ok(await _services
               .CategoryService
                .GetAllCategoryiesAsync(false));
        }
        // URL'den gelen ID değerine göre tek bir kategoriyi getiren HTTP GET isteğini karşılıyoruz.
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetAllCategoriesAsync([FromRoute] int id)
        {
            return Ok(await _services
                .CategoryService
                .GetOneCategoryByIdAsync(id, false));
        }
    }
}
