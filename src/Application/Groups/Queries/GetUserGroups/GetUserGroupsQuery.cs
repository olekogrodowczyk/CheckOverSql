using Application.Interfaces;
using AutoMapper;
using Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Groups.Queries.GetUserGroups
{
    public class GetUserGroupsQuery : IRequest<IEnumerable<GetGroupDto>>
    {
    }

    public class GetUserGroupsQueryHandler : IRequestHandler<GetUserGroupsQuery, IEnumerable<GetGroupDto>>
    {
        private readonly IUserContextService _userContextService;
        private readonly IAssignmentRepository _assignmentRepository;
        private readonly IMapper _mapper;

        public GetUserGroupsQueryHandler(IUserContextService userContextService, IAssignmentRepository assignmentRepository
            ,IMapper mapper)
        {
            _userContextService = userContextService;
            _assignmentRepository = assignmentRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<GetGroupDto>> Handle(GetUserGroupsQuery request, CancellationToken cancellationToken)
        {
            int loggedUserId = (int)_userContextService.GetUserId;
            var userAssignments = await _assignmentRepository.GetWhereAsync(x => x.UserId == loggedUserId, x => x.Group);
            var groupsDto = _mapper.Map<IEnumerable<GetGroupDto>>(userAssignments.Select(x => x.Group));
            return groupsDto;
        }
    }
}
