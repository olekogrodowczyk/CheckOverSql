using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IDatabaseRepository : IRepository<Database>
    {
        Task<string> GetDatabaseConnectionString(string name, bool isAdmin = false);
        Task<int> GetDatabaseIdByName(string name);
        Task<string> GetDatabaseNameById(int databaseId);
    }
}
