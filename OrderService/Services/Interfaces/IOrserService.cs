// Services/IOrderService.cs
namespace OrderService.Services
{
    public interface IOrderService
    {
        Task<IEnumerable<DTOs.OrderDto>> GetAllOrdersAsync();
        Task<DTOs.OrderDto> GetOrderByIdAsync(int id);
        Task<DTOs.OrderDto> CreateOrderAsync(DTOs.CreateOrderDto createOrderDto, int userId);
    }
}