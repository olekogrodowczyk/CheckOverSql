using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Groups.Queries.GetAllAssignableGroups
{
    public class GetAllAssignableGroupsQuery : IRequest<IEnumerable<GetGroupDto>>
    {

    }

    public class GetAllAssignableGroupsQueryHandler : IRequestHandler<GetAllAssignableGroupsQuery, IEnumerable<GetGroupDto>>
    {
        private readonly IUserContextService _userContextService;
        private readonly IGroupRepository _groupRepository;
        private readonly IMapper _mapper;
        private readonly IAssignmentRepository _assignmentRepository;

        public GetAllAssignableGroupsQueryHandler(IUserContextService userContextService, IGroupRepository groupRepository
            ,IMapper mapper, IAssignmentRepository assignmentRepository)
        {
            _userContextService = userContextService;
            _groupRepository = groupRepository;
            _mapper = mapper;
            _assignmentRepository = assignmentRepository;
        }

        public async Task<IEnumerable<GetGroupDto>> Handle(GetAllAssignableGroupsQuery request, CancellationToken cancellationToken)
        {
            int? loggedUserId = _userContextService.GetUserId;
            if (loggedUserId is null) { throw new UnauthorizedAccessException(); }

            var assignments = await _assignmentRepository
                .GetWhereAsync(x=>x.UserId == (int)loggedUserId, x=>x.Group);
            var assignableAssignments = new List<Assignment>();
            foreach(var assignment in assignments)
            {
                if(await checkIfAssignmentCanAssignExerciseInGroup(assignment))
                {
                    assignableAssignments.Add(assignment);
                }
            }
            return _mapper.Map<IEnumerable<GetGroupDto>>(assignableAssignments.Select(x=>x.Group));
        }

        private async Task<bool> checkIfAssignmentCanAssignExerciseInGroup(Assignment assignment)
        {
            return await _assignmentRepository.CheckIfAssignmentHasPermission
                (assignment.Id, GetPermissionByEnum.GetPermissionName(PermissionNames.AssigningExercises));
        }
    }
}
