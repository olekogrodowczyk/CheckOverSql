using Application.Common.QueryEvaluation.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.QueryEvaluation
{
    public class BodiesHandler : IEvaluationHandler
    {
        private readonly IQueryBuilder _queryBuilder;

        public BodiesHandler(IQueryBuilder queryBuilder)
        {
            _queryBuilder = queryBuilder;
        }

        public async Task Handle(QueryEvaluationData data)
        {
            data.Phase = QueryEvaluationPhase.Bodies;
            string queryToCheck1 = _queryBuilder.SetInitQuery(data.Query1).HandleSpaces().GetResult();
            string queryToCheck2 = _queryBuilder.SetInitQuery(data.Query2).HandleSpaces().GetResult();
            bool result = queryToCheck1.Equals(queryToCheck2, StringComparison.OrdinalIgnoreCase);
            if (result) { data.Stop = true; data.FinalResult = true; }
            await Task.CompletedTask;
        }
    }
}
