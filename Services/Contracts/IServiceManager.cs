using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Contracts
{
    // Controller tarafında her servisi tek tek enjekte etmek yerine, tüm servisleri tek bir merkezden sunduğumuz yönetici arayüzümüz.
    public interface IServiceManager
    {
        ICategoryService CategoryService { get; }
        IProductServices ProductServices { get; }
        IAuthenticationService AuthenticationService { get; }
    }
}
