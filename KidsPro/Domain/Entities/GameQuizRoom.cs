using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities.Generic;
using Microsoft.EntityFrameworkCore;

namespace Domain.Entities;

public class GameQuizRoom : BaseEntity
{
    [DataType(DataType.DateTime)]
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}")]
    [Precision(2)]
    public DateTime StartTime { get; set; }

    [Range(0, 10000)]
    [Column(TypeName = "smallint")]
    public int Duration { get; set; }

    [Required] public int TeacherId { get; set; }
    [Required] public int GameLevelId { get; set; }
    public string? JoinCode { get; set; }

    public virtual ICollection<GameStudentQuiz> StudentQuizzes { get; set; } = new List<GameStudentQuiz>();
}