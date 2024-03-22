using Domain.Entities.Generic;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Syllabus:BaseEntity
    {
        public int CourseSlot { get; set; }
        public int SlotTime { get; set; }
        [MaxLength(500)] public string? Target {  get; set; }
        [Column(TypeName = "tinyint")] public SyllabusStatus status { get; set; } = SyllabusStatus.Active;
        public virtual PassCondition? PassCondition { get; set; } = null!;
        public int? PassConditionId { get; set; }

        public virtual Course? Course { get; set; }

    }
}
