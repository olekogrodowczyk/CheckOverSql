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
using Application.Dto.GetGroupDto;

namespace Application.Services
{
    public class GroupService : IGroupService
    {
        private readonly IMapper _mapper;
        private readonly IUserContextService _userContextService;
        private readonly IGroupRoleRepository _groupRoleRepository;
        private readonly IAssignmentRepository _assignmentRepository;
        private readonly IGroupRepository _groupRepository;

        public GroupService(IMapper mapper, IUserContextService userContextService, IGroupRoleRepository groupRoleRepository
            ,IAssignmentRepository assignmentRepository, IGroupRepository groupRepository)
        {
            _mapper = mapper;
            _userContextService = userContextService;
            _groupRoleRepository = groupRoleRepository;
            _assignmentRepository = assignmentRepository;
            _groupRepository = groupRepository;
        }

        public async Task<int> CreateGroupAsync(CreateGroupDto model)
        {
            var group = _mapper.Map<Group>(model);
            group.Creator = _userContextService.User;

            var newRole = await _groupRoleRepository.GetByName("Założyciel");

            var newAssignment = new Assignment
            {
                User=_userContextService.User,
                GroupRole=newRole,
                Group=group,
            };

            await _groupRepository.Add(group);
            await _assignmentRepository.Add(newAssignment);
            return group.Id;
            
        }

        public async Task<IEnumerable<GetGroupDto>> GetUserGroups()
        {
            var groups = await _groupRepository.GetWhere(x => x.CreatorId == _userContextService.GetUserId);
            var groupsDto = _mapper.Map<IEnumerable<GetGroupDto>>(groups);
            return groupsDto;
        }
        
           
           
    }
}
