using Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPI.Seeders;

namespace WebAPI.Seeder.Seeders
{
    public class GroupRolePermissionsSeeder : ISeeder
    {
        private readonly ApplicationDbContext _context;

        public GroupRolePermissionsSeeder(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Seed()
        {
            if (!_context.GroupRolePermissions.Any())
            {
                var permissions = _context.Permissions.ToList();
                var groupRoles = _context.GroupRoles.ToList();

                var groupRolePermission = GroupRolePermissionsSeederData.GetGroupRolePermissions(permissions, groupRoles);
                _context.GroupRolePermissions.AddRange(groupRolePermission);
                _context.SaveChanges();
            }
        }
    }
}
