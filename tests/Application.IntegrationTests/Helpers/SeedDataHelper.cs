using Application.IntegrationTests.Helpers;
using Domain.Entities;
using Domain.ValueObjects;
using Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Seeders;

namespace WebAPI.IntegrationTests.Helpers
{
    public static class SeedDataHelper
    {
        public static async Task SeedDatabase(ApplicationDbContext context, string databaseName, int databaseId)
        {
            if (!(await context.Databases.AnyAsync(x => x.Name == databaseName)))
            {
                Database newDatabase = new Database
                {
                    Id = databaseId,
                    Name = databaseName,
                    ConnectionString = GetSecretDataHelper.GetConnectionString(databaseName)
                };
                await context.Databases.AddAsync(newDatabase);
                await context.SaveChangesAsync();
            }
        }


        public static async Task SeedRoles(ApplicationDbContext context)
        {
            if (!context.Roles.Any())
            {
                await context.Roles.AddAsync(new Role { Id = 1, Name = "Admin" });
                await context.Roles.AddAsync(new Role { Id = 2, Name = "User" });
                await context.SaveChangesAsync();
            }
        }

        public static async Task SeedPublicExercises(ApplicationDbContext context)
        {
            if (!context.Exercises.Any())
            {
                int? superUserId = (await context.Users.FirstOrDefaultAsync(x => x.Email == "superuser@gmail.com")).Id;
                var northWindSimpleDatabaseId = (await context.Databases.SingleOrDefaultAsync(x => x.Name == "NorthwindSimple")).Id;
                var exercises = Seeders.ExercisesSeederData.GetNorthwindSimplePublicExercises(superUserId ?? 0, northWindSimpleDatabaseId);
                await context.Exercises.AddRangeAsync(exercises);
            }
        }

        public static async Task SeedPermissionWithGroupRoles(ApplicationDbContext context)
        {
            var permissions = PermissionsSeederData.GetPermissions();
            var groupRoles = GroupRolesSeederData.GetGroupRoles();
            if (!await context.Permissions.AnyAsync())
            {
                await context.Permissions.AddRangeAsync(permissions);
            }
            if (!await context.GroupRoles.AnyAsync())
            {
                await context.GroupRoles.AddRangeAsync(groupRoles);
            }
            await context.SaveChangesAsync();

            var groupRolesPermissions = GroupRolePermissionsSeederData.GetGroupRolePermissions(permissions, groupRoles);

            if (!await context.GroupRolePermissions.AnyAsync())
            {
                await context.GroupRolePermissions.AddRangeAsync(groupRolesPermissions);
                await context.SaveChangesAsync();
            }

        }

        public static List<User> GetUsers()
        {
            var users = new List<User>
            {
                new User
                {
                    Id = 100,
                    Login = "user1",
                    FirstName = "John",
                    LastName = "Smith",
                    RoleId = 2,
                    Email = "johnsmith@gmail.com",
                    PasswordHash = "dsandnsauindasuidnusa",
                    DateOfBirth = DateTime.UtcNow.AddYears(-20)
                },
                new User
                {
                    Id = 101,
                    Login = "user2",
                    FirstName = "James",
                    LastName = "Kowalski",
                    RoleId = 2,
                    Email = "jameskowalski@gmail.com",
                    PasswordHash = "dsandnsauindasuidnusa",
                    DateOfBirth = DateTime.UtcNow.AddYears(-21)
                },
                new User
                {
                    Id = 102,
                    Login = "user3",
                    FirstName = "Richard",
                    LastName = "Johnson",
                    RoleId = 2,
                    Email = "richardjohnson@gmail.com",
                    PasswordHash = "dsandnsauindasuidnusa",
                    DateOfBirth = DateTime.UtcNow.AddYears(-22)
                },
                new User
                {
                    Id = 103,
                    Login = "user4",
                    FirstName = "Michael",
                    LastName = "Brown",
                    RoleId = 2,
                    Email = "michaelbrown@gmail.com",
                    PasswordHash = "dsandnsauindasuidnusa",
                    DateOfBirth = DateTime.UtcNow.AddYears(-23)
                },
            };
            return users;

        }
    }
}
