using Application.Authorization;
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
            var receiver = await _userRepository.GetByEmail(command.ReceiverEmail);
            var groupRole = await _groupRoleRepository.GetByName(command.RoleName);
            var group = await _groupRepository.GetByIdAsync(command.GroupId);
            var assignment = await _assignmentRepository.SingleAsync(x => x.UserId == _userContextService.GetUserId);

            var authorizationResult = await _authorizationService.AuthorizeAsync(_userContextService.UserClaimPrincipal, assignment
                , new PermissionRequirement(PermissionNames.SendingInvitations));

            var invitation = new Invitation
            {
                SenderId = (int)_userContextService.GetUserId,
                Receiver = receiver,
                Status = InvitationStatusEnum.Sent.ToString(),
                GroupRole = groupRole,
                Group = group
            };

            await _invitationRepository.AddAsync(invitation);
            return invitation.Id;
        }
    }
}
