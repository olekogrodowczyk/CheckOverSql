using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.QueryEvaluation.Handlers
{
    public class IntersectHandler : IEvaluationHandler
    {
        private readonly IQueryEvaluatorService _queryEvaluatorService;

        public IntersectHandler(IQueryEvaluatorService queryEvaluatorService)
        {
            _queryEvaluatorService = queryEvaluatorService;
        }

        public async Task Handle(QueryEvaluationData data)
        {
            data.Phase = QueryEvaluationPhase.IntersectedCount;
            data.IntersectCount = await _queryEvaluatorService.GetIntersectQueryCount(data.Query1, data.Query2);
            if (data.Query1Count != data.IntersectCount) { data.FinalResult = false; }
            else { data.FinalResult = true; }
        }
    }
}
