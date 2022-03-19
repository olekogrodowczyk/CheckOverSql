using Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAPI.Seeders
{
    public class PermissionsSeeder : ISeeder
    {
        private readonly ApplicationDbContext _context;

        public PermissionsSeeder(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Seed()
        {
            if (!_context.Permissions.Any())
            {
                var permissions = PermissionsSeederData.GetPermissions();
                _context.Permissions.AddRange(permissions);
                _context.SaveChanges();
            }
        }
    }
}
