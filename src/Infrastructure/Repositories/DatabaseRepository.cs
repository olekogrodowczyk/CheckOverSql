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
    public class DatabaseRepository : EfRepository<Database>, IDatabaseRepository
    {
        public DatabaseRepository(ApplicationDbContext context, ILogger<DatabaseRepository> logger) : base(context, logger)
        {
        }

        public async Task<int> GetDatabaseIdByName(string name)
        {
            var result = await _context.Databases.FirstOrDefaultAsync(x => x.Name == name);
            if (result == null) { throw new NotFoundException($"Result is not found with name:{name}"); }
            return result.Id;
        }      

        public async Task<string> GetDatabaseNameById(int databaseId)
        {
            var result = await _context.Databases.FirstOrDefaultAsync(x=>x.Id == databaseId);
            return result.Name;
        }

        public async Task<string> GetDatabaseConnectionString(string name, bool isAdmin = false)
        {
            var result = await _context.Databases.FirstOrDefaultAsync(x => x.Name == name);
            if (result == null) { throw new NotFoundException($"Result is not found with name:{name}"); }
            return isAdmin == false ? result.ConnectionString : result.ConnectionStringAdmin;             
        }
    }
}
