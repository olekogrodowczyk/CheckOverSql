using Application.Interfaces;
using Application.Responses;
using Application.Dto.CreateGroupVm;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Solvings;
using Microsoft.AspNetCore.Authorization;
using Application.Authorization;
using Domain.Enums;
using Application.Common.Exceptions;

namespace Application.Services
{
    public class GroupService : IGroupService
    {
        private readonly IMapper _mapper;
        private readonly IUserContextService _userContextService;
        private readonly IGroupRoleRepository _groupRoleRepository;
        private readonly IAssignmentRepository _assignmentRepository;
        private readonly IGroupRepository _groupRepository;
        private readonly IAuthorizationService _authorizationService;
        private readonly ISolvingRepository _solvingRepository;
        private readonly ISolutionRepository _solutionRepository;

        public GroupService(IMapper mapper, IUserContextService userContextService, IGroupRoleRepository groupRoleRepository
            ,IAssignmentRepository assignmentRepository, IGroupRepository groupRepository, IAuthorizationService authorizationService
            ,ISolvingRepository solvingRepository, ISolutionRepository solutionRepository)
        {
            _mapper = mapper;
            _userContextService = userContextService;
            _groupRoleRepository = groupRoleRepository;
            _assignmentRepository = assignmentRepository;
            _groupRepository = groupRepository;
            _authorizationService = authorizationService;
            _solvingRepository = solvingRepository;
            _solutionRepository = solutionRepository;
        }

        public async Task<int> CreateGroup(CreateGroupDto model)
        {
            var group = _mapper.Map<Group>(model);
            group.Creator = _userContextService.User;

            var newRole = await _groupRoleRepository.GetByName("Owner");

            var newAssignment = new Assignment
            {
                User=_userContextService.User,
                GroupRole=newRole,
                Group=group,
            };

            await _groupRepository.AddAsync(group);
            await _assignmentRepository.AddAsync(newAssignment);
            return group.Id;         
        }

        public async Task<IEnumerable<GetGroupVm>> GetUserGroups()
        {
            int loggedUserId = (int)_userContextService.GetUserId;
            var userAssignments = await _assignmentRepository.GetWhereAsync(x => x.UserId == loggedUserId, x=>x.Group);
            var groupsDto = _mapper.Map<IEnumerable<GetGroupVm>>(userAssignments.Select(x=>x.Group));
            return groupsDto;
        }

        public async Task DeleteGroup(int groupId)
        {
            Assignment userAssignment = await _assignmentRepository
                .SingleOrDefaultAsync(x => x.UserId == _userContextService.GetUserId && x.GroupId == groupId);
            if (userAssignment == null) { throw new ForbidException("You're not in the group", true); }

            await _authorizationService.AuthorizeAsync(_userContextService.UserClaimPrincipal
                , userAssignment, new PermissionRequirement(PermissionNames.DeletingGroup));

            //Assignments are needed to be included first
            await _assignmentRepository.GetAllAsync(x => x.Group);
            await _groupRepository.DeleteAsync(groupId, x=>x.Assignments);
        }

        


        public async Task<IEnumerable<GetAssignmentVm>> GetAllAssignmentsInGroup(int groupId)
        {
            Assignment userAssignment = await _assignmentRepository
                .SingleOrDefaultAsync(x => x.UserId == _userContextService.GetUserId && x.GroupId == groupId);
            if(userAssignment == null) { throw new ForbidException("You're not in the group", true); }

            await _authorizationService.AuthorizeAsync(_userContextService.UserClaimPrincipal
                , userAssignment, new PermissionRequirement(PermissionNames.GettingAssignments));

            await _assignmentRepository.GetAllAsync();
            var group = await _groupRepository.GetWhereAsync(x => x.Id == groupId, x => x.Assignments);
            var assignmentDtos = _mapper.Map<IEnumerable<GetAssignmentVm>>(group.SelectMany(x=>x.Assignments));
            return assignmentDtos;
        }

    }
}
