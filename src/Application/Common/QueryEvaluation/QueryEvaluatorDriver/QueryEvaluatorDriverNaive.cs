using Application.Interfaces;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.QueryEvaluation
{
    public class QueryEvaluatorDriverNaive : IQueryEvaluatorDriver
    {
        private readonly IDatabaseQuery _databaseQuery;
        private readonly IDataComparerService _dataComparerService;

        public QueryEvaluatorDriverNaive(IDatabaseQuery databaseQuery, IDataComparerService dataComparerService)
        {
            _databaseQuery = databaseQuery;
            _dataComparerService = dataComparerService;
        }

        public async Task<bool> Evaluate(string query1, string query2, string connectionString)
        {
            var list1 = await _databaseQuery.ExecuteQueryWithData(query1, connectionString);
            var list2 = await _databaseQuery.ExecuteQueryWithData(query2, connectionString);

            bool comparisonResult = await _dataComparerService.compareValues(list1, list2);
            return comparisonResult;
        }
    }
}
