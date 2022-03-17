using Application.Interfaces;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.QueryEvaluation
{
    public class QueryEvaluator : IQueryEvaluator
    {
        private readonly IDatabaseQuery _databaseQuery;
        private readonly IQueryBuilder _queryBuilder;
        private readonly IQueryEvaluatorService _queryEvaluatorService;
        private readonly IConnectionStringService _connectionStringService;
        private readonly IDataComparerService _dataComparerService;
        private int query1Count;
        private int query2Count;

        public QueryEvaluator(IDatabaseQuery databaseQuery, IQueryBuilder queryBuilder
            , IQueryEvaluatorService queryEvaluatorService, IConnectionStringService connectionStringService
            , IDataComparerService dataComparerService)
        {
            _databaseQuery = databaseQuery;
            _queryBuilder = queryBuilder;
            _queryEvaluatorService = queryEvaluatorService;
            _connectionStringService = connectionStringService;
            _dataComparerService = dataComparerService;
        }

        public bool CompareBodies(string query1, string query2)
        {
            string queryToCheck1 = _queryBuilder.SetInitQuery(query1).HandleSpaces().GetResult();
            string queryToCheck2 = _queryBuilder.SetInitQuery(query2).HandleSpaces().GetResult();
            return queryToCheck1.Equals(queryToCheck2, StringComparison.OrdinalIgnoreCase);
        }

        public async Task<bool> CompareColumnNames(string query1, string query2)
        {
            bool result = await _queryEvaluatorService.CompareColumnNames(query1, query2);
            return result;
        }

        public async Task<bool> CompareFirstMiddleLastRows(string query1, string query2)
        {
            if (query1Count < 1 || query2Count < 1) { return false; }
            var matrix1 = await _queryEvaluatorService.GetFirstMiddleLastRows(query1, query1Count);
            var matrix2 = await _queryEvaluatorService.GetFirstMiddleLastRows(query2, query2Count);
            return await _dataComparerService.compareValues(matrix1, matrix2);
        }

        public async Task<bool> CompareQueriesCount(string query1, string query2)
        {
            query1Count = await _queryEvaluatorService.GetCountOfQuery(query1);
            query2Count = await _queryEvaluatorService.GetCountOfQuery(query2);
            if (query1Count != query2Count) { return false; }
            int intersectedQueryCount = await _queryEvaluatorService.GetIntersectQueryCount(query1, query2);
            if (query1Count != intersectedQueryCount) { return false; }
            return true;
        }



    }
}
