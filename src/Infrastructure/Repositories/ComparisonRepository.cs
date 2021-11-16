using Application.Exceptions;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class ComparisonRepository : EfRepository<Comparison>, IComparisonRepository
    {
        public ComparisonRepository(ApplicationDbContext context, ILogger<ComparisonRepository> logger) : base(context, logger)
        {

        }

        public async Task<Comparison> GetComparisonWithIncludes(int comparisonId)
        {
            var comparison = await _context.Comparisons
                .Include(x => x.Solution)
                .ThenInclude(x => x.Solving)
                .ThenInclude(x => x.Assignment)
                .ThenInclude(x => x.User)
                .Include(x => x.Exercise)
                .SingleOrDefaultAsync(x => x.Id == comparisonId);
            if(comparison == null) { throw new NotFoundException($"Comparison with id: {comparisonId} cannot be found"); }
            return comparison;
        }
    }
}
