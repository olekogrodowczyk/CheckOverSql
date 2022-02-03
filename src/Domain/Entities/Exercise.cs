using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Exercise : AuditableEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int DatabaseId { get; set; }
        public virtual Database Database { get; set; }
        public int CreatorId { get; set; }
        public virtual User Creator { get; set; }
        public virtual ICollection<Solving> Solvings { get; set; }
        public virtual ICollection<Solution> Solutions { get; set; }
        public virtual ICollection<Comparison> Comparisons { get; set; }
        public bool IsPrivate { get; set; } = true;
        public string ValidAnswer { get; set; }
    }
}
