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
        private readonly IDatabaseRepository _databaseRepository;

        public SolutionRepository(ApplicationDbContext context, IDatabaseRepository databaseRepository ) : base(context)
        {
            _databaseRepository = databaseRepository;
        }

        public async Task<string> GetDatabaseName(int id)
        {
            var result = await _context.Solutions
                .Include(x => x.Exercise)
                .ThenInclude(x=>x.Database)
                .FirstOrDefaultAsync(x=>x.Id == id);
            if (result == null) { throw new NotFoundException($"Result is not found with id:{id}"); }
            return result.Exercise.Database.Name;
        }
    }
}
