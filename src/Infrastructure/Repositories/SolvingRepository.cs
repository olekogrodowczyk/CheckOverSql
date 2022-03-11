using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class SolvingRepository : EfRepository<Solving>, ISolvingRepository
    {
        private readonly IAssignmentRepository _assignmentRepository;

        public SolvingRepository(ApplicationDbContext context, ILogger<EfRepository<Solving>> logger
            , IAssignmentRepository assignmentRepository) : base(context, logger)
        {
            _assignmentRepository = assignmentRepository;
        }

        public async Task<IEnumerable<Solving>> GetAllSolvingsAssignedToUser
            (int userId, SolvingStatusEnum status = SolvingStatusEnum.ToDo)
        {
            var solvings = await _context.Solvings
                .Include(x => x.Creator)
                .Include(x => x.Exercise)
                .ThenInclude(x => x.Database)
                .Include(x => x.Assignment)
                .ThenInclude(x => x.User)
                .Include(x => x.Assignment)
                .ThenInclude(x => x.Group)
                .Where(x => x.Assignment.UserId == userId && x.Status == status)
                .ToListAsync();
            return solvings;
        }

        public async Task<IEnumerable<Solving>> GetSolvingsAssignedToUserToDo(int userId)
        {
            var solvings = await _context.Solvings
                .Include(x => x.Creator)
                .Include(x => x.Assignment)
                .ThenInclude(x => x.User)
                .Where(x => x.Assignment.UserId == userId && x.Status == SolvingStatusEnum.ToDo)
                .ToListAsync();
            return solvings;
        }

        public async Task<Solving> GetSolvingWithIncludes(int solvingId)
        {
            var solving = await _context.Solvings
                .Include(x => x.Creator)
                .Include(x => x.Assignment)
                .ThenInclude(x => x.Group)
                .Include(x => x.Assignment)
                .ThenInclude(x => x.User)
                .Include(x => x.Exercise)
                .ThenInclude(x => x.Database)
                .FirstOrDefaultAsync(x => x.Id == solvingId);
            return solving;
        }

        public async Task<IEnumerable<Solving>> GetAllSolvingsWithIncludes(Expression<Func<Solving, bool>> expression)
        {
            var solvings = await _context.Solvings
                .Include(x => x.Creator)
                .Include(x => x.Exercise)
                .ThenInclude(x => x.Database)
                .Include(x => x.Assignment)
                .ThenInclude(x => x.User)
                .Include(x => x.Assignment)
                .ThenInclude(x => x.Group)
                .Where(expression)
                .ToListAsync();

            return solvings;
        }

        public async Task<IEnumerable<Solving>> GetAllSolvingsToCheck(int userId, int? groupId)
        {
            var userAssignments = _context.Assignments
                .Where(x => groupId == null ? x.UserId == userId : x.UserId == userId && x.GroupId == groupId);
            var availableAssignments = new List<Assignment>();
            foreach (var assignment in userAssignments)
            {
                if (await _assignmentRepository.CheckIfAssignmentHasPermission(assignment.Id, PermissionEnum.CheckingExercises))
                {
                    availableAssignments.Add(assignment);
                }
            }
            var solvingsToCheck = new List<Solving>();
            foreach (var assignment in availableAssignments)
            {
                solvingsToCheck.AddRange(await _context.Assignments
                    .Include(x => x.Solvings)
                    .Where(x => x.Id == assignment.Id)
                    .SelectMany(x => x.Solvings)
                    .Include(x => x.Creator)
                    .Include(x => x.Exercise)
                    .ThenInclude(x => x.Database)
                    .Include(x => x.Assignment)
                    .ThenInclude(x => x.User)
                    .Include(x => x.Assignment)
                    .ThenInclude(x => x.Group)
                    .Where(x => x.Status == SolvingStatusEnum.Done || x.Status == SolvingStatusEnum.DoneButOverdue)
                    .ToListAsync());
            }
            return solvingsToCheck;
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
