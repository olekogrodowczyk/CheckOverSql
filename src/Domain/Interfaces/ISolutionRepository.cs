using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface ISolutionRepository : IRepository<Solution>
    {
        Task<string> GetDatabaseConnectionString(int solutionId);
        Task<string> GetDatabaseName(int solutionId);
    }
}
