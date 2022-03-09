using Application.Interfaces;
using AutoMapper;
using Domain.Enums;
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
    public class GetAllSolvingsAssignedToUserByStatusQuery : IRequest<IEnumerable<GetSolvingDto>>
    {
        public string Status { get; set; }
    }

    public class GetAllSolvingsAssignedToUserByStatusQueryHandler
        : IRequestHandler<GetAllSolvingsAssignedToUserByStatusQuery, IEnumerable<GetSolvingDto>>
    {
        private readonly ISolvingRepository _solvingRepository;
        private readonly IMapper _mapper;
        private readonly IUserContextService _userContextService;

        public GetAllSolvingsAssignedToUserByStatusQueryHandler(ISolvingRepository solvingRepository, IMapper mapper
            , IUserContextService userContextService)
        {
            _solvingRepository = solvingRepository;
            _mapper = mapper;
            _userContextService = userContextService;
        }
        public async Task<IEnumerable<GetSolvingDto>> Handle(GetAllSolvingsAssignedToUserByStatusQuery request, CancellationToken cancellationToken)
        {
            int? loggedUserId = _userContextService.GetUserId;
            if (loggedUserId is null) { throw new UnauthorizedAccessException(); }

            Enum.TryParse(request.Status, out SolvingStatusEnum status);

            var solvings = await _solvingRepository.GetAllSolvingsAssignedToUser((int)loggedUserId, status);
            var solvingsVm = _mapper.Map<IEnumerable<GetSolvingDto>>(solvings);
            return solvingsVm;
        }
    }
}
