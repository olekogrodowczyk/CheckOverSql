using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAPI.Seeders
{
    public static class GroupRolesSeederData
    {
        public static IEnumerable<GroupRole> GetGroupRoles()
        {
            return new List<GroupRole>()
            {
                new GroupRole()
                {
                    IsCustom = false,
                    Name = "Owner",
                },
                new GroupRole()
                {
                    IsCustom = false,
                    Name = "Moderator",
                },
                new GroupRole()
                {
                    IsCustom = false,
                    Name = "Checker"
                },
                new GroupRole()
                {
                    IsCustom = false,
                    Name = "User"
                }
            };
        }
    }
}
