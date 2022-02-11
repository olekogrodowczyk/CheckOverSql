using Application.Groups;
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
        Task<bool> CheckIfUserPassedExercise(int exerciseId);
        Task<bool> Compare(int solutionId, string query);
        Task<Comparison> CreateComparison(int solutionId, int exerciseId);
        Task<string> GetLatestSolutionQuerySentIntoExercise(int exerciseId);
        Task HandlePossibleSolvingToDo(int exerciseId, Solution solution);
    }
}
