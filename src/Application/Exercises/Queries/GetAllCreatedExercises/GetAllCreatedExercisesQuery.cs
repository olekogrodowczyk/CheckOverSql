using Application.Interfaces;
using Application.Solvings;
using AutoMapper;
using Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Exercises.Queries.GetAllCreated
{
    public class GetAllCreatedExercisesQuery : IRequest<IEnumerable<GetExerciseDto>>
    {
        public int UserId { get; set; }
    }

    public class GetAllCreatedQueryHandler : IRequestHandler<GetAllCreatedExercisesQuery, IEnumerable<GetExerciseDto>>
    {
        private readonly IUserContextService _userContextService;
        private readonly IExerciseRepository _exerciseRepository;
        private readonly IMapper _mapper;

        public GetAllCreatedQueryHandler(IUserContextService userContextService, IExerciseRepository exerciseRepository
            ,IMapper mapper)
        {
            _userContextService = userContextService;
            _exerciseRepository = exerciseRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<GetExerciseDto>> Handle(GetAllCreatedExercisesQuery request, CancellationToken cancellationToken)
        {
            int loggedUserId = (int)_userContextService.GetUserId;
            var exercises = await _exerciseRepository.GetWhereAsync(x => x.CreatorId == loggedUserId, x => x.Creator);
            var exerciseDtos = _mapper.Map<IEnumerable<GetExerciseDto>>(exercises);
            return exerciseDtos;
        }
    }
}
