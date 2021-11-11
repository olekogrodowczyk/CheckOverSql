using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAPI
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
                if(!_context.Permissions.Any())
                {
                    var permissions = getPermissions();
                    _context.Permissions.AddRange(permissions);
                }             
                _context.SaveChanges();
                if (!_context.GroupRolePermissions.Any())
                {
                    var permissions = _context.Permissions.ToList();
                    var groupRoles = _context.GroupRoles.ToList();

                    var groupRolePermission = GetGroupRolePermissions(permissions, groupRoles);
                    _context.GroupRolePermissions.AddRange(groupRolePermission);
                    _context.SaveChanges();
                }
            }
        }

        public static IEnumerable<Role> getRoles()
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

        public static IEnumerable<GroupRole> getGroupRoles()
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

        public static IEnumerable<Permission> getPermissions()
        {
            return new List<Permission>
            {
                new Permission()
                {
                    Title = "Sending invitations",
                    Description = "This permission lets a user to send invitations to other users"
                },
                new Permission()
                {
                    Title = "Deleting users",
                    Description = "This permission lets a user to delete other users from group"
                },
                new Permission()
                {
                    Title = "Deleting group",
                    Description = "This permission lets a user to delete a group"
                }
            };
        }

        public static IEnumerable<GroupRolePermission> GetGroupRolePermissions(IEnumerable<Permission> permissions, IEnumerable<GroupRole> groupRoles)
        {         
            return new List<GroupRolePermission>
            {
                new GroupRolePermission
                {
                    GroupRoleId = groupRoles.SingleOrDefault(x=>x.Name == "Owner").Id,
                    PermissionId = permissions.SingleOrDefault(x=>x.Title == "Sending invitations").Id
                },
                new GroupRolePermission
                {
                    GroupRoleId = groupRoles.SingleOrDefault(x=>x.Name == "Owner").Id,
                    PermissionId = permissions.SingleOrDefault(x=>x.Title == "Deleting users").Id
                },
                new GroupRolePermission
                {
                    GroupRoleId = groupRoles.SingleOrDefault(x=>x.Name == "Owner").Id,
                    PermissionId = permissions.SingleOrDefault(x=>x.Title == "Deleting group").Id
                },
                new GroupRolePermission
                {
                    GroupRoleId = groupRoles.SingleOrDefault(x=>x.Name == "Moderator").Id,
                    PermissionId = permissions.SingleOrDefault(x=>x.Title == "Sending invitations").Id
                },
                new GroupRolePermission
                {
                    GroupRoleId = groupRoles.SingleOrDefault(x=>x.Name == "Moderator").Id,
                    PermissionId = permissions.SingleOrDefault(x=>x.Title == "Deleting users").Id
                },
            };
        }
    }
}
