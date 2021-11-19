using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Solving : AuditableEntity
    {
        public int? AssignmentId { get; set; }
        public virtual Assignment Assignment { get; set; }
        public int ExerciseId { get; set; }
        public virtual Exercise Exercise { get; set; }
        public int CreatorId { get; set; }
        public virtual User Creator { get; set; }
        public string Status { get; set; }
        public DateTime? SentAt { get; set; }
        public DateTime? DeadLine { get; set; }
        public virtual Checking Checking { get; set; }
        public virtual Solution Solution { get; set; }
    }
}
