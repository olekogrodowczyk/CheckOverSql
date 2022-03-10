using Application.Interfaces;
using Domain.Enums;
using Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.GroupRoles.Queries.CheckPermission
{
    public class CheckPermissionQuery : IRequest<bool>
    {
        public string Permission { get; set; }
        public int? GroupId { get; set; }
    }

    public class CheckPermissionQueryHandler : IRequestHandler<CheckPermissionQuery, bool>
    {
        private readonly IUserContextService _userContextService;
        private readonly IAssignmentRepository _assignmentRepository;

        public CheckPermissionQueryHandler(IUserContextService userContextService, IAssignmentRepository assignmentRepository)
        {
            _userContextService = userContextService;
            _assignmentRepository = assignmentRepository;
        }

        public async Task<bool> Handle(CheckPermissionQuery request, CancellationToken cancellationToken)
        {
            int? loggedUserId = _userContextService.GetUserId;
            if (loggedUserId is null) { throw new UnauthorizedAccessException(); }

            Enum.TryParse(request.Permission, out PermissionEnum permission);

            if (request.GroupId is not null)
            {
                return await checkForGroup((int)loggedUserId, permission, (int)request.GroupId);

            }
            return await checkForAllAssignments((int)loggedUserId, permission);
        }

        private async Task<bool> checkForAllAssignments(int loggedUserId, PermissionEnum permission)
        {
            var assignments = (await _assignmentRepository.GetWhereAsync(x => x.UserId == loggedUserId)).ToList();
            bool result = false;
            foreach (var assignment in assignments)
            {
                if (await _assignmentRepository.CheckIfAssignmentHasPermission
                    (assignment.Id, permission))
                {
                    result = true;
                }
            }
            return result;
        }

        private async Task<bool> checkForGroup(int loggedUserId, PermissionEnum permission, int groupId)
        {
            var assignment = await _assignmentRepository.SingleOrDefaultAsync(x => x.UserId == loggedUserId && x.GroupId == groupId);
            return await _assignmentRepository.CheckIfAssignmentHasPermission(assignment.Id, permission);
        }
    }
}
