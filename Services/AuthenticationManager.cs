using AutoMapper;
using Entities.DataTranferObjcets;
using Entities.DataTransferObject;
using Entities.Exceptions;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Services.Contracts;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;


namespace Services
{
    // Kullanıcı kayıt, giriş ve JWT (Token) üretim süreçlerinin tüm iş kurallarını yönettiğimiz servis sınıfımız.
    public class AuthenticationManager : IAuthenticationService
    {
        private readonly ILoggerServices _loggerService;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _config;

        private User? _user;

        public AuthenticationManager(ILoggerServices loggerService, IMapper mapper,
            UserManager<User> userManager,
            IConfiguration config)
        {
            _loggerService = loggerService;
            _mapper = mapper;
            _userManager = userManager;
            _config = config;
        }

        // Doğrulanmış kullanıcı için yeni bir erişim token'ı (JWT) ve yenileme token'ı (Refresh Token) üretiyoruz.
        public async Task<TokenDto> CreateToken(bool exp)
        {
            var signinCredentials = GetSiginCredentials();
            var claims = await GetClaims();
            var tokenOpstions = GenerateTokenOpstions(signinCredentials, claims);

            var refreshToken = GenerateRefreshToken();
            _user.RefreshToken = refreshToken;

            if (exp)
                _user.RefreshTokenExpriyTime = DateTime.Now.AddDays(7);

            await _userManager.UpdateAsync(_user);

            var accessToken = new JwtSecurityTokenHandler().WriteToken(tokenOpstions);
            return new TokenDto()
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }

        // Dışarıdan gelen DTO'yu User nesnesine çevirip sisteme yeni bir kullanıcı olarak kaydediyoruz.
        public async Task<IdentityResult> Register(UserForResgistrationDto userForRegistrationDto)
        {
            var user = _mapper.Map<User>(userForRegistrationDto);

            var result = await _userManager.CreateAsync(user, userForRegistrationDto.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRolesAsync(user, userForRegistrationDto.Roles);
            }
            return result;
        }

        // İstemciden gelen kullanıcı adı ve şifre bilgilerinin veritabanındaki kayıtlarla eşleşip eşleşmediğini denetliyoruz.
        public async Task<bool> ValidateUser(UserForAuthenticationDto userForAuthDto)
        {
            _user = await _userManager.FindByNameAsync(userForAuthDto.UserName);
            var result = (_user != null && await _userManager.CheckPasswordAsync(_user, userForAuthDto.Password));

            if (!result)
            {
                _loggerService.LogWarning($"{nameof(ValidateUser)} : Authentication failed.Wrog username pssword.");
            }
            return result;
        }

        // Appsettings deki anahtarımızı(SecretKey) kullanarak token imzalama güvenliğini sağlıyoruz.
        private SigningCredentials GetSiginCredentials()
        {
            var jwtsettings = _config.GetSection("JwtSetting");
            var key = Encoding.UTF8.GetBytes(jwtsettings["secretKey"]);
            var secret = new SymmetricSecurityKey(key);
            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        }

        // Token içerisine gömeceğimiz kullanıcı bilgilerini yapılandırıyoruz.
        private async Task<List<Claim>> GetClaims()
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name,_user.UserName)
            };
            var roles = await _userManager.GetRolesAsync(_user);

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            return claims;
        }

        // Token'ın kim tarafından, kime ve ne süreyle geçerli olacağını ayarlıyoruz.
        private JwtSecurityToken GenerateTokenOpstions(SigningCredentials signinCredentials, List<Claim> claims)
        {
            var jwtsettings = _config.GetSection("JwtSetting");
            var tokenOpt = new JwtSecurityToken(
                issuer: jwtsettings["validIssuer"],
                audience: jwtsettings["validAudience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(jwtsettings["expires"])),
                signingCredentials: signinCredentials);
            return tokenOpt;
        }
        // Kriptografik olarak güvenli, rastgele bir Refresh Token metni üretiyoruz.
        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var randmgen = RandomNumberGenerator.Create())
            {
                randmgen.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }
        // Süresi dolmuş token'ın imza ve algoritma kontrollerini yapıp, içindeki kullanıcı bilgilerini çıkarıyoruz.
        private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var jwtSetting = _config.GetSection("JwtSetting");
            var secretKey = jwtSetting["secretKey"];

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtSetting["validIssuer"],
                ValidAudience = jwtSetting["validAudience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;

            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters,
                out securityToken);

            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken is null ||
                !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Invalid token.");
            }
            return principal;
        }
        // İstemciden gelen Refresh Token'ın geçerliliğini ve süresini kontrol edip, onaylanırsa yeni bir token seti dönüyoruz.
        public async Task<TokenDto> RefreshToken(TokenDto tokenDto)
        {
            var principal = GetPrincipalFromExpiredToken(tokenDto.AccessToken);
            var user = await _userManager.FindByNameAsync(principal.Identity.Name);

            if (user is null ||
                user.RefreshToken != tokenDto.RefreshToken ||
                user.RefreshTokenExpriyTime <= DateTime.Now)
                throw new RefreshTokenBadRequestException();

                _user = user;

            return await CreateToken(exp: false);
        }
    }
}
    


