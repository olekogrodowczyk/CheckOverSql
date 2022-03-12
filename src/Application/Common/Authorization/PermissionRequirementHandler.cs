using Application.Authorization;
using Application.Common.Exceptions;
using Application.Interfaces;
using Domain.Common;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Authorization
{
    public class PermissionRequirementHandler : AuthorizationHandler<PermissionRequirement, Group>
    {
        private readonly IUserContextService _userContextService;
        private readonly IAssignmentRepository _assignmentRepository;
        private readonly IRepository<Permission> _permissionRepository;

        public PermissionRequirementHandler(IUserContextService userContextService, IAssignmentRepository assignmentRepository
            , IRepository<Permission> permissionRepository)
        {
            _userContextService = userContextService;
            _assignmentRepository = assignmentRepository;
            _permissionRepository = permissionRepository;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement, Group group)
        {
            if (context is null || context.User.Claims.Count() == 0) { throw new UnauthorizedAccessException(); }
            var userId = int.Parse(context.User.FindFirst(x => x.Type == ClaimTypes.NameIdentifier).Value);

            var assignment = await _assignmentRepository.SingleOrDefaultAsync(x => x.UserId == userId && x.GroupId == group.Id);
            if (assignment is null) { throw new ForbidException($"Access denied for permission: {requirement.Permission.GetDisplayName()}", true); }

            if (!await _permissionRepository.AnyAsync(x => x.Title == requirement.Permission.GetDisplayName()))
            {
                throw new NotFoundException($"Permission {requirement.Permission.GetDisplayName()} cannot be found");
            }
            var result = await _assignmentRepository.CheckIfAssignmentHasPermission(assignment.Id, requirement.Permission);
            if (result) { context.Succeed(requirement); }
        }
    }
}
