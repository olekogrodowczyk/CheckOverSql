using Application.Exceptions;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class SolutionRepository : EfRepository<Solution>, ISolutionRepository
    {
        public SolutionRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<string> GetDatabaseConnectionString(int solutionId)
        {
            var solution = await _context.Solutions
                .Include(x=>x.Exercise)
                .ThenInclude(x=>x.Database)
                .FirstOrDefaultAsync(x=>x.Id== solutionId);
            if (solution is null)
            {
                throw new NotFoundException($"Nie znaleziono solucji o podanym id: {solutionId}");
            }
            return solution.Exercise.Database.ConnectionString;
        }
    }
}
