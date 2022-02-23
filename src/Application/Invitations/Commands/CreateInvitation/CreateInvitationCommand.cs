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
            var data = await getData(command); 

            var authorizationResult = await _authorizationService.AuthorizeAsync(_userContextService.UserClaimPrincipal, data.Item1
                , new PermissionRequirement(PermissionNames.SendingInvitations));

            var invitation = new Invitation
            {
                SenderId = (int)_userContextService.GetUserId,
                Receiver = data.Item2,
                Status = InvitationStatusEnum.Sent.ToString(),
                GroupRole = data.Item3,
                Group = data.Item4,
            };

            await _invitationRepository.AddAsync(invitation);
            return invitation.Id;
        }

        private async Task<(Assignment, User, GroupRole, Group)> getData(CreateInvitationCommand command)
        {
            int? loggedUserId = _userContextService.GetUserId;
            if (loggedUserId is null) { throw new UnauthorizedAccessException(); }

            var receiver = await _userRepository.GetByEmail(command.ReceiverEmail);
            if(receiver is null) { throw new NotFoundException($"Receiver with email: {command.ReceiverEmail} hasn't been found"); }
           
            var groupRole = await _groupRoleRepository.GetByName(command.RoleName);
            if(groupRole is null) { throw new NotFoundException($"Group role with name: {command.RoleName} hasn't been found"); }
           
            var group = await _groupRepository.GetByIdAsync(command.GroupId);
            if(group is null) { throw new NotFoundException($"Group with id: {command.GroupId} hasn't been found"); }
            var assignment = await _assignmentRepository
                .SingleOrDefaultAsync(x => x.UserId == (int)loggedUserId && x.GroupId == command.GroupId);
            if (assignment is null) 
            { throw new NotFoundException
                    ($"Assignment with loggedUserId: {(int)loggedUserId} and groupId: {command.GroupId} hasn't been found"); }
            return (assignment, receiver, groupRole, group);
        }
    }
}
