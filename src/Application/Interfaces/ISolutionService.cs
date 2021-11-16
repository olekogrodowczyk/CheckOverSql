using Application.Dto.CreateSolutionDto;
using Application.ViewModels;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface ISolutionService
    {
        Task<bool> Compare(int solutionId, string query);
        Task<Comparison> CreateComparison(int solutionId, int exerciseId);
        Task<GetComparisonVm> CreateSolution(int exerciseId, CreateSolutionDto model);
        Task<IEnumerable<GetSolutionVm>> GetAllSolutions(int exerciseId);
        Task<int> GetComparisonResult(int exerciseId, int solutionId);
        Task<List<List<string>>> SendSolutionQuery(int solutionId);
    }
}
