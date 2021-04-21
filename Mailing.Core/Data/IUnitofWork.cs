using Kromes.Core.Models;
using Kromes.Core.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Kromes.Core.Data
{
    public interface IUnitOfWork : IDisposable
    {
        int SaveChanges();
        void Dispose(bool disposing);
        IRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity;
        void BeginTransaction();
        int Commit();
        void Rollback();
        Task<int> SaveChangesAsync();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
        Task<int> CommitAsync();
    }
}
