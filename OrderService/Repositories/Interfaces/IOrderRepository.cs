// Repositories/IOrderRepository.cs
namespace OrderService.Repositories
{
    public interface IOrderRepository
    {
        Task<IEnumerable<Models.Order>> GetAllAsync();
        Task<Models.Order> GetByIdAsync(int id);
        Task AddAsync(Models.Order order);
    }
}

