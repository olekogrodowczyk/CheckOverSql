using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAPI.Seeders
{
    public static class RolesSeederData
    {
        public static IEnumerable<Role> GetRoles()
        {
            return new List<Role>()
            {
                new Role()
                {
                    Name = "Admin"
                },
                new Role()
                {
                    Name = "Moderator"
                },
                new Role()
                {
                    Name = "User"
                }
            };
        }
    }
}
