using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Story.Contracts
{
    // Tüm repository sınıflarımız için ortak olan temel veritabanı operasyonlarını tanımladığımız generic arayüzümüz.
    public interface IRepositoryBase<T>
    {
        // Koşulsuz tüm verileri sorguluyoruz. trackChanges ile EF Core takip mekanizmasını açıp kapatıyoruz.
        IQueryable<T> FindAll(bool trackChanges);
        // Belirli bir şarta (LINQ expression) uyan verileri filtreleyerek getiriyoruz.
        IQueryable<T> FindByCondition(Expression<Func<T,bool>> expression,bool trackChanges);
        void Create(T entity);
        void Update(T entity); 
        void Delete(T entity);
    }
}
