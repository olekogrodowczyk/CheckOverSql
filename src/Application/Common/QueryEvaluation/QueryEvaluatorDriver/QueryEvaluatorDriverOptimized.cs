using Application.Common.QueryEvaluation.Handlers;
using Application.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.QueryEvaluation
{
    public enum QueryEvaluationPhase
    {
        Bodies = 1,
        Columns,
        QueriesCounts,
        IntersectedCount,
        FirstMiddleLastRows
    }

    public class QueryEvaluatorDriverOptimized : IQueryEvaluatorDriver
    {
        private readonly IUserContextService _userContextService;
        private readonly IConnectionStringService _connectionStringService;
        private readonly IEnumerable<IEvaluationHandler> _handlers;

        public QueryEvaluatorDriverOptimized(IUserContextService userContextService,
             IConnectionStringService connectionStringService, IEnumerable<IEvaluationHandler> handlers)
        {
            _userContextService = userContextService;
            _connectionStringService = connectionStringService;
            _handlers = handlers;
        }

        public async Task<bool> Evaluate(string query1, string query2, string connectionString)
        {
            _connectionStringService.ConnectionString = connectionString;
            int? loggedUserId = _userContextService.GetUserId;
            if (loggedUserId is null) { throw new UnauthorizedAccessException(); }

            QueryEvaluationData data = new QueryEvaluationData()
            {
                InvokingUserId = (int)loggedUserId,
                Query1 = query1,
                Query2 = query2,
            };
            foreach (var handler in _handlers)
            {
                await handler.Handle(data);
                if (data.Stop) { break; }
            }


            return data.FinalResult;
        }


    }
}
