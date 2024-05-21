using backend.Infrastructure.Repositories;
using backend.version1.Domain.Entities;
using backend.version1.Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Identity;

public class AuthenticationRepository : BaseRepository<User>, IAuthenticationRepository
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;

    public AuthenticationRepository(ApplicationDbContext applicationDbContext, UserManager<User> userManager, SignInManager<User> signInManager)
        : base(applicationDbContext)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    public async Task<User> LoginAsync(string username, string password)
    {
        var user = await _userManager.FindByNameAsync(username);
        if (user == null)
        {
            return null;
        }

        var signInResult = await _signInManager.CheckPasswordSignInAsync(user, password, false);
        if (signInResult.Succeeded)
        {
            return user;
        }

        return null;
    }

    public Task<User> LogoutAsync(string username, string password)
    {
        throw new NotImplementedException();
    }

    public async Task<User> RegisterAsync(User user, string password)
    {
        var existingUser = await _userManager.FindByNameAsync(user.UserName);
        if (existingUser != null)
        {
            throw new ArgumentException($"User with username {user.UserName} already exists");
        }

        var existingEmail = await _userManager.FindByEmailAsync(user.Email);
        if (existingEmail != null)
        {
            throw new ArgumentException($"User with email {user.Email} already exists");
        }

        var result = await _userManager.CreateAsync(user, password);
        if (!result.Succeeded)
        {
            throw new Exception("Failed to register user");
        }

        return user;
    }

    public Task<User> UnregisterAsync(string username, string password)
    {
        throw new NotImplementedException();
    }

    public async Task<User> UpdateRefreshTokenAsync(User user, string refreshToken, DateTime expiryTime)
    {
        if (user.RefreshTokenExpiryTime <= DateTime.Now)
        {
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = expiryTime;

            await UpdateAsync(user);
        }

        return user;
    }
}