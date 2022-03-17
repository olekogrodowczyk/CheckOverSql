using Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.QueryEvaluation.Handlers
{
    public class FirstMiddleLastRowsHandler : IEvaluationHandler
    {
        private readonly IQueryEvaluatorService _queryEvaluatorService;
        private readonly IDataComparerService _dataComparerService;

        public FirstMiddleLastRowsHandler(IQueryEvaluatorService queryEvaluatorService, IDataComparerService dataComparerService)
        {
            _queryEvaluatorService = queryEvaluatorService;
            _dataComparerService = dataComparerService;
        }

        public async Task Handle(QueryEvaluationData data)
        {
            data.Phase = QueryEvaluationPhase.FirstMiddleLastRows;
            if (data.Query1Count < 1 || data.Query2Count < 1) { data.Stop = true; return; }
            if (data.Query1Count is null || data.Query2Count is null) { data.Stop = true; return; }

            var matrix1 = await _queryEvaluatorService.GetFirstMiddleLastRows(data.Query1, (int)data.Query1Count);
            var matrix2 = await _queryEvaluatorService.GetFirstMiddleLastRows(data.Query2, (int)data.Query2Count);
            bool result = await _dataComparerService.compareValues(matrix1, matrix2);

            if (!result) { data.Stop = true; data.FinalResult = false; }
            else { data.Stop = true; data.FinalResult = true; }
        }
    }
}
