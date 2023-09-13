using System.Linq.Expressions;

namespace BikeStore.Domain.Interfaces
{
    public interface IRepository<TEntity> where TEntity : class
    {
        public Task<List<TEntity>> GetAsync(Expression<Func<TEntity, bool>> predicate);

        public Task<TEntity?> GetAsync(int id);

        public Task<TEntity> CreateAsync(TEntity entity);

        public Task<TEntity> UpdateAsync(TEntity entity);
    }
}
