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
        Task<bool> CompareAsync(int solutionId, int exerciseId);
        Task<int> CreateSolutionAsync(CreateSolutionDto model, int exerciseId);
        Task<List<List<string>>> SendSolutionQueryAsync(int solutionId);
    }
}
