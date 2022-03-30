using Application.Groups;
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

namespace Application.Solutions.Queries.GetAllSolutionsCreatedByUser
{
    public class GetAllSolutionsCreatedByUserQuery : IRequest<IEnumerable<GetSolutionDto>>
    {
    }

    public class GetAllSolutionsCreatedByUserQueryHandler : IRequestHandler<GetAllSolutionsCreatedByUserQuery, IEnumerable<GetSolutionDto>>
    {
        private readonly ISolutionRepository _solutionRepository;
        private readonly IMapper _mapper;
        private readonly IUserContextService _userContextService;

        public GetAllSolutionsCreatedByUserQueryHandler(ISolutionRepository solutionRepository, IMapper mapper,
            IUserContextService userContextService)
        {
            _solutionRepository = solutionRepository;
            _mapper = mapper;
            _userContextService = userContextService;
        }
        public async Task<IEnumerable<GetSolutionDto>> Handle(GetAllSolutionsCreatedByUserQuery request, CancellationToken cancellationToken)
        {
            int? loggedUserId = _userContextService.GetUserId;
            if (loggedUserId is null) { throw new UnauthorizedAccessException(); }

            var solutions = await _solutionRepository.GetWhereAsync(x => x.CreatorId == loggedUserId, x => x.Exercise, x => x.Creator);
            var solutionDtos = _mapper.Map<IEnumerable<GetSolutionDto>>(solutions);
            return solutionDtos;
        }
    }
}
