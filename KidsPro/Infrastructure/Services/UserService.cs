using Application;
using Application.Services;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Services
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

        public async Task<(string, string?)> LoginAsync(string phonenumber, string password)
        {
            var hashedPass= HashingPass(password);
            var _user = await _unit.UserRepository.GetUserByAttribute(phonenumber, hashedPass, 1);
            if(_user != null)
            {
                var accessToken = _authentication.CreateAccessToken(_user);
                var refeshToken = _authentication.CreateRefreshToken(_user);
                return (accessToken, refeshToken);
            }
            return ("Login Failed", null);
        }
        string HashingPass(string password)
        {
            var hasher= new PasswordHasher<object>();
            return hasher.HashPassword(null, password);
        }
        
}
}
