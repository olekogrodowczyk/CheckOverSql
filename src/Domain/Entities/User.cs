using Domain.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class User : AuditableEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public int RoleId { get; set; }
        public virtual Role Role { get; set; }
        public virtual ICollection<Assignment> Assignments{ get; set; }
        public virtual ICollection<Invitation> InvitationsSent { get; set; }
        public virtual ICollection<Invitation> InvitationsReceived{ get; set; }
        public virtual ICollection<Exercise> Exercises { get; set; }
        public virtual ICollection<Checking> Checkings { get; set; }
        public virtual ICollection<Group> GroupsCreated { get; set; }
        public virtual ICollection<Solution> Solutions { get; set; }
        public virtual ICollection<Solving> SolvingsCreated { get; set; }

    }
}
