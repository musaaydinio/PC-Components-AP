using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Exceptions
{
    // API genelinde oluşabilecek "400 Bad Request" hatalarını tek bir merkezden türetmek için kurguladığımız abstract temel sınıfımız.
    public abstract class BadRequestException : Exception
    {
        protected BadRequestException(string message) :
            base(message)
        {
            
        }

    }
}
