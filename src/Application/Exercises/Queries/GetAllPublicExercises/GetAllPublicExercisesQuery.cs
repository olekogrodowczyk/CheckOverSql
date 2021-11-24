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

namespace Application.Exercises.Queries.GetAllPublicExercises
{
    public class GetAllPublicExercisesQuery : IRequest<IEnumerable<GetExerciseDto>>
    {
    }

    public class GetAllPublicExercisesQueryHandler : IRequestHandler<GetAllPublicExercisesQuery, IEnumerable<GetExerciseDto>>
    {
        private readonly IMapper _mapper;
        private readonly IExerciseRepository _exerciseRepository;

        public GetAllPublicExercisesQueryHandler(IMapper mapper, IExerciseRepository exerciseRepository )
        {
            _mapper = mapper;
            _exerciseRepository = exerciseRepository;
        }

        public async Task<IEnumerable<GetExerciseDto>> Handle(GetAllPublicExercisesQuery request, CancellationToken cancellationToken)
        {
            var exercises = await _exerciseRepository.GetWhereAsync(x => !x.IsPrivate);
            var exerciseDtos = _mapper.Map<IEnumerable<GetExerciseDto>>(exercises);
            return exerciseDtos;
        }
    }
}
