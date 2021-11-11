﻿using Application.Interfaces;
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
using Application.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Application.Authorization;

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

        public GroupService(IMapper mapper, IUserContextService userContextService, IGroupRoleRepository groupRoleRepository
            ,IAssignmentRepository assignmentRepository, IGroupRepository groupRepository, IAuthorizationService authorizationService)
        {
            _mapper = mapper;
            _userContextService = userContextService;
            _groupRoleRepository = groupRoleRepository;
            _assignmentRepository = assignmentRepository;
            _groupRepository = groupRepository;
            _authorizationService = authorizationService;
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
            var groups = await _groupRepository.GetWhereAsync(x => x.CreatorId == _userContextService.GetUserId);
            var groupsDto = _mapper.Map<IEnumerable<GetGroupVm>>(groups);
            return groupsDto;
        }

        public async Task DeleteGroup(int groupId)
        {
            var userAssignment = await _assignmentRepository.SingleAsync(x => x.UserId == _userContextService.GetUserId);
            await _authorizationService.AuthorizeAsync(_userContextService.UserClaimPrincipal
                , userAssignment, new PermissionRequirement("Deleting group"));

            var group = await _groupRepository.GetByIdAsync(groupId);
            var assignments = await _assignmentRepository.GetWhereAsync(x => x.Group == group);
            assignments.ToList().ForEach(async x => await _assignmentRepository.DeleteAsync(x.Id));
            await _groupRepository.DeleteAsync(groupId);
        }



    }
}
