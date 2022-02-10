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

        public QueryEvaluator(IDatabaseQuery databaseQuery)
        {
            _databaseQuery = databaseQuery;
        }

        public async Task<int> GetCountOfQuery(string query, string connectionString)
        {
            QueryBuilder qb = new QueryBuilder(query);
            qb.CheckOrderBy().AddCount();
            return await _databaseQuery.ExecuteQueryGetOneIntValue(qb.GetResult(), connectionString);
        }

        public async Task<int> GetIntersectQueryCount(string query1, string query2, string connectionString)
        {
            string intersectedQuery = interesectBetweenQueries(prepareQueryToIntersect(query1), prepareQueryToIntersect(query2));
            QueryBuilder qb = new QueryBuilder(intersectedQuery);              
            qb.AddCount();
            return await _databaseQuery.ExecuteQueryGetOneIntValue(qb.GetResult(), connectionString);
        }        

        public async Task<bool> CompareColumnNames(string query1, string query2, string connectionString)
        {
            var columnNames1 = await _databaseQuery.GetColumnNames(query1, connectionString);
            var columnNames2 = await _databaseQuery.GetColumnNames(query2, connectionString);
            return columnNames1.SequenceEqual(columnNames2);
        }

        public async Task<List<List<string>>> GetFirstMiddleLastRows(string query, int queryResultCount, string connectionString)
        {
            if (queryResultCount < 2) { return null; }
            List<List<string>> values = new List<List<string>>();
            foreach(int item in new int[] {1, queryResultCount/2, queryResultCount})
            {
                var rows = await getSpecificRow(query, queryResultCount, connectionString);
                values.Add(rows);
            }
            return values;
        }

        private async Task<List<string>> getSpecificRow(string query, int rowCount, string connectionString)
        {
            QueryBuilder qb = new QueryBuilder(query);
            qb.CheckOrderBy().GetSpecificRow(10);
            var row = await _databaseQuery.ExecuteQueryGetOneRow(qb.GetResult(), connectionString);
            return row;
        }

        private string prepareQueryToIntersect(string query)
        {
            QueryBuilder qb = new QueryBuilder(query);
            qb.CheckOrderBy();
            qb.AddRowNumberColumn();
            return qb.GetResult();
        }

        private string interesectBetweenQueries(string query1, string query2)
        {
            return $"{query1} INTERSECT {query2}";
        }
    }
}
