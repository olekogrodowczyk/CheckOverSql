using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface ISolvingRepository : IRepository<Solving>
    {
        Task<IEnumerable<Solving>> GetAllSolvingsAvailable(int userId);
        Task<IEnumerable<Solving>> GetAllSolvingsAssignedToUser(int userId);
        Task<Solving> GetSolvingWithIncludes(int solvingId);
        Task<IEnumerable<Solving>> GetSolvingsAssignedToUserToDo(int userId);
    }
}
