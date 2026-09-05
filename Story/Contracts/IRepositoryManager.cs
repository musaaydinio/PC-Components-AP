using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Contracts
{
    // Unit of Work desenini uygulayarak tüm repository nesnelerimizi tek bir merkezden yönettiğimiz arayüzümüz.
    public interface IRepositoryManager
    {
        ICategoryRepositroy Category { get; }
        IProductRepository Product {  get; }
        Task SaveAsync();
    }
}
