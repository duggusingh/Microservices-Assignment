using OrderService.Models;

namespace OrderService.Services
{
    public interface IOrderService
    {
        Task<(bool Success, string Message)> PlaceOrderAsync(Order order);
    }
}
