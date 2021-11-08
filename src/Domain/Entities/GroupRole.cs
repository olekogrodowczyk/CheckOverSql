using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class GroupRole : AuditableEntity
    {
        public string Name { get; set; }
        public bool IsCustom { get; set; }
        public virtual ICollection<Assignment> Assignments { get; set; }
        public virtual ICollection<Invitation> Invitations { get; set; }
        public virtual ICollection<GroupRolePermission> RolePermissions { get; set; }
    }
}
