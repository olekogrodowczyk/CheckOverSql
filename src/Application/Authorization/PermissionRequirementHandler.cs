using Application.Exceptions;
using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Application.Authorization
{
    public class PermissionRequirementHandler : AuthorizationHandler<PermissionRequirement, Assignment>
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

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement, Assignment assignment)
        {
            if (assignment == null) { throw new ForbidException($"Access denied for permission: {requirement.PermissionTitle}", true); }

            var result = await _assignmentRepository.CheckIfAssignmentHasPermission(assignment.Id, requirement.PermissionTitle);

            if (!await _permissionRepository.ExistsAsync(x => x.Title == requirement.PermissionTitle))
            {
                throw new NotFoundException($"Permission {requirement.PermissionTitle} cannot be found");
            }
            if (!result) { throw new ForbidException($"Access denied for permission: {requirement.PermissionTitle}", true); }

            context.Succeed(requirement);
        }
    }
}
