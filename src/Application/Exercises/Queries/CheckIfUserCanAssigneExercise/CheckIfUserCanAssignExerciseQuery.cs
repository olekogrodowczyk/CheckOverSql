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

namespace Application.Exercises.Queries.CheckIfUserCanAssigneExercise
{
    public class CheckIfUserCanAssignExerciseQuery : IRequest<bool>
    {
    }

    public class CheckIfUserCanAssignExerciseQueryHandler : IRequestHandler<CheckIfUserCanAssignExerciseQuery, bool>
    {
        private readonly IAssignmentRepository _assignmentRepository;
        private readonly IUserContextService _userContextService;

        public CheckIfUserCanAssignExerciseQueryHandler(IAssignmentRepository assignmentRepository
            ,IUserContextService userContextService)
        {
            _assignmentRepository = assignmentRepository;
            _userContextService = userContextService;
        }

        public async Task<bool> Handle(CheckIfUserCanAssignExerciseQuery request, CancellationToken cancellationToken)
        {
            int? loggedUserId = _userContextService.GetUserId;
            if(loggedUserId is null) { throw new UnauthorizedAccessException(); }

            var assignments = (await _assignmentRepository.GetWhereAsync(x => x.UserId == (int)loggedUserId)).ToList();
            bool result = false;
            foreach(var assignment in assignments)
            {
                if (await _assignmentRepository.CheckIfAssignmentHasPermission
                    (assignment.Id, GetPermissionByEnum.GetPermissionName(PermissionNames.AssigningExercises)))
                {
                    result = true;
                }
            }
            return result;
        }
    }
}
