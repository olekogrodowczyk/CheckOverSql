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
        public Assignment()
        {
        }

        public Assignment(int userId, int groupId, int groupRoleId)
        {
            UserId = userId;
            GroupId = groupId;
            GroupRoleId = groupRoleId;
        }

        public int UserId { get; set; }
        public virtual User User { get; set; }
        public int GroupId { get; set; }
        public virtual Group Group { get; set; }
        public int GroupRoleId { get; set; }
        public virtual GroupRole GroupRole { get; set; }

        public virtual ICollection<Solving> Solvings { get; set; }
    }
}
