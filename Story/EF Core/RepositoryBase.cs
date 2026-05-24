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
    public abstract class RepositoryBase<T> :IRepositoryBase<T>
        where T : class
    {
        protected readonly StoreDbcontex _contex;

        protected RepositoryBase(StoreDbcontex contex )
        {
         _contex = contex;   
        }

        public void Create(T entity)=> _contex.Set<T>().Add(entity);
        

        public void Delete(T entity)=> _contex.Set<T>().Remove(entity);
        

        public IQueryable<T> FindAll(bool trackChanges)=>!trackChanges ? _contex.Set<T>()
            .AsNoTracking():_contex.Set<T>();

        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges)=>!trackChanges ? _contex.Set<T>().Where(expression)
            .AsNoTracking() :_contex.Set<T>().Where(expression);
       

        public void Update(T entity)=> _contex.Set<T>().Update(entity);
        
    }
}
