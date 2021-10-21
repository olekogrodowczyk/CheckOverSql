using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IDatabaseQuery
    {
        Task<int> ExecuteQueryNoData(string query, string connectionString);
        Task<List<List<string>>> ExecuteQueryWithData(string query, string connectionString);
    }
}
