using Application.Databases.Queries.GetDatabaseNames;
using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Databases.Queries.GetQueryValueAdmin
{
    public class GetQueryValueQuery : IRequest<IEnumerable<IEnumerable<string>>>
    {
        public string DatabaseName { get; set; }
        public string Query { get; set; }
        public bool ToQueryHistory { get; set; } = false;
    }

    public class GetQueryValueAdminQueryHandler : IRequestHandler<GetQueryValueQuery, IEnumerable<IEnumerable<string>>>
    {
        private readonly IDatabaseRepository _databaseRepository;
        private readonly IDatabaseService _databaseService;
        private readonly IUserContextService _userContextService;
        private readonly IRepository<Query> _queryRepository;

        public GetQueryValueAdminQueryHandler(IDatabaseRepository databaseRepository, IDatabaseService databaseService
            ,IUserContextService userContextService, IRepository<Query> queryRepository)
        {
            _databaseRepository = databaseRepository;
            _databaseService = databaseService;
            _userContextService = userContextService;
            _queryRepository = queryRepository;
        }

        public async Task<IEnumerable<IEnumerable<string>>> Handle(GetQueryValueQuery request, CancellationToken cancellationToken)
        {
            int? loggedUserId = _userContextService.GetUserId;
            if(loggedUserId is null) { throw new UnauthorizedAccessException(); }
            int databaseId = await _databaseRepository.GetDatabaseIdByName(request.DatabaseName);

            var result = await _databaseService.SendQueryWithData(request.Query, request.DatabaseName, 1000);
            if(request.ToQueryHistory)
            {
                Query query = new Query { CreatorId = (int)loggedUserId, DatabaseId = databaseId, QueryValue = request.Query };
                await _queryRepository.AddAsync(query);
            }            
            return result;
        }
    }
}
