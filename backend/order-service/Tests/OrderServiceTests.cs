using Xunit;
using Moq;
using Moq.Protected;
using System.Net;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using OrderService.Services;
using OrderService.Models;
using OrderService.Data;
using Microsoft.Extensions.Configuration;

namespace OrderService.Tests
{
    public class OrderServiceTests
    {
        private readonly Mock<HttpMessageHandler> _httpHandlerMock;
        private readonly Mock<IConfiguration> _configMock;
        private readonly OrderDbContext _dbContext;
        private readonly OrderService _service;

        public OrderServiceTests()
        {
            // 1. Setup In-Memory Database for isolation
            var options = new DbContextOptionsBuilder<OrderDbContext>()
                .UseInMemoryDatabase(databaseName: $"DurgeshTestDb_{Guid.NewGuid()}")
                .Options;
            _dbContext = new OrderDbContext(options);

            // 2. Mock Configuration (BaseUrl)
            _configMock = new Mock<IConfiguration>();
            _configMock.Setup(c => c["ProductServiceSettings:BaseUrl"]).Returns("http://test-url");

            // 3. Mock HTTP Handler
            _httpHandlerMock = new Mock<HttpMessageHandler>();
            var httpClient = new HttpClient(_httpHandlerMock.Object);

            // 4. Initialize Service
            _service = new OrderService(httpClient, _dbContext, _configMock.Object);
        }

        // TEST 1: SUCCESS SCENARIO
        [Fact]
        public async Task PlaceOrder_ShouldReturnTrue_WhenStockIsSufficient()
        {
            // Arrange
            var productId = 1;
            var order = new Order
            {
                CustomerName = "Durgesh Test",
                OrderItems = new List<OrderItem> { new OrderItem { ProductId = productId, Qty = 2 } }
            };

            // Mock Product Service Response: Stock is 10 (Sufficient)
            var productResponse = new ProductDto(productId, "Laptop", 1000, 10);
            
