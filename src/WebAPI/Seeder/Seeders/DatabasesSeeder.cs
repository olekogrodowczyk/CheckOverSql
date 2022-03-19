using Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPI.Seeders;

namespace WebAPI.Seeder.Seeders
{
    public class DatabasesSeeder : ISeeder
    {
        private readonly ApplicationDbContext _context;

        public DatabasesSeeder(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Seed()
        {
            if (!_context.Databases.Any())
            {
                var databases = DatabasesSeederData.GetDatabases();
                _context.Databases.AddRange(databases);
                _context.SaveChanges();
            }
        }
    }
}
