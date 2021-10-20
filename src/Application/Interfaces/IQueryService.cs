using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IQueryService
    {
        Task<List<List<string>>> sendQueryAsync(string query, string connectionString, params string[] unallowedValues);
    }
}
