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
    private TokenValidationParameters _tokenValidation;
    private JwtSecurityTokenHandler _jwtSecurity;

    public AuthenticationService(AppConfiguration appConfiguration,
        IServiceProvider serviceProvider, ILogger<AuthenticationService> logger,
        IUnitOfWork unit, TokenValidationParameters tokenValidation, JwtSecurityTokenHandler jwtSecurity)
    {
        _appConfiguration = appConfiguration;
        _serviceProvider = serviceProvider;
        _logger = logger;
        _unit = unit;
        _tokenValidation = tokenValidation;
        _jwtSecurity = jwtSecurity;
    }

    public string CreateAccessToken(Account account)
    {
        try
        {
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appConfiguration.Key));
            var credentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, account.Id.ToString()),
                new Claim(ClaimTypes.Role, account.Role.Name ?? throw new Exception("Role is empty"))
            };
            var token = new JwtSecurityToken(
                issuer: _appConfiguration.Issuer,
                audience: _appConfiguration.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddDays(7),
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

    public string CreateRefreshToken(Account account)
    {
        try
        {
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appConfiguration.Key));
            var credentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, account.Id.ToString())
            };
            var token = new JwtSecurityToken(
                issuer: _appConfiguration.Issuer,
                audience: _appConfiguration.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddDays(14),
                signingCredentials: credentials
            );
            var refreshToken = new JwtSecurityTokenHandler().WriteToken(token);
            // add refeshtoken data to database
            // var refesh = new RefreshToken
            // {
            //     UserId = user.Id,
            //     Token = refeshToken
            // };
            // _unit.RefeshTokenRepository.AddAsync(refesh);
            // _unit.SaveChangeAsync();

            return refreshToken;
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

        throw new UnauthorizedException("Invalid token.");
    }

    // public async Task<(bool, string, string?)> ReissueToken(string accessToken, string refeshToken, int id)
    // {
    //     _tokenValidation = new TokenValidationParameters
    //     {
    //         ValidateIssuer = true,
    //         ValidateAudience = true,
    //         ValidateIssuerSigningKey = true,
    //         ValidIssuer = _appConfiguration.Issuer,
    //         ValidAudience = _appConfiguration.Audience,
    //         ValidateLifetime = true,
    //         IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appConfiguration.Key)),
    //         ClockSkew = TimeSpan.Zero
    //     };
    //     try
    //     {
    //         //Check 1: Accesstoken & Refeshtoken valid format
    //         var accessTokenVerification =
    //             _jwtSecurity.ValidateToken(accessToken, _tokenValidation, out var validatedAccessToken)
    //             ?? throw new NotImplementException("Access Token wrong format");
    //         var refeshTokenVerification =
    //             _jwtSecurity.ValidateToken(refeshToken, _tokenValidation, out var validatedRefeshToken)
    //             ?? throw new NotImplementException("Refesh Token wrong format");
    //
    //         //Check 2: Algorithm HmacSha512
    //         if (validatedAccessToken is JwtSecurityToken jwtSecurity)
    //         {
    //             var result = jwtSecurity.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
    //                 StringComparison.InvariantCultureIgnoreCase);
    //             if (!result)
    //                 throw new NotImplementException("Access token wrong algorithms");
    //         }
    //
    //         //Check 3: Expire Token. check xem access token có hết hạn chưa
    //         var _ExpToken = accessTokenVerification.Claims.First(x => x.Type == JwtRegisteredClaimNames.Exp).Value;
    //         var utcExpireDate = long.Parse(_ExpToken);
    //         var expireDate = ConverUrnixTimeToDateTime(utcExpireDate);
    //         if (expireDate > DateTime.UtcNow)
    //             throw new NotImplementException("Access token has not yet expired");
    //
    //         //Check 4: Check xem userid của access token có exist trong table refeshtoken 
    //         var _UserIdToken = accessTokenVerification.Claims.First(x => x.Type == ClaimTypes.NameIdentifier)
    //             .Value;
    //         var _checkExistance = _unit.RefeshTokenRepository.CheckRefeshTokenExist(_UserIdToken, 1);
    //         if (!_checkExistance)
    //             throw new NotFoundException("UserId not match");
    //
    //         //Check 5: Check xem refeshtoken gửi từ client có exist trong table refeshtoken 
    //         _checkExistance = _unit.RefeshTokenRepository.CheckRefeshTokenExist(refeshToken, 2);
    //         if (!_checkExistance)
    //             throw new NotFoundException("RefreshToken does not match");
    //
    //         //Check 6: Check expired refeshtoken 
    //         _ExpToken = refeshTokenVerification.Claims.First(x => x.Type == JwtRegisteredClaimNames.Exp).Value;
    //         utcExpireDate = long.Parse(_ExpToken);
    //         expireDate = ConverUrnixTimeToDateTime(utcExpireDate);
    //         string _accessToken = accessToken;
    //         //Get user by id
    //         var user =await _unit.UserRepository.GetByIdAsync(id);
    //         if (expireDate > DateTime.UtcNow)
    //         {
    //             // Nếu refesh token còn expire thì cấp lại access token
    //             _accessToken = CreateAccessToken(user);
    //             return (true, _accessToken, refeshToken);
    //         }
    //         else
    //         {
    //             throw new NotImplementException("RefreshToken has expired, please login again");
    //         }
    //     }
    //     catch (Exception e)
    //     {
    //         throw new BadRequestException("Reissue token fail");
    //     }
    // }
    //
    // private DateTime ConverUrnixTimeToDateTime(long utcExpireDate)
    // {
    //     var dateTimeInterval = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
    //     dateTimeInterval = dateTimeInterval.AddSeconds(utcExpireDate).ToUniversalTime();
    //     return dateTimeInterval;
    // }
}