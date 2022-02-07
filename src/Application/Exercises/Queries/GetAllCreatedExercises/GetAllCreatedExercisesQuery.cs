using Application.Interfaces;
using Application.Groups;
using AutoMapper;
using Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Domain.Entities;
using Application.Common.Models;
using Application.Common.Models.ExtenstionMethods;
using Domain.Common;

namespace Application.Exercises.Queries.GetAllCreated
{
    public class GetAllCreatedExercisesQuery : IRequest<PaginatedList<GetExerciseDto>>
    {    
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }

    public class GetAllCreatedQueryHandler : IRequestHandler<GetAllCreatedExercisesQuery, PaginatedList<GetExerciseDto>>
    {
        private readonly IUserContextService _userContextService;
        private readonly IExerciseRepository _exerciseRepository;
        private readonly IMapper _mapper;
        private readonly ISolutionService _solutionService;

        public GetAllCreatedQueryHandler(IUserContextService userContextService, IExerciseRepository exerciseRepository
            ,IMapper mapper, ISolutionService solutionService)
        {
            _userContextService = userContextService;
            _exerciseRepository = exerciseRepository;
            _mapper = mapper;
            _solutionService = solutionService;
        }

        public async Task<PaginatedList<GetExerciseDto>> Handle(GetAllCreatedExercisesQuery request, CancellationToken cancellationToken)
        {
            int? loggedUserId = _userContextService.GetUserId;
            if (loggedUserId is null) { throw new UnauthorizedAccessException(); }

            var exercises = await _exerciseRepository
                .GetPaginatedResultAsync(x => x.CreatorId == (int)loggedUserId, request.PageNumber, request.PageSize, x => x.Creator);
            var exercisesDto = await exercises.MapPaginatedList<GetExerciseDto, Exercise>(_mapper);
            foreach (var item in exercisesDto.Items)
            {
                item.Passed = await _solutionService.CheckIfUserPassedExercise(item.Id);
            }
            return exercisesDto;
        }
    }
}
