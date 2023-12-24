﻿using Application.Dtos.Request.Authentication;
using Application.Dtos.Response.User;
using Domain.Entities;

namespace Application.Interfaces.Services
{
    public interface IUserService
    {
        Task<(bool, string, string?)> LoginAsync(string phonenumber, string password);
        Task<User> GetUserById(Guid id);
        (bool, string, string?) ReissueToken(string accessToken, string refeshToken, User user);

        Task<(LoginUserDto, string?, string?)> RegisterAsync(RegisterDto request);
    }
}