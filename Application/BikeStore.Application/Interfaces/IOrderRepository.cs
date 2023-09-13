using BikeStore.Domain.Entities.Sales;
using BikeStore.Domain.Interfaces;

namespace BikeStore.Application.Interfaces
{
    public interface IOrderRepository : IRepository<Order>
    {
        public Task<List<Order>> GetWithPaginationAsync(int pageSize, int pageNumber, int? customerId = null);
    }
}
