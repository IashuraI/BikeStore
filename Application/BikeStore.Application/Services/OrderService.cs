using BikeStore.Application.Interfaces;
using BikeStore.Domain.Entities.Sales;

namespace BikeStore.Application.Services
{
    public class OrderService
    {
        private readonly IOrderRepository _orderRepository;

        public OrderService(IOrderRepository repository) 
        {
            _orderRepository = repository;
        }

        public async Task<Order?> GetAsyncById(int id)
        {
            return await _orderRepository.GetAsync(id);
        }

        public async Task<List<Order>> GetWithPaginationAsync(int pageSize, int pageNumber, int? customerId = null)
        {
            return await _orderRepository.GetWithPaginationAsync(pageSize, pageNumber, customerId);
        }

        public async Task<Order> CreateAsync(Order order)
        {
            return await _orderRepository.CreateAsync(order);
        }

        public async Task<int> CancelAsync(int orderId)
        {
            Order? order = await _orderRepository.GetAsync(orderId);

            if (order != null)
            {
                order.OrderStatus = OrderStatus.Rejected;
                await _orderRepository.UpdateAsync(order);

                return orderId;
            }
            else
            {
                return 0;
            }
        }
    }
}
