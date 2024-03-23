using Domain.Entities.Generic;
using Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class Syllabus:BaseEntity
    {
        public int CourseSlot { get; set; }
        public int SlotTime { get; set; }
        [MaxLength(500)] public string? Target {  get; set; }
        [Column(TypeName = "tinyint")] public SyllabusStatus Status { get; set; } = SyllabusStatus.Active;
        public virtual PassCondition? PassCondition { get; set; } = null!;
        public int? PassConditionId { get; set; }

        public virtual Course? Course { get; set; }

    }
}
