using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAPI.Seeders
{
    public static class UsersSeederData
    {
        public static User GetSuperUser(int adminRoleId, string passwordHash)
        {
            return new User
            {
                FirstName = "Super",
                LastName = "User",
                RoleId = adminRoleId,
                Login = "superuser",
                Email = "superuser@gmail.com",
                PasswordHash = passwordHash,
                DateOfBirth = DateTime.UtcNow.AddYears(-23)
            };
        }
    }
}
