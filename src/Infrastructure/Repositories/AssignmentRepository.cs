using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class AssignmentRepository : EfRepository<Assignment>, IAssignmentRepository
    {
        public AssignmentRepository(ApplicationDbContext context, ILogger<AssignmentRepository> logger) : base(context, logger)
        {

        }

        public async Task<bool> CheckIfAssignmentHasPermission(int assignmentId, string permissionTitle)
        {
            var assignment = await _context
                .Assignments
                .Include(x=>x.GroupRole)
                .ThenInclude(x=>x.RolePermissions)
                .ThenInclude(x=>x.Permission)
                .SingleOrDefaultAsync(x=>x.Id == assignmentId);
            var permission = await _context.Permissions.SingleOrDefaultAsync(x=>x.Title == permissionTitle);
            var result = assignment.GroupRole.RolePermissions.Select(x => x.Permission.Title).Contains(permissionTitle);
            return result;
        }
    }
}
