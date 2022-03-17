using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.QueryEvaluation.Handlers
{
    public class ColumnsHandler : IEvaluationHandler
    {
        private readonly IQueryEvaluatorService _queryEvaluatorService;

        public ColumnsHandler(IQueryEvaluatorService queryEvaluatorService)
        {
            _queryEvaluatorService = queryEvaluatorService;
        }

        public async Task Handle(QueryEvaluationData data)
        {
            data.Phase = QueryEvaluationPhase.Columns;
            bool result = await _queryEvaluatorService.CompareColumnNames(data.Query1, data.Query2);
            if (!result) { data.FinalResult = false; data.Stop = true; }
        }
    }
}
