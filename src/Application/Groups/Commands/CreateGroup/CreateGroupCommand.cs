using Application.Common.Interfaces;
using Application.Interfaces;
using Application.Mappings;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        public IFormFile Image { get; set; }

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
        private readonly IUploadFileService _uploadFileService;

        public CreateGroupCommandHandler(IMapper mapper, IGroupRoleRepository groupRoleRepository,
            IUserContextService userContextService, IGroupRepository groupRepository,
            IAssignmentRepository assignmentRepository, IUploadFileService uploadFileService)
        {
            _mapper = mapper;
            _groupRoleRepository = groupRoleRepository;
            _userContextService = userContextService;
            _groupRepository = groupRepository;
            _assignmentRepository = assignmentRepository;
            _uploadFileService = uploadFileService;
        }

        public async Task<int> Handle(CreateGroupCommand request, CancellationToken cancellationToken)
        {
            int? loggedUserId = _userContextService.GetUserId;
            if (loggedUserId is null) { throw new UnauthorizedAccessException(); }

            var group = _mapper.Map<Group>(request);
            group.CreatorId = (int)loggedUserId;

            var newRole = await _groupRoleRepository.GetByName("Owner");
            group.ImageName = await _uploadFileService.UploadFile(request.Image, "wwwroot/images/groups");

            var newAssignment = new Assignment
            {
                UserId = (int)loggedUserId,
                GroupRole = newRole,
                Group = group,
            };

            await _groupRepository.AddAsync(group);
            await _assignmentRepository.AddAsync(newAssignment);
            return group.Id;
        }
    }
}
