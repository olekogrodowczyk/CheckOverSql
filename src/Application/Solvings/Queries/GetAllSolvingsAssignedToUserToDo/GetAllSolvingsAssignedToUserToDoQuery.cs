using AutoMapper;
using Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Groups.Queries.GetAllSolvingsAssignedToUserToDo
{
    public class GetAllSolvingsAssignedToUserToDoQuery : IRequest<IEnumerable<GetSolvingDto>>
    {
        public int UserId { get; set; }
    }

    public class GetAllSolvingsAssignedToUserToDoQueryHandler
        : IRequestHandler<GetAllSolvingsAssignedToUserToDoQuery, IEnumerable<GetSolvingDto>>
    {
        private readonly ISolvingRepository _solvingRepository;
        private readonly IMapper _mapper;

        public GetAllSolvingsAssignedToUserToDoQueryHandler(ISolvingRepository solvingRepository, IMapper mapper)
        {
            _solvingRepository = solvingRepository;
            _mapper = mapper;
        }
        public async Task<IEnumerable<GetSolvingDto>> Handle(GetAllSolvingsAssignedToUserToDoQuery request, CancellationToken cancellationToken)
        {
            var solvings = await _solvingRepository.GetSolvingsAssignedToUserToDo(request.UserId);
            var solvingsVm = _mapper.Map<IEnumerable<GetSolvingDto>>(solvings);
            return solvingsVm;
        }
    }
}
