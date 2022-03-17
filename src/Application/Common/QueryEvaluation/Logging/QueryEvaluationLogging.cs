using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.QueryEvaluation.Logging
{
    public class QueryEvaluationLogging : IQueryEvaluationLogging
    {
        private readonly ILogger<QueryEvaluationLogging> _logger;

        public QueryEvaluationLogging(ILogger<QueryEvaluationLogging> logger)
        {
            _logger = logger;
        }

        public void Log(QueryEvaluationData data)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"\n\nComparing queries results:");
            sb.AppendLine($"User who invoked checking queries: {data.InvokingUserId}");
            sb.AppendLine($"Query1: {data.Query1}");
            sb.AppendLine($"Query2: {data.Query2}");
            sb.AppendLine($"Phase: {data.Phase.ToString()}");
            if (data.Query1Count is not null) { sb.AppendLine($"Count of rows of the first query: {data.Query1Count}"); }
            if (data.Query2Count is not null) { sb.AppendLine($"Count of rows of the second query: {data.Query2Count}"); }
            if (data.IntersectCount is not null) { sb.AppendLine($"Count of rows intersected query: {data.IntersectCount}"); }
            sb.AppendLine($"Passed: {data.FinalResult.ToString()}");

            _logger.LogInformation(sb.ToString());
        }
    }
}
