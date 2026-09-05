using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DataTranferObjcets
{
    // Kimlik doğrulama süreçlerinde istemciye güvenli bir şekilde döneceğimiz token taşıyıcımız.
    public record TokenDto
    {
        public String AccessToken { get; init; }
        public String RefreshToken { get; init; }
    }
}

