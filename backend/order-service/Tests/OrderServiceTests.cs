using Moq;
using Moq.Protected;
using System.Net;
using System.Net.Http.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using OrderService.Models;
using OrderService.Data;
using OrderService.Services;
using Xunit;

namespace OrderService.Tests
{
    public class OrderServiceTests
    {
        private readonly Mock<HttpMessageHandler> _msgHandlerMock;
        private readonly OrderDbContext _dbContext;
        private readonly IConfiguration _config;

        public OrderServiceTests()
        {
            _msgHandlerMock = new Mock<HttpMessageHandler>();
            
            // In-Memory Database for Testing
            var options = new DbContextOptionsBuilder<OrderDbContext>()
                .UseInMemoryDatabase(databaseName: "DurgeshOrderTestDb")
                .Options;
            _dbContext = new OrderDbContext(options);

            // Mock Configuration
            var inMemoryConfig = new Dictionary<string, string> {
                {"ProductServiceSettings:BaseUrl", "http://localhost:8081"}
            };
            _config = new ConfigurationBuilder().AddInMemoryCollection(inMemoryConfig).Build();
        }

        [Fact]
        public async Task PlaceOrder_ShouldReturnTrue_WhenStockIsSufficient()
        {
            // Arrange
            var productId = 1;
            var productDto = new { ProductId = productId, StockQty = 10 };
            
            SetupMockHttpMessage(HttpStatusCode.OK, productDto);

            var httpClient = new HttpClient(_msgHandlerMock.Object);
            var service = new OrderService.Services.OrderService(httpClient, _dbContext, _config);
            
            var order = new Order { 
                CustomerName = "Durgesh", 
                OrderItems = new List<OrderItem> { new OrderItem { ProductId = productId, Qty = 2 } } 
            };

            // Act
            var result = await service.PlaceOrderAsync(order);

            // Assert
            Assert.True(result.Success);
            Assert.Equal("Order placed successfully.", result.Message);
        }

        [Fact]
        public async Task PlaceOrder_ShouldReturnFalse_WhenStockIsInsufficient()
        {
            // Arrange
            var productId = 2;
            var productDto = new { ProductId = productId, StockQty = 1 }; // Only 1 in stock
            
            SetupMockHttpMessage(HttpStatusCode.OK, productDto);

            var httpClient = new HttpClient(_msgHandlerMock.Object);
            var service = new OrderService.Services.OrderService(httpClient, _dbContext, _config);
            
            var order = new Order { 
                CustomerName = "Durgesh", 
                OrderItems = new List<OrderItem> { new OrderItem { ProductId = productId, Qty = 5 } } // Ordering 5
            };

            // Act
            var result = await service.PlaceOrderAsync(order);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Insufficient stock", result.Message);
        }

        [Fact]
        public async Task PlaceOrder_ShouldReturnFalse_WhenProductDoesNotExist()
        {
            // Arrange
            SetupMockHttpMessage(HttpStatusCode.NotFound, null);

            var httpClient = new HttpClient(_msgHandlerMock.Object);
            var service = new OrderService.Services.OrderService(httpClient, _dbContext, _config);
            
            var order = new Order { 
                CustomerName = "Durgesh", 
                OrderItems = new List<OrderItem> { new OrderItem { ProductId = 999, Qty = 1 } } 
            };

            // Act
            var result = await service.PlaceOrderAsync(order);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Product invalid or service unavailable", result.Message);
        }

        private void SetupMockHttpMessage(HttpStatusCode status, object content)
        {
            _msgHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = status,
                    Content = content != null ? JsonContent.Create(content) : null
                });
        }
    }
}
