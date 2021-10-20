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
        Task<bool> CompareAsync(int solutionId, int exerciseId);
        Task<int> CreateSolutionAsync(CreateSolutionDto model, int exerciseId);
        Task<IEnumerable<GetSolutionVm>> GetAllSolutionsAsync(int exerciseId);
        Task<List<List<string>>> SendSolutionQueryAsync(int solutionId);
    }
}
