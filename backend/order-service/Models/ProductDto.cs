namespace OrderService.Models
{
    public record ProductDto(int ProductId, string Name, decimal Price, int StockQty);
}
