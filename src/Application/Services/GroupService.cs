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
using Application.Groups;
using Microsoft.AspNetCore.Authorization;
using Application.Authorization;
using Domain.Enums;
using Application.Common.Exceptions;
using Application.Groups.Commands.CreateGroup;

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

        

        


        
    }
}
