using _3aqarak.BLL.Interfaces;
using _3aqarak.DAL.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace _3aqarak.DAL.Repositories
{
    public class GenericRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly DbSet<TEntity> _dbSet;
        private readonly RealEstateDB _dbContext;

        public GenericRepository(RealEstateDB context)
        {
            this._dbContext = context;
            this._dbSet = context.Set<TEntity>();
        }

        public void Add(TEntity entity)
        {

            this._dbSet.Add(entity);
        }

        public async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> filterPredicate)
        {

            return await this._dbSet.Where(filterPredicate).ToListAsync();
        }

        public IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> filterPredicate)
        {

            return  this._dbSet.Where(filterPredicate).ToList();

        }

        public async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> filterPredicate, Expression<Func<TEntity, dynamic>> selector)
        {
           
            return await this._dbSet.Where(filterPredicate).Include(selector).ToListAsync(); 

        }

        public async Task<TEntity> GetAsync(int id)
        {
            return await this._dbSet.FindAsync(id);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await this._dbSet.ToListAsync();
        }

        public async Task<IEnumerable<TEntity>> GetALLByPageAsync(int pageNumber, int pageSize,
            OrderType orderType, Expression<Func<TEntity, bool>> orderPredicate)
        {
            IEnumerable<TEntity> result = new List<TEntity>();

            switch (orderType)
            {
                case OrderType.Ascending:
                    result = await this._dbSet.OrderBy(orderPredicate)
                        .Skip((pageNumber - 1) * pageSize)
                        .Take(pageSize)
                        .ToListAsync();
                    break;
                case OrderType.Descending:
                    result = await this._dbSet.OrderByDescending(orderPredicate)
                        .Skip((pageNumber - 1) * pageSize)
                        .Take(pageSize)
                        .ToListAsync();
                    break;
                default:
                    result = await this._dbSet.OrderBy(orderPredicate)
                        .Skip((pageNumber - 1) * pageSize)
                        .Take(pageSize)
                        .ToListAsync();
                    break;
            }

            return result;
        }

        public async Task<TEntity> ReloadAsync(TEntity entity)
        {
            await this._dbContext.Entry(entity).ReloadAsync();
            return entity;
        }

        public void Remove(TEntity entity)
        {
            this._dbSet.Remove(entity);
        }

        public void Update(TEntity entity)
        {
            this._dbContext.Entry(entity).State = EntityState.Modified;
        }

        public void Dispose()
        {
            this._dbContext.Dispose();
        }




    }
}
