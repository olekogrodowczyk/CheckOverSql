using Domain.Entities;
using Domain.Enums;
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
    public class SolvingRepository : EfRepository<Solving>, ISolvingRepository
    {
        public SolvingRepository(ApplicationDbContext context, ILogger<EfRepository<Solving>> logger) : base(context, logger)
        {
        }

        public async Task<IEnumerable<Solving>> GetAllSolvingsAssignedToUser(int userId)
        {
            var solvings = await _context.Solvings
                .Include(x => x.Creator)
                .Include(x => x.Exercise)
                .ThenInclude(x=>x.Database)
                .Include(x => x.Assignment)                
                .ThenInclude(x => x.User)
                .Include(x=>x.Assignment)
                .ThenInclude(x=>x.Group)
                .Where(x => x.Assignment.UserId == userId)
                .ToListAsync();
            return solvings;
        }

        public async Task<IEnumerable<Solving>> GetSolvingsAssignedToUserToDo(int userId)
        {
            var solvings = await _context.Solvings
                .Include(x => x.Creator)
                .Include(x => x.Assignment)
                .ThenInclude(x => x.User)
                .Where(x => x.Assignment.UserId == userId && x.Status == SolvingStatus.ToDo.ToString())
                .ToListAsync();
            return solvings;
        }

        public async Task<Solving> GetSolvingWithIncludes(int solvingId)
        {
            var solving = await _context.Solvings
                .Include(x => x.Creator)
                .Include(x => x.Assignment)
                .ThenInclude(x => x.User)
                .FirstOrDefaultAsync(x => x.Id == solvingId);
            return solving;
        }

        public async Task<IEnumerable<Solving>> GetAllSolvingsAvailable(int userId)
        {
            var solvings = await _context.Assignments
                .Include(x => x.Group)
                .ThenInclude(x => x.Assignments)
                .Where(x => x.UserId == userId)
                .Select(x => x.Group)
                .SelectMany(x => x.Assignments)
                .Include(x => x.Solvings)
                .SelectMany(x => x.Solvings)
                .Include(x => x.Checking)
                .Include(x => x.Assignment)
                .ThenInclude(x => x.Group)
                .Include(x => x.Assignment)
                .ThenInclude(x => x.User)
                .Include(x => x.Exercise)
                .ToListAsync();

            return solvings;
        }
    }
}
