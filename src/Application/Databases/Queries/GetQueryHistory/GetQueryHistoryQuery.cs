using Application.Common.Models.ExtenstionMethods;
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

namespace Application.Databases.Queries.GetQueryHistory
{
    public class GetQueryHistoryQuery : IRequest<PaginatedList<QueryHistoryDto>>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }

    public class GetQueryHistoryQueryHandler : IRequestHandler<GetQueryHistoryQuery, PaginatedList<QueryHistoryDto>>
    {
        private readonly IRepository<Query> _queryRepository;
        private readonly IUserContextService _userContextService;
        private readonly IMapper _mapper;

        public GetQueryHistoryQueryHandler(IRepository<Query> repository, IUserContextService userContextService
            ,IMapper mapper)
        {
            _queryRepository = repository;
            _userContextService = userContextService;
            _mapper = mapper;
        }

        public async Task<PaginatedList<QueryHistoryDto>> Handle(GetQueryHistoryQuery request, CancellationToken cancellationToken)
        {
            int? loggedUserId = _userContextService.GetUserId;
            if (loggedUserId is null) { throw new UnauthorizedAccessException(); }
            var queriesCreatedByUser = await _queryRepository
                .GetPaginatedResultAsync(x => x.CreatorId == (int)loggedUserId, request.PageNumber, request.PageSize);
            return await queriesCreatedByUser.MapPaginatedList<QueryHistoryDto, Query>(_mapper);
        }
    }
}
