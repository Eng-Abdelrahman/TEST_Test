using _3aqarak.BLL.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace _3aqarak.BLL.Interfaces
{
    public interface IFilterService<TEntity> : IDisposable where TEntity : class
    {

        IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> filterPredicate);
        IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> filterPredicate, Expression<Func<TEntity, dynamic>> selector);
    }
}
