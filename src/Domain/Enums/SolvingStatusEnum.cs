using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Enums
{
    public enum SolvingStatusEnum
    {
        ToDo=1,
        Overdue,
        Done,
        DoneButOverdue,
        Checked
    }
}
