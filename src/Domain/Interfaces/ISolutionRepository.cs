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
        Task<IEnumerable<Solution>> GetAllCreatedByUser(int exerciseId);
        Task<string> GetDatabaseName(int solutionId);
    }
}
