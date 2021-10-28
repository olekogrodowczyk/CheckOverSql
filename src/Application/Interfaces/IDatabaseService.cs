using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IDatabaseService
    {
        Task<int> SendQueryNoData(string query, string databaseName, bool isAdmin=false);
        Task<List<List<string>>> SendQueryWithData(string query, string databaseName, bool isAdmin=false);
    }
}
