using Application.Dto.CreateExerciseDto;
using Application.Dto.GetExerciseDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IExerciseService
    {
        Task<int> CreateExerciseAsync(CreateExerciseDto model);
        Task<IEnumerable<GetExerciseDto>> GetAllExercisesAsync();
    }
}
