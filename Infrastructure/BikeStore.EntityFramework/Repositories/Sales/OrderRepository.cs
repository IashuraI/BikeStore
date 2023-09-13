using BikeStore.Application.Interfaces;
using BikeStore.Domain.Entities.Sales;
using BikeStore.WebApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace BikeStore.Infrastructure.EntityFramework.Repositories.Sales
{
    internal class OrderRepository : Repository<Order>, IOrderRepository
    {
        public OrderRepository(BikeStoreDbContext dbContext) : base(dbContext)
        {
        }

        public override async Task<Order?> GetAsync(int id)
        {
            return await GetWithIncludes()
                .FirstOrDefaultAsync(x => x.OrderId == id);
        }

        public async Task<List<Order>> GetWithPaginationAsync(int pageSize, int pageNumber, int? customerId = null)
        {
             var query = GetWithIncludes()
                .Take(pageSize).Skip(pageSize * pageNumber - pageSize);

            if(customerId != null)
            {
                query = query.Where(x => x.CustomerId == customerId);
            }

            return await query.AsNoTracking().ToListAsync();
        }

        private IIncludableQueryable<Order, Staff?> GetWithIncludes()
        {
            return _dbSet
                .Include(x => x.Customer)
                .Include(x => x.OrderItems)
                    .ThenInclude(x => x.Product)
                .Include(x => x.Store)
                .Include(x => x.Staff);
        }
    }
}
