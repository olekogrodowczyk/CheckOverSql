using Application.Interfaces;
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

namespace Application.Dto.AssignExerciseToUsersTo
{
    public class AssignExerciseToUsersCommand : IRequest<IEnumerable<int>>
    {
        public int Id { get; set; }
        public int GroupId { get; set; }
        public DateTime DeadLine { get; set; }    
    }

    public class AssignExerciseToUsersCommandHandler : IRequestHandler<AssignExerciseToUsersCommand, IEnumerable<int>>
    {
        private readonly IAssignmentRepository _assignmentRepository;
        private readonly IUserContextService _userContextService;
        private readonly ISolvingRepository _solvingRepository;

        public AssignExerciseToUsersCommandHandler(IAssignmentRepository assignmentRepository
            , IUserContextService userContextService, ISolvingRepository solvingRepository)
        {
            _assignmentRepository = assignmentRepository;
            _userContextService = userContextService;
            _solvingRepository = solvingRepository;
        }

        public async Task<IEnumerable<int>> Handle(AssignExerciseToUsersCommand request, CancellationToken cancellationToken)
        {
            var assignmentsChosenToGetExercise =
                await _assignmentRepository
                .GetWhereAsync(x => x.GroupId == request.GroupId && x.GroupRole.Name == "User", x => x.GroupRole);

            var solvingsIds = new List<int>();
            foreach (Assignment assignment in assignmentsChosenToGetExercise)
            {
                var solving = new Solving
                {
                    ExerciseId = request.Id,
                    Status = SolvingStatus.ToDo.ToString(),
                    DeadLine = request.DeadLine,
                    AssignmentId = assignment.Id,
                    CreatorId = (int)_userContextService.GetUserId
                };
                await _solvingRepository.AddAsync(solving);
                solvingsIds.Add(solving.Id);
            }
            return solvingsIds;
        }
    }
}
