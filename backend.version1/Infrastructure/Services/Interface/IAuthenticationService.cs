using backend.version1.Domain.DTO;
using backend.version1.Domain.Entities;

namespace backend.version1.Infrastructure.Services.Interface
{
    public interface IAuthenticationService : IBaseService<User, UserDto>
    {
        Task<UserDto> Register(RegisterRequest request);

        Task<object> Login(LoginRequest request);
    }
}