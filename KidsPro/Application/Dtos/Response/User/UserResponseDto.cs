using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.Response.User
{
    public class UserResponseDto
    {
        public string FullName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string? PictureUrl { get; set; }
        public Gender? Gender { get; set; }
        public UserStatus Status { get; set; } = UserStatus.Active;
        public bool IsDelete { get; set; } = false;

        public DateTime CreatedDate { get; } = DateTime.UtcNow;

        public string? RoleName { get; set; } = null!;

    }
}
