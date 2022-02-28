using Application.Interfaces;
using AutoMapper;
using Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Groups.Queries.GetAllSolvingsAssignedToUser
{
    public class GetAllSolvingsAssignedToUserQuery : IRequest<IEnumerable<GetSolvingDto>>
    {
    }

    public class GetAllSolvingsAssignedToUserQueryHandler : IRequestHandler<GetAllSolvingsAssignedToUserQuery, IEnumerable<GetSolvingDto>>
    {
        private readonly IMapper _mapper;
        private readonly ISolvingRepository _solvingRepository;
        private readonly IUserContextService _userContextService;
        private readonly ISolutionService _solutionService;

        public GetAllSolvingsAssignedToUserQueryHandler(IMapper mapper, ISolvingRepository solvingRepository
            ,IUserContextService userContextService, ISolutionService solutionService)
        {
            _mapper = mapper;
            _solvingRepository = solvingRepository;
            _userContextService = userContextService;
            _solutionService = solutionService;
        }

        public async Task<IEnumerable<GetSolvingDto>> Handle(GetAllSolvingsAssignedToUserQuery request, CancellationToken cancellationToken)
        {
            int? loggedUserId = _userContextService.GetUserId;
            if(loggedUserId is null) { throw new UnauthorizedAccessException(); }
            var solvings = await _solvingRepository.GetAllSolvingsAssignedToUser((int)loggedUserId);            
            var solvingsDtos = _mapper.Map<IEnumerable<GetSolvingDto>>(solvings);
            foreach (var item in solvingsDtos.Select(x => x.Exercise))
            {
                item.Passed = await _solutionService.CheckIfUserPassedExercise(item.Id);
                item.LastAnswer = await _solutionService.GetLatestSolutionQuerySentIntoExercise(item.Id);
            }
            return solvingsDtos;
        }
    }
}
