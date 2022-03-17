using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.QueryEvaluation.Handlers
{
    public class CountsHandler : IEvaluationHandler
    {
        private readonly IQueryEvaluatorService _queryEvaluatorService;

        public CountsHandler(IQueryEvaluatorService queryEvaluatorService)
        {
            _queryEvaluatorService = queryEvaluatorService;
        }

        public async Task Handle(QueryEvaluationData data)
        {
            data.Phase = QueryEvaluationPhase.QueriesCounts;
            data.Query1Count = await _queryEvaluatorService.GetCountOfQuery(data.Query1);
            data.Query2Count = await _queryEvaluatorService.GetCountOfQuery(data.Query2);
            if (data.Query1Count != data.Query2Count) { data.Stop = true; data.FinalResult = false; }
        }
    }
}
