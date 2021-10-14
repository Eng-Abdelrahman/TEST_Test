using _3aqarak.BLL.Interfaces;
using _3aqarak.DAL.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace _3aqarak.DAL.Repositories.CustomRepositories
{
    public class ScalarValsRepository<TEntity> : IScalarValsRepository<TEntity> where TEntity : class
    {
        private readonly DbSet<TEntity> _dbSet;
        private readonly RealEstateDB _dbContext;

        public ScalarValsRepository(RealEstateDB context)
        {
            this._dbContext = context;
            this._dbSet = context.Set<TEntity>();
        }
        public void Dispose()
        {
            this._dbContext.Dispose();
        }

        public object GetMaxIntValue(Expression<Func<TEntity, int>> selector)
        {
            return _dbSet.Max(selector);
        }

        public object GetMaxDecimalValue(Expression<Func<TEntity, decimal>> selector)
        {
            return _dbSet.Max(selector);
        }

        public object GetMinlValue(Expression<Func<TEntity, DateTime>> selector)
        {
            return _dbSet.Min(selector);
        }
    }
}
