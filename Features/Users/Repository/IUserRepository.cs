using ecommerse_api.Features.Users.Models;

namespace ecommerse_api.Features.Users.Repository
{
    public interface IUserRepository
    {

        Task<User> GetByIdAsync(Guid id);
        Task CreateAsync(User user, CancellationToken cancellationToken );
        Task<bool> UserExistAsync(Guid id);
        Task<bool>IsEmailUniqueAsync(string email);

        

    }
}
