using Microsoft.EntityFrameworkCore;
using Story.Contracts;
using Story.EF_Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repository.EF_Core
{
    // Tüm repository sınıflarımız için veritabanı işlemlerinin temelini attığımız soyut (abstract) sınıfımız.
    public abstract class RepositoryBase<T> :IRepositoryBase<T>
        where T : class
    {
        protected readonly StoreDbcontex _contex;

        protected RepositoryBase(StoreDbcontex contex )
        {
         _contex = contex;   
        }

        // Gelen varlığı veritabanına eklemek üzere izlemeye alıyoruz.
        public void Create(T entity)=> _contex.Set<T>().Add(entity);

        // Gelen varlığı veritabanından silinmek üzere işaretliyoruz.
        public void Delete(T entity)=> _contex.Set<T>().Remove(entity);

        // İsteğe bağlı olarak takip (tracking) mekanizmasını kapatarak, ilgili tablodaki tüm verileri getiriyoruz.
        public IQueryable<T> FindAll(bool trackChanges)=>!trackChanges ? _contex.Set<T>()
            .AsNoTracking():_contex.Set<T>();

        // Belirttiğimiz koşula (expression) uyan verileri filtreleyerek getiriyoruz.
        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges)=>!trackChanges ? _contex.Set<T>().Where(expression)
            .AsNoTracking() :_contex.Set<T>().Where(expression);
       
        // Var olan bir kaydı güncellemek üzere EF Core'a bildiriyoruz.
        public void Update(T entity)=> _contex.Set<T>().Update(entity);
        
    }
}
