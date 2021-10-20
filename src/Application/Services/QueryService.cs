using Application.Exceptions;
using Application.Interfaces;
using Domain.Enums;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class QueryService : IQueryService
    {
        private readonly IDatabaseQuery _databaseQuery;

        public QueryService(IDatabaseQuery databaseQuery)
        {
            _databaseQuery = databaseQuery;
        }
        public async Task<List<List<string>>> sendQueryAsync(string query, string connectionString
            ,params string[] unallowedValues)
        {
            var result = await _databaseQuery.GetData(query, connectionString.Replace("\\\\", "\\"));
            return result;
        }

  

    }
}
