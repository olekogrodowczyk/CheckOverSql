using Application.Common.Exceptions;
using Application.Interfaces;
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

namespace Application.Exercises.Queries.GetExerciseById
{
    public class GetExerciseByIdQuery : IRequest<GetExerciseDto>
    {
        public int ExerciseId { get; set; }
    }

    public class GetExerciseByIdQueryHandler : IRequestHandler<GetExerciseByIdQuery, GetExerciseDto>
    {
        private readonly IExerciseRepository _exerciseRepository;
        private readonly IUserContextService _userContextService;
        private readonly IMapper _mapper;
        private readonly ISolutionService _solutionService;

        public GetExerciseByIdQueryHandler(IExerciseRepository exerciseRepository, IUserContextService userContextService,
            IMapper mapper, ISolutionService solutionService)
        {
            _exerciseRepository = exerciseRepository;
            _userContextService = userContextService;
            _mapper = mapper;
            _solutionService = solutionService;
        }

        public async Task<GetExerciseDto> Handle(GetExerciseByIdQuery request, CancellationToken cancellationToken)
        {
            int? loggedUserId = _userContextService.GetUserId;

            var exercise = await _exerciseRepository.GetByIdAsync(request.ExerciseId);
            if (exercise is null) { throw new NotFoundException(nameof(exercise), request.ExerciseId); }

            validate(exercise);

            var result = _mapper.Map<GetExerciseDto>(exercise);

            result.Passed = await _solutionService.CheckIfUserPassedExercise(exercise.Id);
            result.LastAnswer = await _solutionService.GetLatestSolutionQuerySentIntoExercise(exercise.Id);

            return result;
        }

        private void validate(Exercise exercise)
        {
            int? loggedUserId = _userContextService.GetUserId;
            if (loggedUserId is null) { throw new UnauthorizedAccessException(); }

            if (exercise.IsPrivate && exercise.CreatorId != (int)loggedUserId)
            {
                throw new ForbidException("You don't have access to this exercise", true);
            }
        }
    }
}
