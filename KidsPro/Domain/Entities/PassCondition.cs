using Domain.Entities.Generic;

namespace Domain.Entities
{
    public class PassCondition:BaseEntity
    {
        public int PassRatio { get; set; }

        public ICollection<Syllabus>? Syllabuses { get; set; }
        public ICollection<Quiz>? Quizzes { get; set; }
    }
}
