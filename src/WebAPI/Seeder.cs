using Domain.Entities;
using Domain.Enums;
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
            if (_context.Database.CanConnect())
            {
                if (!_context.Roles.Any())
                {
                    var roles = getRoles();
                    _context.Roles.AddRange(roles);
                    _context.SaveChanges();
                }
                if (!_context.Permissions.Any())
                {
                    var permissions = getPermissions();
                    _context.Permissions.AddRange(permissions);
                    _context.SaveChanges();
                }
                if (!_context.GroupRoles.Any())
                {
                    var groupRoles = getGroupRoles();
                    _context.GroupRoles.AddRange(groupRoles);
                    _context.SaveChanges();
                }
                if (!_context.GroupRolePermissions.Any())
                {
                    var permissions = _context.Permissions.ToList();
                    var groupRoles = _context.GroupRoles.ToList();

                    var groupRolePermission = GetGroupRolePermissions(permissions, groupRoles);
                    _context.GroupRolePermissions.AddRange(groupRolePermission);
                    _context.SaveChanges();
                }
                if (!_context.Users.Any())
                {
                    int adminRoleId = _context.Roles.FirstOrDefault(x => x.Name == "Admin").Id;
                    var user = getSuperUser(adminRoleId);
                    _context.Users.Add(user);
                    _context.SaveChanges();
                }
                if (!_context.Exercises.Any())
                {
                    int superUserId = _context.Users.FirstOrDefault(x => x.Email == "superuser@gmail.com").Id;
                    var exercises = getPublicExercises(superUserId);
                    _context.Exercises.AddRange(exercises);
                    _context.SaveChanges();
                }
            }
        }

        public static User getSuperUser(int adminRoleId)
        {
            return new User
            {
                FirstName = "Super",
                LastName = "User",
                RoleId = adminRoleId,
                Email = "superuser@gmail.com",
                PasswordHash = "dsandnsauindasuidnusa",
                DateOfBirth = DateTime.UtcNow.AddYears(-21)
            };
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

        public static IEnumerable<Exercise> getPublicExercises(int superUserId)
        {
            return new List<Exercise>()
            {
                new Exercise()
                {
                    DatabaseId = 1,
                    CreatorId = superUserId,
                    ValidAnswer = "SELECT * FROM dbo.Footballers",
                    IsPrivate = false,
                    Title = "Get all fields",
                    Description = "Get all field description"
                },
                new Exercise()
                {
                    DatabaseId = 1,
                    CreatorId = superUserId,
                    ValidAnswer = "SELECT FirstName FROM dbo.Footballers",
                    IsPrivate = false,
                    Title = "Get one field",
                    Description = "Get one field description"
                },
                new Exercise()
                {
                    DatabaseId = 1,
                    CreatorId = superUserId,
                    ValidAnswer = "SELECT FirstName, LastName FROM dbo.Footballers",
                    IsPrivate = false,
                    Title = "Get two fields",
                    Description = "Get two fields description"
                },
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
                    Title = GetPermissionByEnum.GetPermissionName(PermissionNames.SendingInvitations),
                    Description = "This permission lets a user to send invitations to other users"
                },
                new Permission()
                {
                    Title = GetPermissionByEnum.GetPermissionName(PermissionNames.DeletingUsers),
                    Description = "This permission lets a user to delete other users from group"
                },
                new Permission()
                {
                    Title = GetPermissionByEnum.GetPermissionName(PermissionNames.DeletingGroup),
                    Description = "This permission lets a user to delete a group"
                },
                new Permission()
                {
                    Title = GetPermissionByEnum.GetPermissionName(PermissionNames.AssigningExercises),
                    Description = "This permission lets a user to assign exercises to do"
                },
                new Permission()
                {
                    Title = GetPermissionByEnum.GetPermissionName(PermissionNames.CheckingExercises),
                    Description = "This permission lets a user in group to check other exercises"
                },
                new Permission()
                {
                    Title = GetPermissionByEnum.GetPermissionName(PermissionNames.GettingAssignments),
                    Description = "This permission lets a user to get assignments in group"
                }
            };
        }

        public static IEnumerable<GroupRolePermission> GetGroupRolePermissions(IEnumerable<Permission> permissions, IEnumerable<GroupRole> groupRoles)
        {
            GroupRolePermission getGroupRolePermission(string groupRoleName, string permission)
            {
                return new GroupRolePermission
                {
                    GroupRoleId = groupRoles.SingleOrDefault(x => x.Name == groupRoleName).Id,
                    PermissionId = permissions.SingleOrDefault(x => x.Title == permission).Id
                };
            }
            return new List<GroupRolePermission>
            {
                getGroupRolePermission("Owner",GetPermissionByEnum.GetPermissionName(PermissionNames.SendingInvitations)),
                getGroupRolePermission("Owner",GetPermissionByEnum.GetPermissionName(PermissionNames.DeletingUsers)),
                getGroupRolePermission("Owner",GetPermissionByEnum.GetPermissionName(PermissionNames.DeletingGroup)),
                getGroupRolePermission("Owner",GetPermissionByEnum.GetPermissionName(PermissionNames.AssigningExercises)),
                getGroupRolePermission("Owner",GetPermissionByEnum.GetPermissionName(PermissionNames.CheckingExercises)),
                getGroupRolePermission("Owner",GetPermissionByEnum.GetPermissionName(PermissionNames.GettingAssignments)),
                getGroupRolePermission("Moderator", GetPermissionByEnum.GetPermissionName(PermissionNames.SendingInvitations)),
                getGroupRolePermission("Moderator", GetPermissionByEnum.GetPermissionName(PermissionNames.DeletingUsers)),
                getGroupRolePermission("Moderator", GetPermissionByEnum.GetPermissionName(PermissionNames.AssigningExercises)),
                getGroupRolePermission("Moderator", GetPermissionByEnum.GetPermissionName(PermissionNames.CheckingExercises)),
                getGroupRolePermission("Moderator", GetPermissionByEnum.GetPermissionName(PermissionNames.GettingAssignments)),
                getGroupRolePermission("Checker",GetPermissionByEnum.GetPermissionName(PermissionNames.AssigningExercises)),
                getGroupRolePermission("Checker",GetPermissionByEnum.GetPermissionName(PermissionNames.CheckingExercises)),
                getGroupRolePermission("Checker",GetPermissionByEnum.GetPermissionName(PermissionNames.GettingAssignments)),
                getGroupRolePermission("User",GetPermissionByEnum.GetPermissionName(PermissionNames.GettingAssignments))
            };
        }
    }
}
