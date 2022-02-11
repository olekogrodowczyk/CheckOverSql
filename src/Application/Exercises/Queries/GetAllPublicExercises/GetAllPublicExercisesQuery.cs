using Application.Common.Models;
using Application.Common.Models.ExtenstionMethods;
using Application.Groups;
using Application.Interfaces;
using AutoMapper;
using Domain.Common;
using Domain.Entities;
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
    public class GetAllPublicExercisesQuery : IRequest<PaginatedList<GetExerciseDto>>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }

    public class GetAllPublicExercisesQueryHandler : IRequestHandler<GetAllPublicExercisesQuery, PaginatedList<GetExerciseDto>>
    {
        private readonly IMapper _mapper;
        private readonly IExerciseRepository _exerciseRepository;
        private readonly IUserContextService _userContextService;
        private readonly ISolutionService _solutionService;

        public GetAllPublicExercisesQueryHandler(IMapper mapper, IExerciseRepository exerciseRepository
            , IUserContextService userContextService, ISolutionService solutionService)
        {
            _mapper = mapper;
            _exerciseRepository = exerciseRepository;
            _userContextService = userContextService;
            _solutionService = solutionService;
        }

        public async Task<PaginatedList<GetExerciseDto>> Handle(GetAllPublicExercisesQuery request, CancellationToken cancellationToken)
        {
            var exercises = await _exerciseRepository
                .GetPaginatedResultAsync(x => !x.IsPrivate && x.DatabaseId != null, request.PageNumber, request.PageSize, x => x.Creator, x=>x.Database);
            var exercisesDto = await exercises.MapPaginatedList<GetExerciseDto, Exercise>(_mapper);
            foreach (var item in exercisesDto.Items)
            {
                item.Passed = await _solutionService.CheckIfUserPassedExercise(item.Id);
                item.LastAnswer = await _solutionService.GetLastExecutedQueryByUserInExercise(item.Id);
            }
            return exercisesDto;
        }
    }
}
