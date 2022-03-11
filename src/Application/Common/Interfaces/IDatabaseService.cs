using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IDatabaseService
    {
        Task<int> SendQueryNoData(string query, string databaseName);
        Task<List<List<string>>> SendQueryWithData(string query, string databaseName, int? numberOfRows = null);
        Task<bool> ValidateQuery(string query, string databaseName);
    }
}
