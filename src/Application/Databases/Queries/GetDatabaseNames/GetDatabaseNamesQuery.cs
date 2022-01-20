using Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Databases.Queries.GetDatabaseNames
{
    public class GetDatabaseNamesQuery : IRequest<IEnumerable<string>>
    {
    }

    public class GetAllCreatedQueryHandler : IRequestHandler<GetDatabaseNamesQuery, IEnumerable<string>>
    {
        private readonly IDatabaseRepository _databaseRepository;

        public GetAllCreatedQueryHandler(IDatabaseRepository databaseRepository)
        {
            _databaseRepository = databaseRepository;
        }

        public async Task<IEnumerable<string>> Handle(GetDatabaseNamesQuery request, CancellationToken cancellationToken)
        {
            var databaseNames = (await _databaseRepository.GetAllAsync()).Select(x=>x.Name).ToList();
            return databaseNames;
        }
    }


}
