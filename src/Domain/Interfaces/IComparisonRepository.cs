using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IComparisonRepository : IRepository<Comparison>
    {
        Task<Comparison> GetComparisonWithIncludes(int comparisonId);
        Task<Comparison> GetLatestComparisonInExercise(int exerciseId);
    }
}
