using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Group : AuditableEntity
    {
        public string Name { get; set; }
        public int CreatorId { get; set; }
        public virtual User Creator { get; set; }
        public virtual ICollection<Assignment> Assignments { get; set; }
        public virtual ICollection<Invitation> Invitations { get; set; }
    }
}
