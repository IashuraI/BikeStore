using BikeStore.Application.Interfaces;
using BikeStore.Application.Services;
using BikeStore.Domain.Entities.Sales;
using Moq;

namespace BikeStore.Application.Tests.Services
{
    [TestFixture]
    public class Tests
    {
        //[Test]
        //public void GetWithPaginationAsync_TenElementsPageOne_ReturnsTenElementsFromPageOne()
        //{
        //    //Arrange
        //    Order createdOrder = new Order { };
        //    var mockRepo = new Mock<IOrderRepository>();
        //    mockRepo
        //        .Setup(repo => repo.CreateAsync(new Order { }))
        //        .ReturnsAsync(new Order { });

        //    var mockOrderService = new OrderService(mockRepo.Object);

        //    // Act
        //    var order = mockOrderService.GetWithPaginationAsync();

        //    // Assert
        //    mockRepo.Verify(r => r.CreateAsync(new Order { }));
        //    Assert.Equals(createdOrder, order);
        //}

        //[Test]
        //public void GetWithPaginationAsync_TenElementsPageTwo_ReturnsTenElementsFromPageTwo()
        //{
        //    //Arrange
        //    Order createdOrder = new Order { };
        //    var mockRepo = new Mock<IOrderRepository>();
        //    mockRepo
        //        .Setup(repo => repo.CreateAsync(new Order { }))
        //        .ReturnsAsync(new Order { });

        //    var mockOrderService = new OrderService(mockRepo.Object);

        //    // Act
        //    var order = mockOrderService.GetWithPaginationAsync();

        //    // Assert
        //    mockRepo.Verify(r => r.CreateAsync(new Order { }));
        //    Assert.Equals(createdOrder, order);
        //}

        //[Test]
        //public void GetWithPaginationAsync_CustomerIdTwo_ReturnsOrdersOnlyWithCustomerIdEqualToTwo()
        //{
        //    //Arrange
        //    Order createdOrder = new Order { };
        //    var mockRepo = new Mock<IOrderRepository>();
        //    mockRepo
        //        .Setup(repo => repo.CreateAsync(new Order { }))
        //        .ReturnsAsync(new Order { });

        //    var mockOrderService = new OrderService(mockRepo.Object);

        //    // Act
        //    var order = mockOrderService.GetWithPaginationAsync();

        //    // Assert
        //    mockRepo.Verify(r => r.CreateAsync(new Order { }));
        //    Assert.Equals(createdOrder, order);
        //}

        [Test]
        public async Task CreateAsync_Order_ReturnsCreatedOrder()
        {
            //Arrange
            Order createdOrder = new() {
                OrderId = 1,
                OrderDate = new DateTime(2022, 10, 1),
                CustomerId = 1,
                OrderStatus = OrderStatus.Pending
            };
            var mockRepo = new Mock<IOrderRepository>();
            mockRepo
                .Setup(repo => repo.CreateAsync(createdOrder))
                .ReturnsAsync(createdOrder);

            var mockOrderService = new OrderService(mockRepo.Object);

            // Act
            var order = await mockOrderService.CreateAsync(createdOrder);

            // Assert
            mockRepo.Verify(r => r.CreateAsync(createdOrder));
            Assert.Equals(createdOrder, order);
        }

        [Test]
        public async Task CancelAsync_Order_CancelsOrder()
        {
            //Arrange
            Order order = new()
            {
                OrderId = 1,
                OrderDate = new DateTime(2022, 10, 1),
                CustomerId = 1,
                OrderStatus = OrderStatus.Pending
            };
            var mockRepo = new Mock<IOrderRepository>();
            mockRepo
                .Setup(repo => repo.GetAsync(order.OrderId))
                .ReturnsAsync(order);

            var mockOrderService = new OrderService(mockRepo.Object);

            // Act
            var cancelledOrderId = await mockOrderService.CancelAsync(order.OrderId);

            // Assert
            Assert.That(cancelledOrderId, Is.EqualTo(order.OrderId));
        }

        [Test]
        public async Task CancelAsync_OrderDoesntExist_CancelsOrder()
        {
            //Arrange
            Order order = new()
            {
                OrderId = 2,
                OrderDate = new DateTime(2022, 10, 1),
                CustomerId = 1,
                OrderStatus = OrderStatus.Pending
            };
            var mockRepo = new Mock<IOrderRepository>();
            mockRepo
                .Setup(repo => repo.GetAsync(order.OrderId))
                .ReturnsAsync(order);

            var mockOrderService = new OrderService(mockRepo.Object);

            // Act
            var cancelledOrderId = await mockOrderService.CancelAsync(1);

            // Assert
            Assert.Equals(order.OrderId, cancelledOrderId);
        }
    }
}