using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Exceptions
{
    // İstemciden gelen yenileme token'ı geçersiz veya hatalı olduğunda güvenlik amacıyla fırlattığımız hata sınıfımız.
    public class RefreshTokenBadRequestException : BadRequestException
    {
        public RefreshTokenBadRequestException() : base($"Invalid client request.The tokendto has some invalid values.")
        {

        }
    }
}
