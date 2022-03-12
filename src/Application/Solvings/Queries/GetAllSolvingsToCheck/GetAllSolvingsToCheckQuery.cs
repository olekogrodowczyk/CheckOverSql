using Application.Common.Exceptions;
using Application.Groups.Queries;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Solvings.Queries.GetAllSolvingsToCheck
{
    public class GetAllSolvingsToCheckQuery : IRequest<IEnumerable<GetSolvingDto>>
    {
        public int? GroupId { get; set; } = null;
    }

    public class GetAllSolvingsToCheckQueryHandler : IRequestHandler<GetAllSolvingsToCheckQuery, IEnumerable<GetSolvingDto>>
    {
        private readonly ISolvingRepository _solvingRepository;
        private readonly IMapper _mapper;
        private readonly IUserContextService _userContextService;
        private readonly IGroupRepository _groupRepository;

        public GetAllSolvingsToCheckQueryHandler(ISolvingRepository solvingRepository, IMapper mapper
            , IUserContextService userContextService, IGroupRepository groupRepository)
        {
            _solvingRepository = solvingRepository;
            _mapper = mapper;
            _userContextService = userContextService;
            _groupRepository = groupRepository;
        }

        public async Task<IEnumerable<GetSolvingDto>> Handle(GetAllSolvingsToCheckQuery request, CancellationToken cancellationToken)
        {
            int? loggedUserId = _userContextService.GetUserId;
            if (loggedUserId is null) { throw new UnauthorizedAccessException(); }
            await checkGroupExistance(request);

            var solvingsToCheck = await _solvingRepository.GetAllSolvingsToCheck((int)loggedUserId, request.GroupId);
            return _mapper.Map<IEnumerable<GetSolvingDto>>(solvingsToCheck);
        }

        private async Task checkGroupExistance(GetAllSolvingsToCheckQuery request)
        {
            if (request.GroupId is not null)
            {
                var group = await _groupRepository.GetByIdAsync((int)request.GroupId);
                if (group is null) { throw new NotFoundException(nameof(group), request.GroupId); }
            }
        }
    }
}
