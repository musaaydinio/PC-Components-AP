using Entities.DataTranferObjcets;
using Entities.DataTransferObject;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Contracts
{
    // Kullanıcı kimlik doğrulama, kayıt olma ve JWT (Token) işlemlerini yönettiğimiz servis arayüzümüz.
    public interface IAuthenticationService
    {
        Task<IdentityResult> Register(UserForResgistrationDto userForRegistrationDto);
        Task<bool> ValidateUser(UserForAuthenticationDto userForAuthDto);
        Task<TokenDto> CreateToken(bool exp);
        Task<TokenDto> RefreshToken(TokenDto tokenDto);
    }
}
