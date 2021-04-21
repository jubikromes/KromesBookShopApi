using Kromes.Core.Context;
using Kromes.Core.Models;
using Kromes.Core.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Kromes.Core.Data.Repositories
{
    public class EntityRepository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
    {
        private BookShopDbContext context;
        private DbSet<TEntity> _dbSet;

        public EntityRepository(BookShopDbContext context)
        {
            this.context = context;
        }

        protected virtual DbSet<TEntity> Entities
        {
            get
            {
                if (_dbSet == null)
                {
                    _dbSet = context.Set<TEntity>();
                }

                return _dbSet;
            }
        }


        public TEntity GetById(object id)
        {
            return Entities.Find(id);
        }

        public IQueryable<TEntity> GetAll()
        {
            return Entities;
        }

        public IQueryable<TEntity> GetAll(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> query = Entities;

            foreach (Expression<Func<TEntity, object>> include in includes)
                query = query.Include(include);

            if (filter != null)
                query = query.Where(filter);

            if (orderBy != null)
                query = orderBy(query);

            return query;
        }

        public IQueryable<TEntity> GetAllIncluding(params Expression<Func<TEntity, object>>[] includeProperties)
        {
            var entities = IncludeProperties(includeProperties);
            return entities;
        }

        public TEntity GetSingle(Guid id)
        {
            return Entities.FirstOrDefault(t => t.Id == id);
        }


        public Task<List<TEntity>> GetAllAsync()
        {
            return Entities.ToListAsync();
        }

        public async Task<TEntity> GetByIdAsync(Guid id)
        {
            return await Entities.FirstOrDefaultAsync(x => x.Id == id);
        }


        public Task<List<TEntity>> GetAllIncludingAsync(params Expression<Func<TEntity, string>>[] includeProperties)
        {
            return GetAllIncludingAsync(includeProperties);
        }

        public Task<List<TEntity>> GetAllIncludingAsync(params Expression<Func<TEntity, object>>[] includeProperties)
        {
            var entities = IncludeProperties(includeProperties);
            return entities.ToListAsync();
        }

    


        private IQueryable<TEntity> IncludeProperties(params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> entities = Entities;

            foreach (var includeProperty in includeProperties)
            {
                entities = entities.Include(includeProperty);
            }
            return entities;
        }

        public virtual IQueryable<TEntity> Table
        {
            get
            {
                return this.Entities;
            }
        }

        public bool Any(Expression<Func<TEntity, bool>> filter)
        {
            IQueryable<TEntity> query = Entities;
            return query.Any(filter);
        }

        public virtual void Update(TEntity entityToUpdate)
        {
            context.SetAsModified<TEntity>(entityToUpdate);
        }

        public void Insert(TEntity entity)
        {
            Entities.Add(entity);
        }

        public void InsertRange(IEnumerable<TEntity> entities)
        {
            Entities.AddRange(entities);
        }

        public virtual IEnumerable<TEntity> GetMany(Func<TEntity, bool> where)
        {
            return Entities.Where(where).ToList();
        }

       
        public virtual IQueryable<TEntity> GetManyQueryable(Func<TEntity, bool> where)
        {
            return Entities.Where(where).AsQueryable();
        }

        public virtual IQueryable<TEntity> Query(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null)
        {
            IQueryable<TEntity> query = Entities;

            if (filter != null)
                query = query.Where(filter);

            if (orderBy != null)
                query = orderBy(query);

            return query;
        }

        public TEntity GetFirstOrDefault(Expression<Func<TEntity, bool>> filter = null, params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> query = Entities;

            foreach (Expression<Func<TEntity, object>> include in includes)
                query = query.Include(include);

            return query.FirstOrDefault(filter);
        }

        public void Remove(TEntity entityToDelete)
        {
            context.SetAsDeleted<TEntity>(entityToDelete);
        }

        public void Delete(TEntity entityToDelete)
        {
            context.SetAsDetached<TEntity>(entityToDelete);
        }

        public void Dispose()
        {
            //Dispose(true);
            GC.SuppressFinalize(this);
        }

        public async Task<int> Count(Expression<Func<TEntity, bool>> predicate)
        {
            return await _dbSet.CountAsync(predicate);
        }
    }
}
