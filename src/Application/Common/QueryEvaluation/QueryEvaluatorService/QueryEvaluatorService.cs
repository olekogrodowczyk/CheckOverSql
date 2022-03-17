using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.QueryEvaluation
{
    public class QueryEvaluatorService : IQueryEvaluatorService
    {
        private readonly IDatabaseQuery _databaseQuery;
        private readonly IQueryBuilder _queryBuilder;
        private readonly IConnectionStringService _connectionStringService;

        public QueryEvaluatorService(IDatabaseQuery databaseQuery, IQueryBuilder queryBuilder
            , IConnectionStringService connectionStringService)
        {
            _databaseQuery = databaseQuery;
            _queryBuilder = queryBuilder;
            _connectionStringService = connectionStringService;
        }

        public async Task<int> GetCountOfQuery(string query)
        {
            _queryBuilder.SetInitQuery(query).CheckOrderBy().AddCount();
            return await _databaseQuery.ExecuteQueryGetOneIntValue(_queryBuilder.GetResult(), _connectionStringService.ConnectionString);
        }

        public async Task<int> GetIntersectQueryCount(string query1, string query2)
        {
            string intersectedQuery = interesectBetweenQueries(prepareQueryToIntersect(query1), prepareQueryToIntersect(query2));
            _queryBuilder.SetInitQuery(intersectedQuery).AddCount();
            return await _databaseQuery.ExecuteQueryGetOneIntValue(_queryBuilder.GetResult(), _connectionStringService.ConnectionString);
        }

        public async Task<bool> CompareColumnNames(string query1, string query2)
        {
            var columnNames1 = await _databaseQuery.GetColumnNames(query1, _connectionStringService.ConnectionString);
            var columnNames2 = await _databaseQuery.GetColumnNames(query2, _connectionStringService.ConnectionString);
            return columnNames1.SequenceEqual(columnNames2);
        }

        public async Task<List<List<string>>> GetFirstMiddleLastRows(string query, int queryResultCount)
        {
            if (queryResultCount < 2) { return null; }
            List<List<string>> values = new List<List<string>>();
            foreach (int item in new int[] { 1, queryResultCount / 2, queryResultCount })
            {
                var rows = await GetSpecificRow(query, queryResultCount);
                values.Add(rows);
            }
            return values;
        }

        public async Task<List<string>> GetSpecificRow(string query, int rowCount)
        {
            _queryBuilder.SetInitQuery(query).CheckOrderBy().GetSpecificRow(10);
            var row = await _databaseQuery.ExecuteQueryGetOneRow(_queryBuilder.GetResult(), _connectionStringService.ConnectionString);
            return row;
        }

        private string prepareQueryToIntersect(string query)
        {
            _queryBuilder.SetInitQuery(query).CheckOrderBy().AddRowNumberColumn();
            return _queryBuilder.GetResult();
        }

        private string interesectBetweenQueries(string query1, string query2)
        {
            return $"{query1} INTERSECT {query2}";
        }
    }
}
