using System.ComponentModel.DataAnnotations;
using Domain.Entities.Generic;

namespace Domain.Entities
{
    public class RefreshToken : BaseEntity
    {
        [Required] public string Token { get; set; } = string.Empty;
        [Required] public int UserId { get; set; }

        public virtual User User { get; set; } = null!;
    }
}