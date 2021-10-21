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
    public class DatabaseRepository : EfRepository<Database>, IDatabaseRepository
    {
        public DatabaseRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<int> GetDatabaseIdByName(string name)
        {
            var result = await _context.Databases.FirstOrDefaultAsync(x => x.Name == name);
            return result.Id;
        }

        public async Task<string> GetDatabaseConnectionStringByName(string name)
        {
            var result = await _context.Databases.FirstOrDefaultAsync(x => x.Name == name);
            return result.ConnectionString;
        }

        public async Task<string> GetAdminDatabaseConnectionStringByName(string name)
        {
            var result = await _context.Databases.FirstOrDefaultAsync(x => x.Name == name);
            return result.ConnectionStringAdmin;
        }
    }
}
