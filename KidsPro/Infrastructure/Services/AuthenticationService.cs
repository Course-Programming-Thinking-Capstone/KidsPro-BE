using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application;
using Application.Configurations;
using Application.ErrorHandlers;
using Application.Interfaces.IServices;
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

    public AuthenticationService(AppConfiguration appConfiguration, IServiceProvider serviceProvider,
        ILogger<AuthenticationService> logger,
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
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };
            var token = new JwtSecurityToken(
                issuer: _appConfiguration.Issuer,
                audience: _appConfiguration.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddDays(3),
                signingCredentials: credentials
            );
            var refeshToken = new JwtSecurityTokenHandler().WriteToken(token);
            // add refeshtoken data to database
            var refesh = new RefreshToken
            {
                UserId = user.Id,
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

    public Guid GetCurrentUserId()
    {
        var httpContextAccessor = _serviceProvider.GetService<IHttpContextAccessor>();

        if (httpContextAccessor != null)
        {
            if (httpContextAccessor.HttpContext?.User.Identity is ClaimsIdentity claimsIdentity &&
                claimsIdentity.Claims.Any() &&
                Guid.TryParse(claimsIdentity.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value,
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

    public (bool, string, string?) ReissueToken(string accessToken, string refeshToken, User user)
    {
        try
        {
            //Check 1: Accesstoken & Refeshtoken valid format
            var accessTokenVerification =
                _jwtSecurity.ValidateToken(accessToken, _tokenValidation, out var validatedAccessToken)
                ?? throw new NotImplementedException();
            var refeshTokenVerification =
                _jwtSecurity.ValidateToken(refeshToken, _tokenValidation, out var validatedRefeshToken)
                ?? throw new NotImplementedException();

            //Check 2: Algorithm HmacSha512
            if (validatedAccessToken is JwtSecurityToken jwtSecurity)
            {
                var result = jwtSecurity.Header.Alg.Equals(SecurityAlgorithms.HmacSha512,
                    StringComparison.InvariantCultureIgnoreCase);
                if (!result)
                    return (false, "Algorithm Wrong!", null);
            }

            //Check 3: Expire Token. check xem access token có hết hạn chưa
            var _ExpToken = accessTokenVerification.Claims.First(x => x.Type == JwtRegisteredClaimNames.Exp).Value;
            var utcExpireDate = long.Parse(_ExpToken);
            var expireDate = ConverUrnixTimeToDateTime(utcExpireDate);
            if (expireDate > DateTime.UtcNow)
                return (false, "Access token has not yet expired", null);

            //Check 4: Check xem userid của access token có exist trong table refeshtoken 
            var _UserIdToken = accessTokenVerification.Claims.First(x => x.Type == JwtRegisteredClaimNames.NameId)
                .Value;
            var _checkExistance = _unit.RefeshTokenRepository.CheckRefeshTokenExist(_UserIdToken, 1);
            if (!_checkExistance)
                return (false, "UserId does not match", null);

            //Check 5: Check xem refeshtoken gửi từ client có exist trong table refeshtoken 
            _checkExistance = _unit.RefeshTokenRepository.CheckRefeshTokenExist(refeshToken, 2);
            if (!_checkExistance)
                return (false, "RefreshToken does not match", null);

            //Check 6: Check expired refeshtoken 
            _ExpToken = refeshTokenVerification.Claims.First(x => x.Type == JwtRegisteredClaimNames.Exp).Value;
            utcExpireDate = long.Parse(_ExpToken);
            expireDate = ConverUrnixTimeToDateTime(utcExpireDate);
            string _accessToken = accessToken;
            if (expireDate > DateTime.UtcNow)
            {
                // Nếu refesh token còn expire thì cấp lại access token
                _accessToken = CreateAccessToken(user);
                return (true, _accessToken, refeshToken);
            }
            else
            {
                // Nếu refesh token hết expire thì cấp lại access token và refesh token
                _accessToken = CreateAccessToken(user);
                var _refeshToken = CreateRefreshToken(user);
                return (true, _accessToken, _refeshToken);
            }
        }
        catch (Exception e)
        {
            _logger.LogError("Error at ReissueToken: {}", e.Message);
            throw;
        }
    }

    private DateTime ConverUrnixTimeToDateTime(long utcExpireDate)
    {
        var dateTimeInterval = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        dateTimeInterval.AddSeconds(utcExpireDate).ToUniversalTime();
        return dateTimeInterval;
    }
}