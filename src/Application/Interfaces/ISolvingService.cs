using Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface ISolvingService
    {
        Task<IEnumerable<GetSolvingVm>> GetAllSolvingsAssignedToUser();
        Task<IEnumerable<GetSolvingVm>> GetAllSolvingsAssignedToUserToDo();
        Task<GetSolvingVm> GetSolvingById(int solvingId);
    }
}
