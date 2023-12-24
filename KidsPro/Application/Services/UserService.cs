using Application.Configurations;
using Application.Dtos.Request.Authentication;
using Application.Dtos.Response.User;
using Application.ErrorHandlers;
using Application.Interfaces.Services;
using Application.Mappers;
using Domain.Entities;
using Domain.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.Services
{
    public class UserService : IUserService
    {
        readonly IUnitOfWork _unit;
        readonly IAuthenticationService _authentication;

        public UserService(IUnitOfWork unit, IAuthenticationService authentication)
        {
            _unit = unit;
            _authentication = authentication;
        }

        public Task<User> GetUserById(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<(bool, string, string?)> LoginAsync(string phonenumber, string password)
        {
            var hashedPass = HashingPass(password);
            var _user = await _unit.UserRepository.GetUserByAttribute(phonenumber, hashedPass, 1);
            if (_user != null)
            {
                var accessToken = _authentication.CreateAccessToken(_user);
                var refeshToken = _authentication.CreateRefreshToken(_user);
                return (true, accessToken, refeshToken);
            }

            return (false, "Login Failed", null);
        }

        public (bool, string, string?) ReissueToken(string accessToken, string refeshToken, User user)
        {
            var result = _authentication.ReissueToken(accessToken, refeshToken, user);
            return result;
        }

        public async Task<(LoginUserDto, string, string)> RegisterAsync(RegisterDto request)
        {
            //check duplicate phone number
            var isExist = await _unit.UserRepository.GetAll()
                .FirstOrDefaultAsync(u => u.PhoneNumber == request.PhoneNumber)
                .ContinueWith(t => t.Result != null);

            if (isExist)
            {
                throw new ConflictException("Phone number has been existed.");
            }

            //check confirm password
            if (request.Password != request.ConfirmPassword)
            {
                throw new BadRequestException("Confirm password does not match.");
            }

            var userRole = await _unit.RoleRepository.GetAll()
                .FirstOrDefaultAsync(r => r.Name == Constant.USER_ROLE)
                .ContinueWith(t => t.Result ?? throw new NotFoundException("User role not found on database."));

            var entity = new User()
            {
                PhoneNumber = request.PhoneNumber,
                FullName = request.FullName,
                Gender = request.Gender,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                RoleId = userRole.Id,
                Role = userRole,
                Status = UserStatus.Active,
                IsDelete = false,
            };
            await _unit.UserRepository.AddAsync(entity);
            var result = await _unit.SaveChangeAsync();

            if (result > 0)
            {
                var returnDto = UserMapper.EntityToLoginUserDto(entity);
                var accessToken = _authentication.CreateAccessToken(entity);
                var refreshToken = _authentication.CreateRefreshToken(entity);
                return (returnDto, accessToken, refreshToken);
            }

            throw new BadRequestException("Error when adding user to database.");
        }

        string HashingPass(string password)
        {
            var hasher = new PasswordHasher<object>();
            return hasher.HashPassword(null, password);
        }
    }
}