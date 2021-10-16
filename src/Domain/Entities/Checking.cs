using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Checking : AuditableEntity
    {
        public int CheckerId { get; set; }
        public virtual User Checker { get; set; }
        public int SolvingId { get; set; }
        public virtual Solving Solving { get; set; }
        public string Remarks { get; set; }
        public int Points { get; set; }
    }
}
