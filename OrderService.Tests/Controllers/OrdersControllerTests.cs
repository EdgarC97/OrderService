using Moq;
using OrderService.Controllers;
using OrderService.Data;
using OrderService.DTOs;
using OrderService.Models;
using OrderService.Services;
using Microsoft.AspNetCore.Mvc;
using Xunit;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace OrderService.Tests.Controllers
{
    public class OrdersControllerTests : TestBase
    {
        [Fact]
        public void GetAllOrders_AuthorizedUser_ReturnsOkWithOrders()
        {
            // Arrange
            var mockService = new Mock<IOrderService>();
            mockService.Setup(s => s.GetAllOrdersAsync()).Returns(Task.FromResult(new List<OrderDto>
            {
                new OrderDto { Id = 1, OrderNumber = "ORD-001", CustomerName = "John Doe" }
            } as IEnumerable<OrderDto>));

            var controller = new OrdersController(mockService.Object);
            SetupAuthenticatedUser(controller);

            // Act
            var result = controller.GetAllOrders().Result;

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var orders = Assert.IsType<List<OrderDto>>(okResult.Value);
            Assert.Single(orders);
        }

        [Fact]
        public void GetOrderById_ExistingId_ReturnsOkWithOrder()
        {
            // Arrange
            var mockService = new Mock<IOrderService>();
            mockService.Setup(s => s.GetOrderByIdAsync(1)).Returns(Task.FromResult(new OrderDto { Id = 1, OrderNumber = "ORD-001", CustomerName = "John Doe" }));

            var controller = new OrdersController(mockService.Object);
            SetupAuthenticatedUser(controller);

            // Act
            var result = controller.GetOrderById(1).Result;

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var order = Assert.IsType<OrderDto>(okResult.Value);
            Assert.Equal(1, order.Id);
        }

        [Fact]
        public void GetOrderById_NonExistingId_ReturnsNotFound()
        {
            // Arrange
            var mockService = new Mock<IOrderService>();
            mockService.Setup(s => s.GetOrderByIdAsync(999)).Returns(Task.FromResult<OrderDto?>(null));

            var controller = new OrdersController(mockService.Object);
            SetupAuthenticatedUser(controller);

            // Act
            var result = controller.GetOrderById(999).Result;

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void CreateOrder_ValidDto_ReturnsCreatedWithOrder()
        {
            // Arrange
            var mockService = new Mock<IOrderService>();
            var orderDto = new OrderDto { Id = 1, OrderNumber = "ORD-001", CustomerName = "John Doe" };
            mockService.Setup(s => s.CreateOrderAsync(It.IsAny<CreateOrderDto>(), It.IsAny<int>())).Returns(Task.FromResult(orderDto));

            var controller = new OrdersController(mockService.Object);
            SetupAuthenticatedUser(controller);
            var createOrderDto = new CreateOrderDto { OrderNumber = "ORD-001", CustomerName = "John Doe", TotalAmount = 100.00M };

            // Act
            var result = controller.CreateOrder(createOrderDto).Result;

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var returnedOrder = Assert.IsType<OrderDto>(createdResult.Value);
            Assert.Equal("ORD-001", returnedOrder.OrderNumber);
        }

        private void SetupAuthenticatedUser(ControllerBase controller)
        {
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, "1")
            }, "mock"));
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };
        }
    }
}