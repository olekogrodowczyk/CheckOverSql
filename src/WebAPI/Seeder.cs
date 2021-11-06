using Domain.Entities;
using Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class Seeder
    {
        private readonly ApplicationDbContext _context;

        public Seeder(ApplicationDbContext context)
        {
            _context = context;
        }
        public void Seed()
        {
            if(_context.Database.CanConnect())
            {
                if(!_context.GroupRoles.Any())
                {
                    var groupRoles = getGroupRoles();
                    _context.GroupRoles.AddRange(groupRoles);
                }
                if(!_context.Roles.Any())
                {
                    var roles = getRoles();
                    _context.Roles.AddRange(roles);
                }
                _context.SaveChanges();
            }
        }

        private IEnumerable<Role> getRoles()
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

        private IEnumerable<GroupRole> getGroupRoles()
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
                    Name = "Checker"
                }
            };
        }
    }
}
