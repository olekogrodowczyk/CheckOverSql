using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Invitation : AuditableEntity
    {
        public int GroupId { get; set; }
        public virtual Group Group { get; set; }
        public int SenderId { get; set; }
        public virtual User Sender { get; set; }
        public int ReceiverId { get; set; }
        public virtual User Receiver { get; set; }
        public string Status { get; set; }
        public int GroupRoleId { get; set; }
        public virtual GroupRole GroupRole { get; set; }
        public DateTime AnsweredAt { get; set; }
    }
}
