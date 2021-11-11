using Application.Authorization;
using Application.Dto.CreateInvitationDto;
using Application.Exceptions;
using Application.Interfaces;
using Application.ViewModels;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class InvitationService : IInvitationService
    {
        private readonly IMapper _mapper;
        private readonly IGroupRepository _groupRepository;
        private readonly IInvitationRepository _invitationRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUserContextService _userContextService;
        private readonly IGroupRoleRepository _groupRoleRepository;
        private readonly IAssignmentRepository _assignmentRepository;
        private readonly IAuthorizationService _authorizationService;

        public InvitationService(IMapper mapper, IGroupRepository groupRepository
            ,IInvitationRepository invitationRepository, IUserRepository userRepository
            ,IUserContextService userContextService, IGroupRoleRepository groupRoleRepository
            ,IAssignmentRepository assignmentRepository, IAuthorizationService authorizationService)
        {
            _mapper = mapper;
            _groupRepository = groupRepository;
            _invitationRepository = invitationRepository;
            _userRepository = userRepository;
            _userContextService = userContextService;
            _groupRoleRepository = groupRoleRepository;
            _assignmentRepository = assignmentRepository;
            _authorizationService = authorizationService;
        }

        public async Task<int> CreateInvitation(CreateInvitationDto model, int groupId)
        {
            var receiver = await _userRepository.GetByEmail(model.ReceiverEmail);
            var groupRole = await _groupRoleRepository.GetByName(model.RoleName);
            var group = await _groupRepository.GetByIdAsync(groupId);
            var assignment = await _assignmentRepository.SingleAsync(x => x.UserId == _userContextService.GetUserId);

            var authorizationResult = await _authorizationService.AuthorizeAsync(_userContextService.UserClaimPrincipal
                , assignment, new PermissionRequirement("Sending invitations"));

            var invitation = new Invitation
            {
                SenderId = (int)_userContextService.GetUserId,
                Receiver = receiver,
                Status = InvitationStatus.Sent.ToString(),
                GroupRole = groupRole,
                Group = group
            };

            await _invitationRepository.AddAsync(invitation);
            return invitation.Id;
        }

        public async Task CheckIfInvitationAlreadyExists(string email, string role, int groupId)
        {
            var receiver = await _userRepository.GetByEmail(email);
            var groupRole = await _groupRoleRepository.GetByName(role);

            bool result = await _invitationRepository
                .ExistsAsync(x => x.ReceiverId == receiver.Id && x.Status == "Sent"
                && x.GroupId == groupId && x.GroupRoleId == groupRole.Id);

            if(result) { throw new AlreadyExistsException($"Similar invitation already exists"); }
        }

        public async Task CheckIfUserIsAlreadyInGroup(string email, string role, int groupId)
        {
            var receiver = await _userRepository.GetByEmail(email);
            bool result = await _assignmentRepository.ExistsAsync(x=>x.UserId==receiver.Id && x.GroupId == groupId);

            if(result) { throw new AlreadyExistsException($"User already is in the group"); }
        }

        public async Task CheckIfSenderIsInTheGroup(int groupId)
        {
            int senderId = (int)_userContextService.GetUserId;
            var result = await _assignmentRepository.ExistsAsync(x => x.GroupId == groupId && x.UserId == senderId);
            
            if(!result) { throw new NotFoundException($"Sender is not in the group"); }
        }

        public async Task<IEnumerable<GetInvitationVm>> GetAllUserReceivedInvitations()
        {
            var invitations = await _invitationRepository.GetInvitationsWithAllIncludes();
            invitations = invitations.Where(x=>x.ReceiverId == _userContextService.GetUserId);
            var invitationsVm = _mapper.Map<IEnumerable<GetInvitationVm>>(invitations);
            return invitationsVm;
        }

        public async Task<IEnumerable<GetInvitationVm>> GetAllUserSentInvitations()
        {
            var invitations = await _invitationRepository.GetInvitationsWithAllIncludes();
            invitations = invitations.Where(x => x.SenderId == _userContextService.GetUserId);
            var invitationsVm = _mapper.Map<IEnumerable<GetInvitationVm>>(invitations);
            return invitationsVm;
        }

        public async Task<IEnumerable<GetInvitationVm>> GetAllUserInvitations()
        {
            var invitations = await _invitationRepository.GetInvitationsWithAllIncludes();
            invitations = invitations.Where
                (x => x.ReceiverId == _userContextService.GetUserId
                || x.SenderId == _userContextService.GetUserId);
            var invitationsVm = _mapper.Map<IEnumerable<GetInvitationVm>>(invitations);
            return invitationsVm;
        }

        public async Task AcceptInvitation(int invitationId)
        {
            var invitation = await _invitationRepository.GetByIdAsync(invitationId);
            if(invitation.Status != InvitationStatus.Sent.ToString())
            {
                throw new BadRequestException("The invitation isn't pending", true);
            }

            await _assignmentRepository.AddAsync(new Assignment
            {
                GroupId = invitation.GroupId,
                GroupRoleId = invitation.GroupRoleId,
                UserId = invitation.ReceiverId,               
            });

            invitation.Status = InvitationStatus.Accepted.ToString();
            await _invitationRepository.UpdateAsync(invitation);
        }
        
        public async Task RejectInvitation(int invitationId)
        {
            var invitation = await _invitationRepository.GetByIdAsync(invitationId);
            if(invitation.Status != InvitationStatus.Sent.ToString())
            {
                throw new BadRequestException("The invitation isn't pending", true);
            }

            invitation.Status = InvitationStatus.Rejected.ToString();
            await _invitationRepository.UpdateAsync(invitation);
        }


    }
}
