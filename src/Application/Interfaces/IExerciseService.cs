using Application.Dto.AssignExerciseToUsersTo;
using Application.Dto.CreateExerciseDto;
using Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IExerciseService
    {
        Task<int> CreateExercise(CreateExerciseDto model);
        Task<IEnumerable<GetExerciseVm>> GetAllPublicExercises();
        Task<IEnumerable<GetExerciseVm>> GetAllExercisesCreatedByLoggedUser();
        Task<IEnumerable<int>> AssignExerciseToAllUsers(int groupId, int exerciseId, AssignExerciseToUsersDto model);
        Task CheckIfUserCanAssignExerciseToUsers(int groupId);
    }
}
