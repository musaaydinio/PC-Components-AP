using Repository.Contracts;
using Story.EF_Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.EF_Core
{
    // Unit of Work desenini uygulayarak tüm repository nesnelerini ve veritabanı kayıt işlemlerini tek bir merkezden yönettiğimiz sınıfımız.
    public class RepositoryManager:IRepositoryManager
    {
        private readonly StoreDbcontex _contex;
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepositroy _categoryRepository;

        public RepositoryManager(StoreDbcontex contex, IProductRepository productRepository, ICategoryRepositroy categoryRepository)
        {
            _contex = contex;
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
        }

        // Dışarıdan sadece okunabilir (get) şekilde ürünler repository'sine güvenli erişim sağlıyoruz.
        public IProductRepository Product => _productRepository;

        // Dışarıdan sadece okunabilir şekilde kategori repository'sine güvenli erişim sağlıyoruz.
        public ICategoryRepositroy Category => _categoryRepository;

        // Yapılan tüm ekleme, silme ve güncelleme işlemlerini tek bir transaction ile veritabanına yansıtıyoruz.
        public async Task SaveAsync()
        {
           await _contex.SaveChangesAsync();
        }
    }
}
