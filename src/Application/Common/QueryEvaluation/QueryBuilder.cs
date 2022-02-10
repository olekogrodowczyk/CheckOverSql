﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.QueryEvaluation
{
    public interface IQueryBuilder
    {
        QueryBuilder AddCount();
        QueryBuilder AddLimit(int limit);
        QueryBuilder CheckOrderBy();
        QueryBuilder AddRowNumberColumn();
    }

    public class QueryBuilder : IQueryBuilder
    {
        private string _query;
        public QueryBuilder()
        {
            this.Reset();
        }

        public QueryBuilder(string query)
        {
            this._query = query;
            this.Trim();
        }

        private void Trim()
        {
            _query = _query.Trim();
            _query = _query.Trim(';');
        }

        public void Reset()
        {
            this._query = String.Empty;
        }

        public QueryBuilder AddCount()
        {
            _query = $"SELECT COUNT(*) AS COUNT FROM ({_query}) QUERY";
            return this;
        }

        public QueryBuilder AddLimit(int limit)
        {
            _query = $"SELECT TOP({limit}) * FROM ({_query})";
            return this;
        }

        public QueryBuilder GetSpecificRow(int rowNumber)
        {
            _query = "SELECT * FROM(SELECT *, ROW_NUMBER() OVER(ORDER BY(SELECT NULL)) AS RowNumber"+
                $" FROM ({_query}) QUERY ) AS myTable WHERE RowNumber = {rowNumber}";
            return this;
        }

        public QueryBuilder AddRowNumberColumn()
        {
            _query = "SELECT * FROM(SELECT *, ROW_NUMBER() OVER(ORDER BY(SELECT NULL)) AS RowNumber" +
                $" FROM ({_query}) QUERY ) AS MyTable";
            return this;
        }

        public QueryBuilder CheckOrderBy()
        {
            if (_query.ToLower().Contains("order by"))
            {
                _query = $"{_query} OFFSET 0 ROW";
            }
            return this;
        }

        public string GetResult()
        {
            string result = this._query;
            this.Reset();
            return result;
        }
    }
}
