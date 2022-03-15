﻿using Domain.Interfaces;
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
        private string _connectionString;

        public QueryEvaluator(IDatabaseQuery databaseQuery, IQueryBuilder queryBuilder)
        {
            _databaseQuery = databaseQuery;
            _queryBuilder = queryBuilder;
        }

        public void InitConnectionString(string connectionString)
        {
            _connectionString = connectionString;
        }


        public async Task<int> GetCountOfQuery(string query)
        {
            _queryBuilder.SetInitQuery(query).CheckOrderBy().AddCount();
            return await _databaseQuery.ExecuteQueryGetOneIntValue(_queryBuilder.GetResult(), _connectionString);
        }

        public async Task<int> GetIntersectQueryCount(string query1, string query2)
        {
            string intersectedQuery = interesectBetweenQueries(prepareQueryToIntersect(query1), prepareQueryToIntersect(query2));
            _queryBuilder.SetInitQuery(intersectedQuery).AddCount();
            return await _databaseQuery.ExecuteQueryGetOneIntValue(_queryBuilder.GetResult(), _connectionString);
        }

        public async Task<bool> CompareColumnNames(string query1, string query2)
        {
            var columnNames1 = await _databaseQuery.GetColumnNames(query1, _connectionString);
            var columnNames2 = await _databaseQuery.GetColumnNames(query2, _connectionString);
            return columnNames1.SequenceEqual(columnNames2);
        }

        public async Task<List<List<string>>> GetFirstMiddleLastRows(string query, int queryResultCount)
        {
            if (queryResultCount < 2) { return null; }
            List<List<string>> values = new List<List<string>>();
            foreach (int item in new int[] { 1, queryResultCount / 2, queryResultCount })
            {
                var rows = await getSpecificRow(query, queryResultCount);
                values.Add(rows);
            }
            return values;
        }

        private async Task<List<string>> getSpecificRow(string query, int rowCount)
        {
            _queryBuilder.SetInitQuery(query).CheckOrderBy().GetSpecificRow(10);
            var row = await _databaseQuery.ExecuteQueryGetOneRow(_queryBuilder.GetResult(), _connectionString);
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
