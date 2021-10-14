using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace _3aqarak.BLL.Interfaces
{
    public interface IRepository<TEntity>: IDisposable where TEntity : class
    {

        void Add(TEntity entity);
        void Update(TEntity entity);
        void Remove(TEntity entity);
        Task<TEntity> ReloadAsync(TEntity entity);

        Task<TEntity> GetAsync(int id);

        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<IEnumerable<TEntity>> GetALLByPageAsync(int pageNumber, int pageSize,
            OrderType orderType, Expression<Func<TEntity, bool>> orderPredicate);
        Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> filterPredicate);
        Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> filterPredicate, Expression<Func<TEntity, dynamic>> selector);

        IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> filterPredicate);

    }
}
