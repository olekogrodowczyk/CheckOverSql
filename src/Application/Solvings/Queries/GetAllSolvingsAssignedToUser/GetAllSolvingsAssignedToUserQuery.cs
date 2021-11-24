using AutoMapper;
using Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Solvings.Queries.GetAllSolvingsAssignedToUser
{
    public class GetAllSolvingsAssignedToUserQuery : IRequest<IEnumerable<GetSolvingDto>>
    {
        public int UserId { get; set; }
    }

    public class GetAllSolvingsAssignedToUserQueryHandler : IRequestHandler<GetAllSolvingsAssignedToUserQuery, IEnumerable<GetSolvingDto>>
    {
        private readonly IMapper _mapper;
        private readonly ISolvingRepository _solvingRepository;

        public GetAllSolvingsAssignedToUserQueryHandler(IMapper mapper, ISolvingRepository solvingRepository)
        {
            _mapper = mapper;
            _solvingRepository = solvingRepository;
        }

        public async Task<IEnumerable<GetSolvingDto>> Handle(GetAllSolvingsAssignedToUserQuery request, CancellationToken cancellationToken)
        {
            var solvings = await _solvingRepository.GetAllSolvingsAssignedToUser(request.UserId);
            var solvingsVm = _mapper.Map<IEnumerable<GetSolvingDto>>(solvings);
            return solvingsVm;
        }
    }
}
