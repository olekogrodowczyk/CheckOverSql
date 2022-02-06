using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Query : AuditableEntity
    {
        public string QueryValue { get; set; }
        public int DatabaseId { get; set; }
        public int CreatorId { get; set; }
        public virtual User Creator  { get; set; }
    }
}
