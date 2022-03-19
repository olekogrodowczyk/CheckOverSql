using Domain.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using WebAPI.Seeders;

namespace WebAPI.Seeders
{
    public class UsersSeeder : ISeeder
    {
        private readonly ApplicationDbContext _context;
        private readonly IPasswordHasher<User> _hasher;

        public UsersSeeder(ApplicationDbContext context, IPasswordHasher<User> hasher)
        {
            _context = context;
            _hasher = hasher;
        }

        public void Seed()
        {
            if (!_context.Users.Any(x => x.Login == "superuser"))
            {
                seedSuperUser();
            }
        }

        private void seedSuperUser()
        {
            int? adminRoleId = _context.Roles.FirstOrDefault(x => x.Name == "Admin")?.Id;
            if (adminRoleId is null) { return; }
            var superUser = UsersSeederData.GetSuperUser((int)adminRoleId
                , _hasher.HashPassword(null, "SET_HERE_YOUR_ADMIN_PASSWORD"));

            _context.Users.Add(superUser);
            _context.SaveChanges();
        }
    }
}
