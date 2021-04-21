using Mailing.Core.Data;
using Mailing.Core.Models;
using Mailing.Core.Repositories.Interfaces;
using Mailing.Core.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Mailing.Core.Services
{

    public class Service<TEntity> : IService<TEntity> where TEntity : BaseEntity
    {
        public IUnitOfWork UnitOfWork { get; private set; }
        private readonly IRepository<TEntity> _repository;
        private bool _disposed;
        protected List<ValidationResult> results = new List<ValidationResult>();

        public Service(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
            _repository = UnitOfWork.Repository<TEntity>();
        }

      

        protected bool IsValid<T>(T entity)
        {
            return Validator.TryValidateObject(entity, new ValidationContext(entity, null, null),
              results, false);
        }
      

        public TEntity FirstOrDefault(Func<TEntity, bool> predicate)
        {
            return _repository.GetAll().FirstOrDefault(predicate);
        }

        public IQueryable<TEntity> All { get { return _repository.Query(); } }

        public TEntity FirstOrDefault()
        {
            return _repository.GetAll().FirstOrDefault();
        }

        public IQueryable<TEntity> GetAll()
        {
            return _repository.GetAll();
        }
        public TEntity GetById(Guid id)
        {
            return _repository.GetSingle(id);
        }

        public void Add(TEntity entity)
        {
            _repository.Insert(entity);
            UnitOfWork.SaveChanges();
        }

        public void AddRange(IEnumerable<TEntity> entities)
        {
            _repository.InsertRange(entities);
            UnitOfWork.SaveChanges();
        }

        public void Update(TEntity entity)
        {
            _repository.Update(entity);
            UnitOfWork.SaveChanges();
        }

        public void Delete(TEntity entity)
        {
            if (entity is BaseEntity)
                _repository.Remove(entity);
            else
                _repository.Delete(entity);
            UnitOfWork.SaveChanges();
        }


        public async Task<List<TEntity>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

      
        public async Task<TEntity> GetByIdAsync(Guid id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await Task.FromResult(_repository.GetAll(predicate).Count());
        }

        public async Task<Int32> AddAsync(TEntity entity)
        {
            _repository.Insert(entity);
            return await UnitOfWork.SaveChangesAsync();
        }

       

        public async Task<Int32> UpdateAsync(TEntity entity)
        {
            _repository.Update(entity);
            return await UnitOfWork.SaveChangesAsync();
        }

        public async Task<Int32> DeleteAsync(TEntity entity)
        {
            _repository.Delete(entity);
            return await UnitOfWork.SaveChangesAsync();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public virtual void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                UnitOfWork.Dispose();
            }
            _disposed = true;
        }

        public IEnumerable<ValidationResult> Errors
        {
            get
            {
                if (results.Count > 0)
                {
                    return results;
                }
                return Enumerable.Empty<ValidationResult>();
            }
        }

        public string this[string columnName]
        {
            get
            {
                var validatioResult = results.FirstOrDefault(r => r.MemberNames.FirstOrDefault() == columnName);
                return validatioResult == null ? string.Empty : validatioResult.ErrorMessage;
            }
        }

        public Boolean HasError
        {
            get
            {
                if (results.Count > 0)
                {
                    return true;
                }

                return false;
            }
        }
    }
}
