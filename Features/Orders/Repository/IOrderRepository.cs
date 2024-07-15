using ecommerse_api.Features.Orders.Models;

namespace ecommerse_api.Features.Orders.Repository
{
    public interface IOrderRepository
    {
        Task<Guid> GetOrCreatePendingOrderIdAync(Guid userId);
        Task<List<Order>> GetAllStatusOrderAsync(string status);
        Task<List<Order>> GetAllOrderAsync();
        Task<Order> GetByIdAsync(Guid id);
        Task DeleteAsync(Guid id);
        Task UpdateAsync(Order order);
        Task<List<Order>> GetOrdersByUserAsync(Guid userId);
    }
}
