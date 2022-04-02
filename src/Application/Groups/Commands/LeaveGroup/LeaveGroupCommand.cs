using Application.Common.Exceptions;
using Application.Interfaces;
using Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Groups.Commands.LeaveGroup
{
    public class LeaveGroupCommand : IRequest<Unit>
    {
        public int GroupId { get; set; }
    }

    public class LeaveGroupCommandHandler : IRequestHandler<LeaveGroupCommand, Unit>
    {
        private readonly IUserContextService _userContextService;
        private readonly IAssignmentRepository _assignmentRepository;
        private readonly IGroupRepository _groupRepository;

        public LeaveGroupCommandHandler(IUserContextService userContextService, IAssignmentRepository assignmentRepository
            , IGroupRepository groupRepository)
        {
            _userContextService = userContextService;
            _assignmentRepository = assignmentRepository;
            _groupRepository = groupRepository;
        }

        public async Task<Unit> Handle(LeaveGroupCommand request, CancellationToken cancellationToken)
        {
            int? loggedUserId = _userContextService.GetUserId;
            if (loggedUserId is null) { throw new UnauthorizedAccessException(); }

            var group = await _groupRepository.GetByIdAsync(request.GroupId);
            if (group is null) { throw new NotFoundException(nameof(group), request.GroupId); }

            var assignment = await _assignmentRepository
                .SingleOrDefaultAsync(x => x.UserId == (int)loggedUserId && x.GroupId == request.GroupId);
            if (assignment is null)
            { throw new NotFoundException($"Assignment cannot be found with user: {(int)loggedUserId} and group: {request.GroupId}"); }

            if (group.CreatorId == (int)loggedUserId)
            { throw new ForbidException("You cannot leave your group while being a creator", true); }

            await _assignmentRepository.DeleteAsync(assignment.Id);
            return Unit.Value;
        }
    }
}
