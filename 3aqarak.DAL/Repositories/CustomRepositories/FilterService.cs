using _3aqarak.BLL.Dto;
using _3aqarak.BLL.Interfaces;
using _3aqarak.DAL.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;


namespace _3aqarak.DAL.Services
{
    public class FilterService<TEntity> : IFilterService<TEntity> where TEntity : class
    {
        private readonly DbSet<TEntity> _dbSet;
        private readonly RealEstateDB _dbContext;

        public FilterService(RealEstateDB context)
        {
            this._dbContext = context;
            this._dbSet = context.Set<TEntity>();
        }

        public void Dispose()
        {
            this._dbContext.Dispose();
        }

        public IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> filterPredicate, Expression<Func<TEntity, dynamic>> selector)
        {
            return  this._dbSet.Where(filterPredicate).Include(selector).ToList();
        }

        public IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> filterPredicate)
        {
            return  this._dbSet.Where(filterPredicate).ToList();
        }

    }
}
