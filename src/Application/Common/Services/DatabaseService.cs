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
    public class DatabaseService : IDatabaseService
    {
        private readonly IDatabaseQuery _databaseQuery;
        private readonly IDatabaseRepository _databaseRepository;

        public DatabaseService(IDatabaseQuery databaseQuery, IDatabaseRepository databaseRepository)
        {
            _databaseQuery = databaseQuery;
            _databaseRepository = databaseRepository;
        }
       
        public async Task<List<List<string>>> SendQueryWithData(string query, string databaseName, bool isAdmin=false, int? numberOfRows = null)
        {
            string connectionString = await _databaseRepository.GetDatabaseConnectionString(databaseName);
            var result = await _databaseQuery.ExecuteQueryWithData(query, connectionString.Replace("\\\\", "\\"), numberOfRows);
            return result;
        }

        public async Task<int> SendQueryNoData(string query, string databaseName, bool isAdmin=false)
        {
            string connectionString = await _databaseRepository.GetDatabaseConnectionString(databaseName);
            var result = await _databaseQuery.ExecuteQueryNoData(query, connectionString.Replace("\\\\","\\"));
            return result;
        }
  

    }
}
