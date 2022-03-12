using Application.Common.Exceptions;
using Application.Interfaces;
using Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Solutions.Commands.SendSolutionQuery
{
    public class SendSolutionQueryCommand : IRequest<IEnumerable<IEnumerable<string>>>
    {
        public int SolutionId { get; set; }
        public int ExerciseId { get; set; }
    }

    public class SendSolutionQueryCommandHandler : IRequestHandler<SendSolutionQueryCommand, IEnumerable<IEnumerable<string>>>
    {
        private readonly ISolutionRepository _solutionRepository;
        private readonly IDatabaseService _databaseService;

        public SendSolutionQueryCommandHandler(ISolutionRepository solutionRepository, IDatabaseService databaseService)
        {
            _solutionRepository = solutionRepository;
            _databaseService = databaseService;
        }

        public async Task<IEnumerable<IEnumerable<string>>> Handle(SendSolutionQueryCommand command, CancellationToken cancellationToken)
        {
            var solution = await _solutionRepository.GetByIdAsync(command.SolutionId);
            if (solution is null) { throw new NotFoundException(nameof(solution), command.SolutionId); }
            string databaseName = await _solutionRepository.GetDatabaseName(command.SolutionId);
            var result = await _databaseService.SendQueryWithData(solution.Query, databaseName);
            return result;
        }
    }
}
