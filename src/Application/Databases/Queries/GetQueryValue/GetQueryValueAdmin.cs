using Application.Databases.Queries.GetDatabaseNames;
using Application.Interfaces;
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
    public class GetQueryValueAdmin : IRequest<IEnumerable<IEnumerable<string>>>
    {
        public string DatabaseName { get; set; }
        public string Query { get; set; }
    }

    public class GetQueryValueAdminQueryHandler : IRequestHandler<GetQueryValueAdmin, IEnumerable<IEnumerable<string>>>
    {
        private readonly IDatabaseRepository _databaseRepository;
        private readonly IDatabaseService _databaseService;

        public GetQueryValueAdminQueryHandler(IDatabaseRepository databaseRepository, IDatabaseService databaseService)
        {
            _databaseRepository = databaseRepository;
            _databaseService = databaseService;
        }

        public async Task<IEnumerable<IEnumerable<string>>> Handle(GetQueryValueAdmin request, CancellationToken cancellationToken)
        {
            var result = await _databaseService.SendQueryWithData(request.Query, request.DatabaseName, false);
            return result;
        }
    }
}
