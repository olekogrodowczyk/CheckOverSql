using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Assignment : AuditableEntity
    {
        public int UserId { get; set; }
        public virtual User User { get; set; }
        public int GroupId { get; set; }
        public virtual Group Group{ get; set; }
        public int RoleId { get; set; }
        public virtual GroupRole GroupRole {  get; set; }

        public virtual ICollection<Solving> Solvings { get; set; }
    }
}
