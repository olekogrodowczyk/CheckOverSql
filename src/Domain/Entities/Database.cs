using Domain.Common;
using Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Database : AuditableEntity
    {
        public string Name { get; set; }
        public ConnectionString ConnectionString { get; set; }
        public virtual ICollection<Query> Queries { get; set; }
        public virtual ICollection<Exercise> Exercises { get; set; }
    }
}
