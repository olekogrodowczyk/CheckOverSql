using Application.Common.Exceptions;
using Application.Interfaces;
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
    public class SolutionRepository : EfRepository<Solution>, ISolutionRepository
    {
        private readonly IDatabaseRepository _databaseRepository;
        private readonly IUserContextService _userContextService;

        public SolutionRepository(ApplicationDbContext context, IDatabaseRepository databaseRepository,
            IUserContextService userContextService, ILogger<SolutionRepository> logger) : base(context, logger)
        {
            _databaseRepository = databaseRepository;
            _userContextService = userContextService;
        }

        public async Task<string> GetDatabaseName(int solutionId)
        {
            var result = await _context.Solutions
                .Include(x => x.Exercise)
                .ThenInclude(x=>x.Database)
                .FirstOrDefaultAsync(x=>x.Id == solutionId);
            if (result == null) { throw new NotFoundException($"Result is not found with id:{solutionId}"); }
            return result.Exercise.Database.Name;
        }

        public async Task<IEnumerable<Solution>> GetAllCreatedByUser(int exerciseId)
        {
            var result = await _context.Solutions
                .Include(x => x.Exercise)
                .Where(x => x.ExerciseId == exerciseId && x.CreatorId == _userContextService.GetUserId)
                .ToListAsync();
            if (result == null) { throw new NotFoundException($"Result is not found with exerciseId:{exerciseId}, or user is not logged in"); }
            return result;
        }

        public async Task<Solution> GetLatestSolutionSentByUserInExercise(int exerciseId, int userId)
        {
            var solution = await _context.Solutions
                .Where(x => x.ExerciseId == exerciseId && x.CreatorId == userId)
                .OrderByDescending(x => x.Created)
                .FirstOrDefaultAsync();
            return solution;
        }
    }
}
