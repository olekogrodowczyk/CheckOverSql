using Application.Interfaces;
using Application.Solutions.Commands.CreateSolution;
using AutoMapper;
using Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Solutions.Queries.GetLastQueryInExercise
{
    public class GetLastSolutionSentIntoExerciseQuery : IRequest<GetSolutionDto>
    {
        public int ExerciseId { get; set; }
    }

    public class GetLastSolutionInExerciseQueryHandler : IRequestHandler<GetLastSolutionSentIntoExerciseQuery, GetSolutionDto>
    {
        private readonly IMapper _mapper;
        private readonly ISolutionRepository _solutionRepository;
        private readonly IUserContextService _userContextService;

        public GetLastSolutionInExerciseQueryHandler(IMapper mapper, ISolutionRepository solutionRepository, IUserContextService userContextService)
        {
            _mapper = mapper;
            _solutionRepository = solutionRepository;
            _userContextService = userContextService;
        }

        public async Task<GetSolutionDto> Handle(GetLastSolutionSentIntoExerciseQuery request, CancellationToken cancellationToken)
        {
            int? loggedUserId = _userContextService.GetUserId;
            if(loggedUserId is null) { throw new UnauthorizedAccessException(); }
            var solution = await _solutionRepository
                .GetLatestSolutionSentByUserInExercise(request.ExerciseId, (int)loggedUserId);
            return _mapper.Map<GetSolutionDto>(solution);            
        }
    }
}
