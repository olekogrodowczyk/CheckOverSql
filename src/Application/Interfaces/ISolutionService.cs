using Application.Dto.CreateSolutionDto;
using Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface ISolutionService
    {
        Task<bool> Compare(int solutionId, int exerciseId);
        Task CreateComparison(int solutionId, int exerciseId, bool comparisonResult);
        Task<int> CreateSolution(CreateSolutionDto model, int exerciseId);
        Task<IEnumerable<GetSolutionVm>> GetAllSolutions(int exerciseId);
        Task<List<List<string>>> SendSolutionQuery(int solutionId);
    }
}
