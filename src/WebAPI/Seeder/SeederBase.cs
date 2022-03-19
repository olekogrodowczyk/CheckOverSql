using Domain.Common;
using Domain.Entities;
using Domain.Enums;
using Domain.ValueObjects;
using Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPI.Seeder.Seeders;
using WebAPI.Seeders;

namespace WebAPI
{
    public class SeederBase
    {
        private readonly ApplicationDbContext _context;

        public SeederBase(ApplicationDbContext context)
        {
            _context = context;
        }
        public void Seed()
        {
            if (_context.Database.CanConnect())
            {
                if (_context.Database.IsSqlServer())
                {
                    var pendingMigrations = _context.Database.GetPendingMigrations();
                    if (pendingMigrations != null && pendingMigrations.Any())
                    {
                        _context.Database.Migrate();
                    }
                }

                IEnumerable<ISeeder> seeders = new List<ISeeder>()
                {
                    new RolesSeeder(_context),
                    new PermissionsSeeder(_context),
                    new GroupRolesSeeder(_context),
                    new GroupRolePermissionsSeeder(_context),
                    new UsersSeeder(_context, new PasswordHasher<User>()),
                    new DatabasesSeeder(_context),
                    new ExercisesSeeder(_context)
                };

                foreach (ISeeder seeder in seeders)
                {
                    seeder.Seed();
                }
            }
        }






    }
}
