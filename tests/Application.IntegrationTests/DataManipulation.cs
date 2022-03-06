﻿using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPI.IntegrationTests.Helpers;

namespace WebAPI.IntegrationTests
{
    public partial class SharedUtilityClass
    {
        protected async Task SeedPublicExercises()
        {
            using var scope = _scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetService<ApplicationDbContext>();
            await SeedDataHelper.SeedPublicExercises(context);
        }

        protected async Task<Dictionary<string, int>> SeedPermissionWithGroupRoles()
        {
            using var scope = _scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetService<ApplicationDbContext>();
            await SeedDataHelper.SeedPermissionWithGroupRoles(context);
            return await GetGroupRolesDict();
        }

        protected async Task<Dictionary<string, int>> GetGroupRolesDict()
        {
            using var scope = _scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetService<ApplicationDbContext>();
            var groupRolesDict = await context.GroupRoles.ToDictionaryAsync(x => x.Name, x => x.Id);
            return groupRolesDict;
        }

        protected async Task<Dictionary<string, int>> SeedUsers()
        {
            using var scope = _scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetService<ApplicationDbContext>();
            //Include potential logged user
            if (await context.Users.CountAsync() < 2)
            {
                await context.AddRangeAsync(SeedDataHelper.GetUsers());
                await context.SaveChangesAsync();
            }
            return await GetUsersDictionary();
        }

        protected async Task<Dictionary<string, int>> GetUsersDictionary()
        {
            using var scope = _scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetService<ApplicationDbContext>();
            var usersDict = await context.Users.ToDictionaryAsync(x => x.Login, x => x.Id);
            return usersDict;
        }

        protected Exercise GetValidExercise(bool isPrivate = false, int creatorId = 104)
        {
            var model = new Exercise
            {
                DatabaseId = 1,
                Description = "Opis2dsadsa",
                Title = "Zadanie2 title",
                ValidAnswer = "SELECT * FROM dbo.Footballers",
                IsPrivate = isPrivate,
                CreatorId = creatorId,
            };
            return model;
        }
    }
}
