using Application.Configurations;
using Application.Dtos.Request.Authentication;
using Application.Dtos.Response.User;
using Application.ErrorHandlers;
using Application.Interfaces.IServices;
using Application.Mappers;
using Domain.Entities;
using Domain.Enums;

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

        public async Task<LoginUserDto> LoginAsync(string phonenumber, string password)
        {
            var user = await _unit.UserRepository.GetAsync(
                    filter: u => u.PhoneNumber == phonenumber && u.IsDelete == false && u.Status == UserStatus.Active,
                    orderBy: null,
                    includeProperties: $"{nameof(User.Role)}",
                    disableTracking: true)
                .ContinueWith(t =>
                    t.Result.FirstOrDefault() ?? throw new NotFoundException("Account does not exist or being block."));

            if (BCrypt.Net.BCrypt.EnhancedVerify(password, user.PasswordHash))
            {
                
                var result = UserMapper.EntityToLoginUserDto(user);
                result.AccessToken = _authentication.CreateAccessToken(user);
                result.RefreshToken = _authentication.CreateRefreshToken(user);
                
                return result;
            }

            throw new BadRequestException("Incorrect password");
        }
        //Cấp lại accesstoken qua refeshtoken
        public async Task<(bool, string, string?)> ReissueToken(string accessToken, string refeshToken, int id)
        {
            var result =await _authentication.ReissueToken(accessToken, refeshToken, id);
            return result;
        }

        public async Task<LoginUserDto> RegisterAsync(RegisterDto request,int number)
        {
            //check duplicate phone number
            var isExist = await _unit.UserRepository.GetAsync(
                    filter: u => u.PhoneNumber == request.PhoneNumber,
                    orderBy: null
                )
                .ContinueWith(t => t.Result.Any());

            if (isExist)
            {
                throw new ConflictException("Phone number has been existed.");
            }

            var userRole = await _unit.RoleRepository.GetRoleAsync(number);

            var entity = new User()
            {
                PhoneNumber = request.PhoneNumber,
                FullName = request.FullName,
                PasswordHash = BCrypt.Net.BCrypt.EnhancedHashPassword(request.Password),
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
                returnDto.AccessToken = _authentication.CreateAccessToken(entity);
                returnDto.RefreshToken = _authentication.CreateRefreshToken(entity);
                return returnDto;
            }

            throw new BadRequestException("Error when adding user to database.");
        }

        public async Task<List<User>> GetAllUsersByRole(int role)
        {
            if (role <1 || role >4) throw new ForbiddenException("Access Unacceptable");
            return await _unit.UserRepository.GetAllUsersByRole(role);
        }

        public async Task<bool> SwitchStatusUser(int id, int number)
        {
            var user = await _unit.UserRepository.GetByIdAsync(id);
            if (user != null)
            {
                switch (number)
                {
                    case 1: // Active User
                        user.Status = UserStatus.Active;
                        break;
                    case 2:// Deactive User
                        user.Status = UserStatus.Deactive;
                        break;
                }
                _unit.UserRepository.Update(user);
                var result = await _unit.SaveChangeAsync();
                if (result > 0) return true;
                throw new ConflictException("Error when switch user status");
            }
            throw new BadRequestException("id not exist in database");
        }


    }
}