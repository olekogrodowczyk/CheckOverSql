using Domain.Entities;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface ISolvingRepository : IRepository<Solving>
    {
        Task<IEnumerable<Solving>> GetAllSolvingsAvailable(int userId);
        Task<IEnumerable<Solving>> GetAllSolvingsAssignedToUser(int userId, SolvingStatusEnum status = SolvingStatusEnum.ToDo);
        Task<Solving> GetSolvingWithIncludes(int solvingId);
        Task<IEnumerable<Solving>> GetSolvingsAssignedToUserToDo(int userId);
        Task<IEnumerable<Solving>> GetAllSolvingsWithIncludes(Expression<Func<Solving, bool>> expression);
        Task<IEnumerable<Solving>> GetAllSolvingsToCheck(int userId, int? groupId = null);
        Task<IEnumerable<Solving>> GetAllSolvingsToCheckInGroup(int groupId);
        Task<Group> GetGroupBySolvingId(int solvingId);
    }
}
