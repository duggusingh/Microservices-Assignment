using OrderService.Models;
using OrderService.Data;

namespace OrderService.Services
{
    public class OrderService(HttpClient httpClient, OrderDbContext context, IConfiguration config) : IOrderService
    {
        public async Task<(bool Success, string Message)> PlaceOrderAsync(Order order)
        {
            var baseUrl = config["ProductServiceSettings:BaseUrl"];

            foreach (var item in order.OrderItems)
            {
                try
                {
                    var response = await httpClient.GetAsync($"{baseUrl}/api/products/{item.ProductId}");
                    
                    if (!response.IsSuccessStatusCode)
                        return (false, $"Product ID {item.ProductId} invalid or service unavailable.");

                    var product = await response.Content.ReadFromJsonAsync<ProductDto>();

                    if (product == null || product.StockQty < item.Qty)
                        return (false, $"Insufficient stock for Product ID {item.ProductId}.");
                }
                catch (Exception ex)
                {
                    return (false, $"Error validating product stock: {ex.Message}");
                }
            }

            context.Orders.Add(order);
            await context.SaveChangesAsync();

            return (true, "Order placed successfully.");
        }
    }
}
