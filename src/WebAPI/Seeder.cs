using Domain.Common;
using Domain.Entities;
using Domain.Enums;
using Domain.ValueObjects;
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
                    var superUser = getSuperUser(adminRoleId);
                    _context.Users.Add(superUser);
                    _context.SaveChanges();
                }
                if (!_context.Databases.Any())
                {
                    var databases = getDatabases();
                    _context.Databases.AddRange(databases);
                    _context.SaveChanges();
                }
                if (!_context.Exercises.Where(x => !x.IsPrivate).Any())
                {
                    int superUserId = _context.Users.FirstOrDefault(x => x.Email == "superuser@gmail.com").Id;
                    int nortwindSimpleDatabaseId = _context.Databases.SingleOrDefault(x => x.Name == "NorthwindSimple").Id;
                    var exercises = getNorthwindSimplePublicExercises(superUserId, nortwindSimpleDatabaseId);
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

        public static IEnumerable<Exercise> getNorthwindSimplePublicExercises(int superUserId, int databaseId)
        {
            return new List<Exercise>()
            {
                new Exercise()
                {
                    CreatorId = superUserId,
                    DatabaseId = databaseId,
                    IsPrivate = false,
                    Title = "Get all data from table Orders",
                    Description = " Get all the data from table orders without any aliases",
                    ValidAnswer = "SELECT * FROM Orders"
                },
                new Exercise()
                {
                    CreatorId = superUserId,
                    DatabaseId = databaseId,
                    IsPrivate = false,
                    Title = "Get all customers with valid data",
                    Description = "Get all customers with only Id, FirstName and LastName columns "+
                    "with following aliases: id, first, last",
                    ValidAnswer = "SELECT Id id, firstName first, lastName last FROM Customers"
                },
                new Exercise()
                {
                    CreatorId=superUserId,
                    DatabaseId = databaseId,
                    IsPrivate=false,
                    Title="Get all data from table Products sorted",
                    Description = "Get all data from table Products sorted by firstly ProductName "+
                    "and secondly by UnitPrice all in descending type",
                    ValidAnswer = "SELECT Id id, firstName first, lastName last FROM Customers"
                },
                new Exercise()
                {
                    CreatorId = superUserId,
                    DatabaseId = databaseId,
                    IsPrivate = false,
                    Title = "Get data with JOINS",
                    Description = "Get Orders.Id as OrderId, OrderItems.UnitPrice as UnitPriceOrderItem " +
                    "Products.Package without alias with the appropriate JOINS and sort the data by Products.UnitPrice",
                    ValidAnswer = @"Select ord.Id OrderId, oi.UnitPrice UnitPriceOrderItem, p.Package from OrderItems oi
                    INNER JOIN Orders ord ON ord.Id = oi.OrderId
                    INNER JOIN Products p ON p.Id = oi.ProductId
                    ORDER BY p.UnitPrice DESC;"
                },
                new Exercise()
                {
                    CreatorId = superUserId,
                    DatabaseId = databaseId,
                    IsPrivate =false,
                    Title = "Grouping",
                    Description = "For every OrderId in OrderItems table show amount of UnitPrices. "+
                    "Do not use aliases",
                    ValidAnswer = "SELECT OrderId, COUNT(UnitPrice) FROM OrderItems GROUP BY OrderId;"
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

        //The real data needs be specified by user
        public static IEnumerable<Database> getDatabases()
        {
            return new List<Database>()
            {
                new Database()
                {
                    ConnectionString=
                    new ConnectionString("NorthwindSimple Server", "NorthwindSimple database name", "User", "Password"),
                    Name = "NorthwindSimple",
                }
            };
        }

        public static IEnumerable<Permission> getPermissions()
        {
            return new List<Permission>
            {
                new Permission()
                {
                    Title = PermissionEnum.SendingInvitations.GetDisplayName(),
                    Description = "This permission lets a user to send invitations to other users"
                },
                new Permission()
                {
                    Title = PermissionEnum.DeletingUsers.GetDisplayName(),
                    Description = "This permission lets a user to delete other users from group"
                },
                new Permission()
                {
                    Title = PermissionEnum.DeletingGroup.GetDisplayName(),
                    Description = "This permission lets a user to delete a group"
                },
                new Permission()
                {
                    Title = PermissionEnum.AssigningExercises.GetDisplayName(),
                    Description = "This permission lets a user to assign exercises to do"
                },
                new Permission()
                {
                    Title = PermissionEnum.CheckingExercises.GetDisplayName(),
                    Description = "This permission lets a user in group to check other exercises"
                },
                new Permission()
                {
                    Title = PermissionEnum.GettingAssignments.GetDisplayName(),
                    Description = "This permission lets a user to get assignments in group"
                },
                new Permission()
                {
                    Title = PermissionEnum.DoingExercises.GetDisplayName(),
                    Description = "This permission lets a user to do an exercise",
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
                getGroupRolePermission("Owner",PermissionEnum.SendingInvitations.GetDisplayName()),
                getGroupRolePermission("Owner",PermissionEnum.DeletingUsers.GetDisplayName()),
                getGroupRolePermission("Owner",PermissionEnum.DeletingGroup.GetDisplayName()),
                getGroupRolePermission("Owner",PermissionEnum.AssigningExercises.GetDisplayName()),
                getGroupRolePermission("Owner",PermissionEnum.CheckingExercises.GetDisplayName()),
                getGroupRolePermission("Owner",PermissionEnum.GettingAssignments.GetDisplayName()),
                getGroupRolePermission("Moderator", PermissionEnum.SendingInvitations.GetDisplayName()),
                getGroupRolePermission("Moderator", PermissionEnum.DeletingUsers.GetDisplayName()),
                getGroupRolePermission("Moderator", PermissionEnum.AssigningExercises.GetDisplayName()),
                getGroupRolePermission("Moderator", PermissionEnum.CheckingExercises.GetDisplayName()),
                getGroupRolePermission("Moderator", PermissionEnum.GettingAssignments.GetDisplayName()),
                getGroupRolePermission("Checker",PermissionEnum.AssigningExercises.GetDisplayName()),
                getGroupRolePermission("Checker",PermissionEnum.CheckingExercises.GetDisplayName()),
                getGroupRolePermission("Checker",PermissionEnum.GettingAssignments.GetDisplayName()),
                getGroupRolePermission("User",PermissionEnum.GettingAssignments.GetDisplayName()),
                getGroupRolePermission("User",PermissionEnum.DoingExercises.GetDisplayName())
            };
        }
    }
}
