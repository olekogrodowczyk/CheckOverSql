using Domain.Entities;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IAssignmentRepository : IRepository<Assignment>
    {
        Task<bool> CheckIfAssignmentHasPermission(int assignmentId, PermissionEnum permissionsEnum);
        Task<Assignment> GetUserAssignmentBasedOnOtherAssignment(int userId, int assignmentId);
    }
}
