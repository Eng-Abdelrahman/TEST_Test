using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace _3aqarak.BLL.Interfaces
{
    public interface IScalarValsRepository<TEntity>: IDisposable where TEntity : class
    {
        object GetMaxIntValue(Expression<Func<TEntity, int>> selector);
        object GetMaxDecimalValue(Expression<Func<TEntity, decimal>> selector);
        object GetMinlValue(Expression<Func<TEntity, DateTime>> selector);
    }
}
