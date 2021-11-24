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

namespace Application.Solutions.Queries.GetAllSolutionsCreatedByUser
{
    public class GetAllSolutionsCreatedByUserQuery : IRequest<IEnumerable<GetSolutionDto>>
    {
        public int UserId { get; set; }
    }

    public class GetAllSolutionsCreatedByUserQueryHandler : IRequestHandler<GetAllSolutionsCreatedByUserQuery, IEnumerable<GetSolutionDto>>
    {
        private readonly ISolutionRepository _solutionRepository;
        private readonly IMapper _mapper;

        public GetAllSolutionsCreatedByUserQueryHandler(ISolutionRepository solutionRepository, IMapper mapper)
        {
            _solutionRepository = solutionRepository;
            _mapper = mapper;
        }
        public async Task<IEnumerable<GetSolutionDto>> Handle(GetAllSolutionsCreatedByUserQuery request, CancellationToken cancellationToken)
        {
            var solutions = await _solutionRepository.GetWhereAsync(x => x.CreatorId == request.UserId, x => x.Exercise);
            var solutionDtos = _mapper.Map<IEnumerable<GetSolutionDto>>(solutions);
            return solutionDtos;
        }
    }
}
