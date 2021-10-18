using Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IDatabaseQuery
    {
        Task<List<List<string>>> GetData(string query, string connectionString);
    }
}
