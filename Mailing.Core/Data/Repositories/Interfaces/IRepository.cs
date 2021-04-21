using Mailing.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Mailing.Core.Repositories.Interfaces
{
    public interface IRepository<TEntity> : IDisposable where TEntity : BaseEntity
    {
 
        IQueryable<TEntity> GetAll(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            params Expression<Func<TEntity, object>>[] includes);
        TEntity GetSingle(Guid id);
        Task<List<TEntity>> GetAllAsync();
  
        Task<TEntity> GetByIdAsync(Guid id);

        Task<int> Count(Expression<Func<TEntity, bool>> predicate);
        
        IQueryable<TEntity> Query(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null);

       
        TEntity GetById(object id);

       
        TEntity GetFirstOrDefault(
            Expression<Func<TEntity, bool>> filter = null,
            params Expression<Func<TEntity, object>>[] includes);

      
        void Insert(TEntity entity);

        void InsertRange(IEnumerable<TEntity> entities);

        
        void Update(TEntity entity);

       
        void Delete(TEntity entity);

        void Remove(TEntity entityToDelete);

    }
}
