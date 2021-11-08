using Application.Dto.CreateInvitationDto;
using Application.Exceptions;
using Application.Interfaces;
using Application.ViewModels;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
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

        public InvitationService(IMapper mapper, IGroupRepository groupRepository
            ,IInvitationRepository invitationRepository, IUserRepository userRepository
            ,IUserContextService userContextService, IGroupRoleRepository groupRoleRepository
            ,IAssignmentRepository assignmentRepository)
        {
            _mapper = mapper;
            _groupRepository = groupRepository;
            _invitationRepository = invitationRepository;
            _userRepository = userRepository;
            _userContextService = userContextService;
            _groupRoleRepository = groupRoleRepository;
            _assignmentRepository = assignmentRepository;
        }

        public async Task<int> CreateInvitation(CreateInvitationDto model, int groupId)
        {
            var receiver = await _userRepository.GetByEmail(model.ReceiverEmail);
            var groupRole = await _groupRoleRepository.GetByName(model.RoleName);
            var group = await _groupRepository.GetById(groupId);

            var invitation = new Invitation
            {
                SenderId = (int)_userContextService.GetUserId,
                Receiver = receiver,
                Status = InvitationStatus.Sent.ToString(),
                GroupRole = groupRole,
                Group = group
            };

            await _invitationRepository.Add(invitation);
            return invitation.Id;
        }

        public async Task CheckIfInvitationAlreadyExists(string email, string role, int groupId)
        {
            var receiver = await _userRepository.GetByEmail(email);
            var groupRole = await _groupRoleRepository.GetByName(role);

            bool result = await _invitationRepository
                .Exists(x => x.ReceiverId == receiver.Id && x.Status == "Sent"
                && x.GroupId == groupId && x.GroupRoleId == groupRole.Id);

            if(result) { throw new AlreadyExistsException($"Similar invitation already exists"); }
        }

        public async Task CheckIfUserIsAlreadyInGroup(string email, string role, int groupId)
        {
            var receiver = await _userRepository.GetByEmail(email);
            bool result = await _assignmentRepository.Exists(x=>x.UserId==receiver.Id && x.GroupId == groupId);

            if(result) { throw new AlreadyExistsException($"User already is in the group"); }
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


    }
}
