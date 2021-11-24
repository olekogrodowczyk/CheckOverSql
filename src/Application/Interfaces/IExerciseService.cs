using Application.Dto.AssignExerciseToUsersTo;
using Application.Dto.CreateExerciseDto;
using Application.Solvings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IExerciseService
    {
        Task CheckIfUserCanAssignExerciseToUsers(int groupId);
    }
}
