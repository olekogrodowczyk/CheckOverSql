using Application.Authorization;
using Application.Common.Exceptions;
using Application.Interfaces;
using Domain.Enums;
using Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.GroupRoles.Commands
{
    public class ChangeRoleCommand : IRequest<int>
    {
        public string RoleName { get; set; }
        public int UserId { get; set; }
        public int GroupId { get; set; }
    }

    public class ChangeGroupRoleCommandHandler : IRequestHandler<ChangeRoleCommand, int>
    {
        private readonly IGroupRoleRepository _groupRoleRepository;
        private readonly IUserContextService _userContextService;
        private readonly IAssignmentRepository _assignmentRepository;
        private readonly IAuthorizationService _authorizationService;
        private readonly IGroupRepository _groupRepository;

        public ChangeGroupRoleCommandHandler(IGroupRoleRepository groupRoleRepository, IUserContextService userContextService
            , IAssignmentRepository assignmentRepository, IAuthorizationService authorizationService, IGroupRepository groupRepository)
        {
            _groupRoleRepository = groupRoleRepository;
            _userContextService = userContextService;
            _assignmentRepository = assignmentRepository;
            _authorizationService = authorizationService;
            _groupRepository = groupRepository;
        }

        public async Task<int> Handle(ChangeRoleCommand request, CancellationToken cancellationToken)
        {
            var assignment = await _assignmentRepository
                .SingleOrDefaultAsync(x => x.UserId == request.UserId && x.GroupId == request.GroupId);
            if (assignment is null) { throw new NotFoundException(nameof(assignment), request.UserId); }

            var groupRole = await _groupRoleRepository.FirstOrDefaultAsync(x => x.Name == request.RoleName);
            if (groupRole is null) { throw new NotFoundException(nameof(groupRole), request.RoleName); }

            var group = await _groupRepository.GetByIdAsync(request.GroupId);
            var authorizationResult = await _authorizationService.AuthorizeAsync(_userContextService.UserClaimPrincipal
                , group, new PermissionRequirement(PermissionEnum.ChangingRoles));
            if (!authorizationResult.Succeeded) { throw new ForbidException(PermissionEnum.ChangingRoles); }

            assignment.GroupRoleId = groupRole.Id;
            await _assignmentRepository.UpdateAsync(assignment);
            return assignment.Id;
        }
    }
}
