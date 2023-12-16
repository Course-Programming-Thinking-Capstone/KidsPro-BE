using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application;
using Application.Configurations;
using Application.ErrorHandlers;
using Application.Services;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly AppConfiguration _appConfiguration;
    private readonly IServiceProvider _serviceProvider;

    private readonly ILogger<AuthenticationService> _logger;

    private readonly IUnitOfWork _unit;
    private readonly TokenValidationParameters _tokenValidation;
    private readonly JwtSecurityTokenHandler _jwtSecurity;

    public AuthenticationService(AppConfiguration appConfiguration, IServiceProvider serviceProvider, ILogger<AuthenticationService> logger, 
        IUnitOfWork unit, TokenValidationParameters tokenValidation, JwtSecurityTokenHandler jwtSecurity)
    {
        _appConfiguration = appConfiguration;
        _serviceProvider = serviceProvider;
        _logger = logger;
        _unit = unit;
        _tokenValidation = tokenValidation;
        _jwtSecurity = jwtSecurity;
    }

    public string CreateAccessToken(User user)
    {
        try
        {
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appConfiguration.Key));
            var credentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Role.Name ?? throw new Exception("Role is empty"))
            };
            var token = new JwtSecurityToken(
                issuer: _appConfiguration.Issuer,
                audience: _appConfiguration.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: credentials
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        catch (Exception e)
        {
            _logger.LogError("Error at CreateAccessToken: {}", e.Message);
            throw;
        }
    }

    public string CreateRefreshToken(User user)
    {
        try
        {
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appConfiguration.Key));
            var credentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Role.Name ?? throw new Exception("Role is empty"))
            };
            var token = new JwtSecurityToken(
                issuer: _appConfiguration.Issuer,
                audience: _appConfiguration.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddDays(3),
                signingCredentials: credentials
            );
            var refeshToken= new JwtSecurityTokenHandler().WriteToken(token);
            // add refeshtoken data to database
            var refesh = new RefeshToken
            {
                Id= Guid.NewGuid(),
                UserId= user.Id,
                Token = refeshToken
            };
            _unit.RefeshTokenRepository.AddAsync(refesh);
            _unit.SaveChangeAsync();

            return refeshToken;
        }
        catch (Exception e)
        {
            _logger.LogError("Error at CreateRefreshToken: {}", e.Message);
            throw;
        }
    }

    public int GetCurrentUserId()
    {
        var httpContextAccessor = _serviceProvider.GetService<IHttpContextAccessor>();

        if (httpContextAccessor != null)
        {
            if (httpContextAccessor.HttpContext?.User.Identity is ClaimsIdentity claimsIdentity &&
                claimsIdentity.Claims.Any() &&
                int.TryParse(claimsIdentity.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value,
                    out var userId))
            {
                return userId;
            }
        }

        throw new NotFoundException();
    }

    public void GetCurrentUserInformation(out int userId, out string role)
    {
        var httpContextAccessor = _serviceProvider.GetService<IHttpContextAccessor>();

        if (httpContextAccessor != null)
        {
            if (httpContextAccessor.HttpContext?.User.Identity is ClaimsIdentity claimsIdentity &&
                claimsIdentity.Claims.Any())
            {
                // Retrieve the user ID claim
                var userIdClaim = claimsIdentity.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

                if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out userId))
                {
                    throw new Exception("User ID claim not found or invalid.");
                }

                // Retrieve the role claim
                var roleClaim = claimsIdentity.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role);

                if (roleClaim == null)
                {
                    throw new Exception("Role claim not found.");
                }

                role = roleClaim.Value;

                return;
            }
        }

        throw new NotFoundException();
    }

    public string RenewToken(string accessToken, User user)
    {
        try
        {
            //Check 1: Accesstoken valid format
            var tokenVerification = _jwtSecurity.ValidateToken(accessToken, _tokenValidation, out var validatedToken);

            //Check 2: Algorithm

        }catch (Exception e)
        {
            _logger.LogError("Error at RenewToken: {}", e.Message);
            throw;
        }
        return null;
    }
}