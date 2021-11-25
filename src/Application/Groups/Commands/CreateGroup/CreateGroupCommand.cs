using Application.Interfaces;
using Application.Mappings;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Groups.Commands.CreateGroup
{
    public class CreateGroupCommand : IMap, IRequest<int>
    {
        public string Name { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<CreateGroupCommand, Group>();
        }
    }

    public class CreateGroupCommandHandler : IRequestHandler<CreateGroupCommand, int>
    {
        private readonly IMapper _mapper;
        private readonly IGroupRoleRepository _groupRoleRepository;
        private readonly IUserContextService _userContextService;
        private readonly IGroupRepository _groupRepository;
        private readonly IAssignmentRepository _assignmentRepository;

        public CreateGroupCommandHandler(IMapper mapper, IGroupRoleRepository groupRoleRepository,
            IUserContextService userContextService, IGroupRepository groupRepository,
            IAssignmentRepository assignmentRepository)
        {
            _mapper = mapper;
            _groupRoleRepository = groupRoleRepository;
            _userContextService = userContextService;
            _groupRepository = groupRepository;
            _assignmentRepository = assignmentRepository;
        }

        public async Task<int> Handle(CreateGroupCommand request, CancellationToken cancellationToken)
        {
            var group = _mapper.Map<Group>(request);
            group.CreatorId = (int)_userContextService.GetUserId;

            var newRole = await _groupRoleRepository.GetByName("Owner");

            var newAssignment = new Assignment
            {
                User = _userContextService.User,
                GroupRole = newRole,
                Group = group,
            };

            await _groupRepository.AddAsync(group);
            await _assignmentRepository.AddAsync(newAssignment);
            return group.Id;
        }
    }
}
