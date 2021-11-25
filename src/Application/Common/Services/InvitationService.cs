using Application.Authorization;
using Application.Common.Exceptions;
using Application.Dto.CreateInvitationDto;
using Application.Interfaces;
using Application.Groups;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<InvitationService> _logger;

        public InvitationService(IMapper mapper, IGroupRepository groupRepository
            ,IInvitationRepository invitationRepository, IUserRepository userRepository
            ,IUserContextService userContextService, IGroupRoleRepository groupRoleRepository
            ,IAssignmentRepository assignmentRepository, IAuthorizationService authorizationService
            ,ILogger<InvitationService> logger)
        {
            _mapper = mapper;
            _groupRepository = groupRepository;
            _invitationRepository = invitationRepository;
            _userRepository = userRepository;
            _userContextService = userContextService;
            _groupRoleRepository = groupRoleRepository;
            _assignmentRepository = assignmentRepository;
            _authorizationService = authorizationService;
            _logger = logger;
        }

        

        public async Task CheckIfInvitationAlreadyExists(string email, string role, int groupId)
        {
            var receiver = await _userRepository.GetByEmail(email);
            var groupRole = await _groupRoleRepository.GetByName(role);

            bool result = await _invitationRepository
                .AnyAsync(x => x.ReceiverId == receiver.Id && x.Status == "Sent"
                && x.GroupId == groupId && x.GroupRoleId == groupRole.Id);

            if(result) { throw new AlreadyExistsException($"Similar invitation already exists"); }
        }

        public async Task CheckIfUserIsAlreadyInGroup(string email, string role, int groupId)
        {
            var receiver = await _userRepository.GetByEmail(email);
            bool result = await _assignmentRepository.AnyAsync(x=>x.UserId==receiver.Id && x.GroupId == groupId);

            if(result) { throw new AlreadyExistsException($"User already is in the group"); }
        }

        public async Task CheckIfSenderIsInTheGroup(int groupId)
        {
            int senderId = (int)_userContextService.GetUserId;
            var result = await _assignmentRepository.AnyAsync(x => x.GroupId == groupId && x.UserId == senderId);
            
            if(!result) { throw new NotFoundException($"Sender is not in the group"); }
        }

      

    }
}
