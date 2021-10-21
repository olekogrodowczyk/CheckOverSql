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
    public class DatabaseService : IDatabaseService
    {
        private readonly IDatabaseQuery _databaseQuery;
        private readonly IDatabaseRepository _databaseRepository;

        public DatabaseService(IDatabaseQuery databaseQuery, IDatabaseRepository databaseRepository)
        {
            _databaseQuery = databaseQuery;
            _databaseRepository = databaseRepository;
        }
        
        public async Task<string> GetDatabaseConnectionString(string databaseName, bool isAdmin=false)
        {
            return isAdmin == false ? await _databaseRepository.GetDatabaseConnectionStringByName(databaseName) :
               await _databaseRepository.GetAdminDatabaseConnectionStringByName(databaseName);
        }

        public async Task<List<List<string>>> SendQueryWithData(string query, string databaseName, bool isAdmin=false)
        {
            string connectionString = await GetDatabaseConnectionString(databaseName, isAdmin);
            var result = await _databaseQuery.ExecuteQueryWithData(query, connectionString.Replace("\\\\", "\\"));
            return result;
        }

        public async Task<int> SendQueryNoData(string query, string databaseName, bool isAdmin=false)
        {
            string connectionString = await GetDatabaseConnectionString(databaseName, isAdmin);
            var result = await _databaseQuery.ExecuteQueryNoData(query, connectionString.Replace("\\\\","\\"));
            return result;
        }
  

    }
}
