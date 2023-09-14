using BikeStore.Domain.Interfaces;
using BikeStore.Persistence.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BikeStore.Infrastructure.EntityFramework.Repositories
{
    internal abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly BikeStoreDbContext _dbContext;
        protected readonly DbSet<TEntity> _dbSet;

        protected Repository(BikeStoreDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<TEntity>();
        }

        public virtual async Task<List<TEntity>> GetAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _dbSet.Where(predicate).ToListAsync();
        }

        public virtual async Task<TEntity?> GetAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public virtual async Task<TEntity> CreateAsync(TEntity entity)
        {
            _dbSet.Add(entity);
            await _dbContext.SaveChangesAsync();

            return entity;
        }

        public virtual async Task<TEntity> UpdateAsync(TEntity entity)
        {
            _dbSet.Update(entity);
            await _dbContext.SaveChangesAsync();

            return entity;
        }
    }
}
