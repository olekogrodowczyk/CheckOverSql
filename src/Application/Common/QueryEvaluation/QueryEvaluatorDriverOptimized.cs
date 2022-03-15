using Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.QueryEvaluation
{
    public class QueryEvaluatorDriverOptimized : IQueryEvaluatorDriver
    {
        private readonly IQueryEvaluator _queryEvaluator;
        private readonly IDataComparerService _dataComparerService;
        private readonly IQueryBuilder _queryBuilder;
        private int query1Count = 0;
        private int query2Count = 0;

        public QueryEvaluatorDriverOptimized(IQueryEvaluator queryEvaluator, IDataComparerService dataComparerService
            , IQueryBuilder queryBuilder)
        {
            _queryEvaluator = queryEvaluator;
            _dataComparerService = dataComparerService;
            _queryBuilder = queryBuilder;
        }

        public async Task<bool> Evaluate(string query1, string query2, string connectionString)
        {
            _queryEvaluator.InitConnectionString(connectionString);

            if (compareBodies(query1, query2)) { return true; }
            if (!(await compareColumnNames(query1, query2))) { return false; }
            if (!(await compareQueriesCount(query1, query2))) { return false; }
            if (!(await compareFirstMiddleLastRows(query1, query2))) { return false; }
            return true;
        }

        private bool compareBodies(string query1, string query2)
        {
            string queryToCheck1 = _queryBuilder.SetInitQuery(query1).HandleSpaces().GetResult();
            string queryToCheck2 = _queryBuilder.SetInitQuery(query2).HandleSpaces().GetResult();
            return queryToCheck1.Equals(queryToCheck2);
        }

        private async Task<bool> compareColumnNames(string query1, string query2)
        {
            bool result = await _queryEvaluator.CompareColumnNames(query1, query2);
            return result;
        }

        private async Task<bool> compareFirstMiddleLastRows(string query1, string query2)
        {
            if (query1Count < 1 || query2Count < 1) { return false; }
            var matrix1 = await _queryEvaluator.GetFirstMiddleLastRows(query1, query1Count);
            var matrix2 = await _queryEvaluator.GetFirstMiddleLastRows(query2, query2Count);
            return await _dataComparerService.compareValues(matrix1, matrix2);
        }

        private async Task<bool> compareQueriesCount(string query1, string query2)
        {
            query1Count = await _queryEvaluator.GetCountOfQuery(query1);
            query2Count = await _queryEvaluator.GetCountOfQuery(query2);
            if (query1Count != query2Count) { return false; }
            int intersectedQueryCount = await _queryEvaluator.GetIntersectQueryCount(query1, query2);
            if (query1Count != intersectedQueryCount) { return false; }
            return true;
        }


    }
}
