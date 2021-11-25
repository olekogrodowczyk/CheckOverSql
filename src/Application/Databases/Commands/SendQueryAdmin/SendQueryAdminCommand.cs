using Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Databases.Commands.SendQueryAdmin
{
    public class SendQueryAdminCommand : IRequest<int>
    {
        public string Query { get; set; }
        public string Database { get; set; }
    }

    public class SendQueryAdminCommandHandler : IRequestHandler<SendQueryAdminCommand, int>
    {
        private readonly IDatabaseService _databaseService;

        public SendQueryAdminCommandHandler(IDatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public async Task<int> Handle(SendQueryAdminCommand command, CancellationToken cancellationToken)
        {
            var result = await _databaseService.SendQueryNoData(command.Query, command.Database, true);
            return result;
        }
    }
}
