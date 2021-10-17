using Application.Dto.CreateSolutionDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface ISolutionService
    {
        Task<int> CreateSolutionAsync(CreateSolutionDto model, int ExerciseId);
        Task<Dictionary<int, object>> SendSolutionQuery(int solutionId);
    }
}
