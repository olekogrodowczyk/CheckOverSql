using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IAssignmentRepository : IRepository<Assignment>
    {
        Task<bool> CheckIfAssignmentHasPermission(int assignmentId, string permissionTitle);
        Task<Assignment> GetUserAssignmentBasedOnOtherAssignment(int userId, int assignmentId);
    }
}
