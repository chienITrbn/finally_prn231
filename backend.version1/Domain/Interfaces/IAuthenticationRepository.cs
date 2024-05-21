using backend.Domain.Interfaces;
using backend.version1.Domain.Entities;

namespace backend.version1.Domain.Interfaces
{
    public interface IAuthenticationRepository : IRepositoryBase<User>
    {
        Task<User> LoginAsync(string username, string password);

        Task<User> LogoutAsync(string username, string password);

        Task<User> RegisterAsync(User user, string password);

        Task<User> UnregisterAsync(string username, string password);

        Task<User> UpdateRefreshTokenAsync(User user, string refreshToken, DateTime expiryTime);
    }
}