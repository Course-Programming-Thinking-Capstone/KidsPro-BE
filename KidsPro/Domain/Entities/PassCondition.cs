using Domain.Entities.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class PassCondition:BaseEntity
    {
        public int PassRatio { get; set; }

        public ICollection<Syllabus>? Syllabi { get; set; }
        public ICollection<Quiz>? Quizs { get; set; }
    }
}
