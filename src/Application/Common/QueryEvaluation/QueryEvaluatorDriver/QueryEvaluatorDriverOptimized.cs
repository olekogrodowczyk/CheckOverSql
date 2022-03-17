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
        private readonly IConnectionStringService _connectionStringService;

        public QueryEvaluatorDriverOptimized(IQueryEvaluator queryEvaluator,
             IConnectionStringService connectionStringService)
        {
            _queryEvaluator = queryEvaluator;
            _connectionStringService = connectionStringService;
        }

        public async Task<bool> Evaluate(string query1, string query2, string connectionString)
        {
            _connectionStringService.ConnectionString = connectionString;

            if (_queryEvaluator.CompareBodies(query1, query2)) { return true; }
            if (!(await _queryEvaluator.CompareColumnNames(query1, query2))) { return false; }
            if (!(await _queryEvaluator.CompareQueriesCount(query1, query2))) { return false; }
            if (!(await _queryEvaluator.CompareFirstMiddleLastRows(query1, query2))) { return false; }
            return true;
        }
    }
}
