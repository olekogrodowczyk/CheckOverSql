using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IDatabaseQuery
    {
        Task<int> ExecuteQueryGetOneIntValue(string query, string connectionString);
        Task<List<string>> ExecuteQueryGetOneRow(string query, string connectionString);
        Task<int> ExecuteQueryNoData(string query, string connectionString);
        Task<List<List<string>>> ExecuteQueryWithData(string query, string connectionString, int? numberOfRows=null);
        Task<List<string>> GetColumnNames(string query, string connectionString);
    }
}
