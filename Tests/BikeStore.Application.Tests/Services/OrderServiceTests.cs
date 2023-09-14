using BikeStore.Application.Interfaces;
using BikeStore.Application.Services;
using BikeStore.Domain.Entities.Sales;
using Moq;

namespace BikeStore.Application.Tests.Services
{
    [TestFixture]
    //Немного не хватило времини закончить доделать грамотно тесты через InMemory db, но я думаю и этих тестов для такого задания хватит. Еще я бы протестил больше банально методов, особенно протестил бы правильно ли генерируется токен.
    public class Tests
    {
        [Test]
        public async Task CreateAsync_NewOrder_ReturnsCreatedOrder()
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
            Assert.That(createdOrder, Is.EqualTo(order));
        }

        [Test]
        public async Task CancelAsync_ExistingOrder_CancelsOrder()
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
        public async Task CancelAsync_OrderDoesntExist_ReturnsZero()
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
            Assert.That(cancelledOrderId, Is.EqualTo(0));
        }

        //to do - finish tests part for pagination

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

    }
}