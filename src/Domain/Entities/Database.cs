using Domain.Common;
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
        public string ConnectionString { get; set; }
        public string ConnectionStringAdmin { get; set; }
        public virtual ICollection<Query> Queries { get; set; }
    }
}
