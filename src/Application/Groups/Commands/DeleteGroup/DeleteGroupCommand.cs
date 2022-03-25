using Application.Authorization;
using Application.Common.Exceptions;
using Application.Interfaces;
using Domain.Entities;
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

namespace Application.Groups.Commands.DeleteGroup
{
    public class DeleteGroupCommand : IRequest
    {
        public int GroupId { get; set; }
    }

    public class DeleteGroupCommandHandler : IRequestHandler<DeleteGroupCommand>
    {
        private readonly IAssignmentRepository _assignmentRepository;
        private readonly IUserContextService _userContextService;
        private readonly IAuthorizationService _authorizationService;
        private readonly IGroupRepository _groupRepository;

        public DeleteGroupCommandHandler(IAssignmentRepository assignmentRepository, IUserContextService userContextService
            , IAuthorizationService authorizationService, IGroupRepository groupRepository)
        {
            _assignmentRepository = assignmentRepository;
            _userContextService = userContextService;
            _authorizationService = authorizationService;
            _groupRepository = groupRepository;
        }

        public async Task<Unit> Handle(DeleteGroupCommand command, CancellationToken cancellationToken)
        {
            Assignment userAssignment = await _assignmentRepository
                .SingleOrDefaultAsync(x => x.UserId == _userContextService.GetUserId && x.GroupId == command.GroupId);

            var group = await _groupRepository.GetByIdAsync(command.GroupId);
            if (group is null) { throw new NotFoundException(nameof(group), command.GroupId); }

            var authorizationResult = await _authorizationService.AuthorizeAsync(_userContextService.UserClaimPrincipal
                , group, new PermissionRequirement(PermissionEnum.DeletingGroup));
            if (!authorizationResult.Succeeded) { throw new ForbidException(PermissionEnum.DeletingGroup); }

            //Assignments need to be loaded in purpose to get cascade deleting working
            var assignments = await _assignmentRepository.GetAllAsync();
            await _groupRepository.DeleteAsync(command.GroupId);
            return Unit.Value;
        }
    }
}
