using ecommerse_api.Features.CartItems.Models;

namespace ecommerse_api.Features.CartItems.Repository
{
    public interface ICartItemRepository
    {

        Task<CartItem?> GetByIdAsync(Guid id);
        Task CreateAysnc(CartItem cartItem);
        Task UpdateAynsc(CartItem cartItem);
        Task <CartItem?> DeleteAysnc(Guid id);

        Task<IEnumerable<CartItem>> GetAllPendingAsync();
        Task<IEnumerable<CartItem>> GetAllByOrderAsync(Guid orderId);


    }
}
