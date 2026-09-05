using Entities.DataTranferObjcets;
using Entities.DataTransferObject;
using Microsoft.AspNetCore.Mvc;
using Presentation.ActionFilters;
using Services.Contracts;


namespace Presentation.Controllers
{
    // Kullanıcı kayıt, giriş ve token yenileme gibi kimlik doğrulama isteklerini karşıladığımız controller sınıfımız.
    [ApiController]
    [Route("api/authentication")]
    [ApiExplorerSettings(GroupName = "V1")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IServiceManager _services;
        public AuthenticationController(IServiceManager services)
        {
            _services = services;
        }
        // Yeni kullanıcı kaydı oluşturmak için HTTP POST isteğini karşılıyoruz.
        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> RegisterUser(UserForResgistrationDto userForResgistrationDto)
        {
            var result = await _services.AuthenticationService.Register
                (userForResgistrationDto);
            if (result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.TryAddModelError(error.Code, error.Description);
                }
                return BadRequest(ModelState);
            }
            return StatusCode(201);
        }
        // Kullanıcı giriş bilgilerini doğrulayıp istemciye JWT ve Refresh Token dönüyoruz.
        [HttpPost("login")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> Authenticate([FromBody] UserForAuthenticationDto user)
        {
            if (!await _services.AuthenticationService.ValidateUser(user))
                return Unauthorized();

            var tokenDto = await _services.AuthenticationService.CreateToken(exp: true);

            return Ok(tokenDto);
        }
        // Süresi dolan erişim token'ını yenilemek için gelen refresh token isteğini karşılıyoruz.
        [HttpPost("refresh")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> Refresh([FromBody] TokenDto tokenDto)
        {
            var tokenDtoToReturn = await _services
                .AuthenticationService
                .RefreshToken(tokenDto);
            return Ok(tokenDtoToReturn);
        }
    }
}
