using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Exceptions
{
    // API genelinde "404 Not Found" durumlarını hiyerarşik olarak yönetmek için oluşturduğumuz abstract temel sınıfımız.
    public abstract class NotFoundException: Exception
    {
        protected NotFoundException(string message): base(message)
        {
            
        }
    }
}
