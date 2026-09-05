using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace Presentation.Controllers
{
    // Sunucuya dosya yükleme ve sunucudan dosya indirme işlemlerini yönettiğimiz controller sınıfımız.
    [ApiController]
    [Route("api/files")]
    public class FilesController : ControllerBase
    {
        // İstemciden gelen dosyayı sunucudaki Media klasörüne kaydediyoruz.
        [HttpPost("upload")]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var folder = Path.Combine(Directory.GetCurrentDirectory(), "Media");
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            var path = Path.Combine(folder, file?.FileName);

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return Ok(new
            {
                file = file.FileName,
                path = path,
                size = file.Length
            });
        }

        // Belirtilen dosyayı Media klasöründen okuyup uygun içerik tipiyle istemciye indiriyoruz.
        [HttpGet("download")]
        public async Task<IActionResult> Download(string fileName)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Media", fileName);

            // ContentType
            var provider = new FileExtensionContentTypeProvider();
            if (!provider.TryGetContentType(fileName, out var contentType))
            {
                contentType = "application/octet-stream";
            }

            // Read
            var bytes = await System.IO.File.ReadAllBytesAsync(filePath);
            return File(bytes, contentType, Path.GetFileName(filePath));
        }
    }
}
