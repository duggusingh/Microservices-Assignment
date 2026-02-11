using Xunit;
using Moq;
using OrderService.Services;
using OrderService.Models;
using OrderService.Data;
using Microsoft.EntityFrameworkCore;
using Moq.Protected;
using System.Net;

namespace OrderService.Tests
{
    public class OrderServiceTests
    {
        [Fact]
        public async Task PlaceOrder_ShouldFail_WhenStockIsInsufficient()
        {
            // Setup In-Memory Database
            var options = new DbContextOptionsBuilder<OrderDbContext>()
                .UseInMemoryDatabase(databaseName: "DurgeshTestDb_1")
                .Options;
            using var context = new OrderDbContext(options);

            // Mock HTTP Client to return Low Stock
            var handlerMock = new Mock<HttpMessageHandler>();
            handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = JsonContent.Create(new ProductDto(1, "Laptop", 1000, 0)) // 0 Stock
                });
            var httpClient = new HttpClient(handlerMock.Object);
            
            var configMock = new Mock<IConfiguration>();
            configMock.Setup(c => c["ProductServiceSettings:BaseUrl"]).Returns("http://fake-url");

            var service = new OrderService(httpClient, context, configMock.Object);
            
            var order = new Order { OrderItems = new List<OrderItem> { new OrderItem { ProductId = 1, Qty = 1 } } };
            
            var result = await service.PlaceOrderAsync(order);

            Assert.False(result.Success);
        }
    }
}
