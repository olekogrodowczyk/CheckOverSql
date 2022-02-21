using Application.Interfaces;
using Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Groups.Queries.GetUserGroupRole
{
    public class GetUserGroupRoleQuery : IRequest<string>
    {
        public int GroupId { get; set; }
    }

    public class GetUserGroupRoleQueryHandler : IRequestHandler<GetUserGroupRoleQuery, string>
    {
        private readonly IUserContextService _userContextService;
        private readonly IGroupRepository _groupRepository;
        private readonly IAssignmentRepository _assignmentRepository;

        public GetUserGroupRoleQueryHandler(IUserContextService userContextService, IGroupRepository groupRepository
            ,IAssignmentRepository assignmentRepository)
        {
            _userContextService = userContextService;
            _groupRepository = groupRepository;
            _assignmentRepository = assignmentRepository;
        }

        public async Task<string> Handle(GetUserGroupRoleQuery request, CancellationToken cancellationToken)
        {
            int? loggedUserId = _userContextService.GetUserId;
            if(loggedUserId is null) { throw new UnauthorizedAccessException(); }
            var assignment = await _assignmentRepository
                .SingleOrDefaultAsync(x=>x.UserId == (int)loggedUserId && x.GroupId == request.GroupId, x=>x.GroupRole);
            if(assignment is not null) { return assignment.GroupRole.Name; }
            return null;
        }
    }
}
