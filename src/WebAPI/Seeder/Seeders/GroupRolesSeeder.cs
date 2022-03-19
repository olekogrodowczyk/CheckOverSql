using Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPI.Seeders;

namespace WebAPI.Seeder.Seeders
{
    public class GroupRolesSeeder : ISeeder
    {
        private readonly ApplicationDbContext _context;

        public GroupRolesSeeder(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Seed()
        {
            if (!_context.GroupRoles.Any())
            {
                var groupRoles = GroupRolesSeederData.GetGroupRoles();
                _context.GroupRoles.AddRange(groupRoles);
                _context.SaveChanges();
            }
        }
    }
}
