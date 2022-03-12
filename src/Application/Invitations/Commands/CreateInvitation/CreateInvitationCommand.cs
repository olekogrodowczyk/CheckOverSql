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

namespace Application.Invitations.Commands.CreateInvitation
{
    public class CreateInvitationCommand : IRequest<int>
    {
        public string ReceiverEmail { get; set; }
        public string RoleName { get; set; }
        public int GroupId { get; set; }
    }

    public class CreateInvitationCommandHandler : IRequestHandler<CreateInvitationCommand, int>
    {
        private readonly IUserRepository _userRepository;
        private readonly IGroupRepository _groupRepository;
        private readonly IGroupRoleRepository _groupRoleRepository;
        private readonly IAssignmentRepository _assignmentRepository;
        private readonly IUserContextService _userContextService;
        private readonly IAuthorizationService _authorizationService;
        private readonly IInvitationRepository _invitationRepository;

        public CreateInvitationCommandHandler(IUserRepository userRepository, IGroupRepository groupRepository
            , IGroupRoleRepository groupRoleRepository, IAssignmentRepository assignmentRepository,
            IUserContextService userContextService, IAuthorizationService authorizationService,
            IInvitationRepository invitationRepository)
        {
            _userRepository = userRepository;
            _groupRepository = groupRepository;
            _groupRoleRepository = groupRoleRepository;
            _assignmentRepository = assignmentRepository;
            _userContextService = userContextService;
            _authorizationService = authorizationService;
            _invitationRepository = invitationRepository;
        }

        public async Task<int> Handle(CreateInvitationCommand command, CancellationToken cancellationToken)
        {
            var receiver = await getReceiver(command.ReceiverEmail);
            var groupRole = await getGroupRole(command.RoleName);
            var group = await getGroup(command.GroupId);
            var assignment = await getAssignment(command.GroupId);
            var authorizationResult = await _authorizationService.AuthorizeAsync(_userContextService.UserClaimPrincipal, group
                , new PermissionRequirement(PermissionEnum.SendingInvitations));
            if (!authorizationResult.Succeeded) { throw new ForbidException(PermissionEnum.SendingInvitations); }

            var invitation = new Invitation
            {
                SenderId = (int)_userContextService.GetUserId,
                Receiver = receiver,
                Status = InvitationStatusEnum.Sent,
                GroupRole = groupRole,
                Group = group
            };

            await _invitationRepository.AddAsync(invitation);
            return invitation.Id;
        }

        private async Task<User> getReceiver(string receiverEmail)
        {
            var receiver = await _userRepository.GetByEmail(receiverEmail);
            if (receiver is null) { throw new NotFoundException(nameof(receiver), receiverEmail); }
            return receiver;
        }

        private async Task<GroupRole> getGroupRole(string groupRoleName)
        {
            var groupRole = await _groupRoleRepository.GetByName(groupRoleName);
            if (groupRole is null) { throw new NotFoundException(nameof(groupRole), groupRoleName); }
            return groupRole;
        }

        private async Task<Group> getGroup(int groupId)
        {
            var group = await _groupRepository.GetByIdAsync(groupId);
            if (group is null) { throw new NotFoundException(nameof(group), groupId); }
            return group;
        }

        private async Task<Assignment> getAssignment(int groupId)
        {
            int? loggedUserId = _userContextService.GetUserId;
            if (loggedUserId is null) { throw new UnauthorizedAccessException(); }
            var assignment = await _assignmentRepository
                .SingleOrDefaultAsync(x => x.UserId == (int)loggedUserId && x.GroupId == groupId);
            if (assignment is null) { throw new NotFoundException(nameof(assignment), groupId); }
            return assignment;
        }
    }
}
