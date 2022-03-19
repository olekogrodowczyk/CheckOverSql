using Infrastructure.Data;
using System.Linq;
using WebAPI.Seeders;

namespace WebAPI.Seeders
{
    public class RolesSeeder : ISeeder
    {
        private readonly ApplicationDbContext _context;

        public RolesSeeder(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Seed()
        {
            if (!_context.Roles.Any())
            {
                var roles = RolesSeederData.GetRoles();
                _context.Roles.AddRange(roles);
                _context.SaveChanges();
            }
        }
    }
}
