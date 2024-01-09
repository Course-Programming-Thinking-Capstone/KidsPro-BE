using Domain.Enums;

namespace Application.Dtos.Response.User
{
    public class UserResponseDto
    {
        public string FullName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string? PictureUrl { get; set; }
        public Gender? Gender { get; set; }
        public UserStatus Status { get; set; } 
        public bool IsDelete { get; set; } = false;

        public DateTime CreatedDate { get; } = DateTime.UtcNow;

        public string? RoleName { get; set; } = null!;

    }
}
