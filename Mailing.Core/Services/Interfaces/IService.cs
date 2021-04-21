using Mailing.Core.Data;
using Mailing.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Mailing.Core.Services.Interfaces
{
    public interface IService<TEntity> : IDisposable where TEntity : BaseEntity
    {
        TEntity FirstOrDefault();
        TEntity FirstOrDefault(Func<TEntity, bool> predicate);
        Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate);
        IUnitOfWork UnitOfWork { get; }
        IQueryable<TEntity> GetAll();
        TEntity GetById(Guid id);
        void Add(TEntity entity);
        void AddRange(IEnumerable<TEntity> entities);
        void Update(TEntity entity);
        void Delete(TEntity entity);
        Task<List<TEntity>> GetAllAsync();
        Task<TEntity> GetByIdAsync(Guid id);
        Task<Int32> AddAsync(TEntity entity);
        Task<Int32> UpdateAsync(TEntity entity);
        Task<Int32> DeleteAsync(TEntity entity);
    }
}
