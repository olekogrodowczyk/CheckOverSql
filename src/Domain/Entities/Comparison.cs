using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Comparison : AuditableEntity
    {
        public int SolutionId { get; set; }
        public virtual Solution Solution { get; set; }
        public int ExerciseId { get; set; }
        public virtual Exercise Exercise { get; set; }
        public bool Result { get; set; }

    }
}
