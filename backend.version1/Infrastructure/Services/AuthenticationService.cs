using backend.version1.Domain.AutoMapper;
using backend.version1.Domain.DTO;
using backend.version1.Domain.Entities;
using backend.version1.Domain.Interfaces;
using backend.version1.Infrastructure.Services.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace backend.version1.Infrastructure.Services
{
    public class AuthenticationService : BaseService<User, UserDto>, IAuthenticationService
    {
        private readonly IAuthenticationRepository _authenticationRepository;
        private readonly IConfiguration _configuration;

        public AuthenticationService(IAuthenticationRepository authenticationRepository, IAutoMapperBase<User, UserDto> mapper, IConfiguration configuration) : base(authenticationRepository, mapper)
        {
            _authenticationRepository = authenticationRepository;
            _configuration = configuration;
        }

        public async Task<object> Login(LoginRequest request)
        {
            var user = await _authenticationRepository.LoginAsync(request.Username, request.Password);
            if (user is null)
            {
                throw new ArgumentException($"Unable to authenticate user {request.Username}");
            }

            var authClaims = new List<Claim>
            {
                new(ClaimTypes.Name, user.UserName),
                new(ClaimTypes.Email, user.Email),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            var token = CreateToken(authClaims);
            if (user.RefreshTokenExpiryTime <= DateTime.Now)
            {
                var refreshToken = GenerateRefreshToken();
                _ = int.TryParse(_configuration["JWT:RefreshTokenValidityInDays"], out int refreshTokenValidityInDays);

                await _authenticationRepository.UpdateRefreshTokenAsync(user, refreshToken, DateTime.Now.AddDays(refreshTokenValidityInDays));
            }

            return new
            {
                Data = _mapper.Map(user),
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = token.ValidTo
            };
        }

        public async Task<UserDto> Register(RegisterRequest request)
        {
            var user = new User
            {
                Email = request.Email,
                UserName = request.UserName,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            var registeredUser = await _authenticationRepository.RegisterAsync(user, request.Password);

            if (registeredUser == null)
            {
                throw new Exception("There was an error registering the user");
            }

            var userDto = new UserDto
            {
                Email = registeredUser.Email,
                Username = registeredUser.UserName
            };

            return userDto;
        }

        private JwtSecurityToken GetToken(IEnumerable<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var tokenValidityInMinutes = _configuration.GetValue<int>("JWT:TokenValidityInMinutes");
            var expires = DateTime.UtcNow.AddMinutes(tokenValidityInMinutes);

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: expires,
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256));

            return token;
        }

        private JwtSecurityToken CreateToken(List<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
            _ = int.TryParse(_configuration["JWT:TokenValidityInMinutes"], out int tokenValidityInMinutes);

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddMinutes(tokenValidityInMinutes),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            return token;
        }

        private static string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        private ClaimsPrincipal? GetPrincipalFromExpiredToken(string? token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"])),
                ValidateLifetime = false
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
            if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");

            return principal;
        }

        private string GetErrorsText(IEnumerable<IdentityError> errors)
        {
            return string.Join(", ", errors.Select(error => error.Description).ToArray());
        }
    }
}