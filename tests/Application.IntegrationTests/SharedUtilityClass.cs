﻿using Application.Identities.Commands.RegisterUser;
using Application.Interfaces;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Threading.Tasks;
using WebAPI.IntegrationTests.Helpers;

namespace WebAPI.IntegrationTests
{
    public partial class SharedUtilityClass
    {
        public static int? CurrentUserId { get; set; }
        protected readonly HttpClient _client;
        protected readonly CustomWebApplicationFactory<Startup> _factory;
        protected readonly IServiceScopeFactory _scopeFactory;


        public SharedUtilityClass(CustomWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
            _client = factory.CreateClient();
            _scopeFactory = _factory.Services.GetService<IServiceScopeFactory>();
        }

        protected async Task<T> AddAsync<T>(T value) where T : class, new()
        {
            using var scope = _scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetService<ApplicationDbContext>();

            await context.Set<T>().AddAsync(value);
            await context.SaveChangesAsync();
            return value;
        }

        protected async Task<IEnumerable<T>> AddRangeAsync<T>(IEnumerable<T> values) where T : class, new()
        {
            using var scope = _scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetService<ApplicationDbContext>();

            await context.Set<T>().AddRangeAsync(values);
            await context.SaveChangesAsync();
            return values;
        }

        protected async Task<bool> AnyAsync<T>(Expression<Func<T, bool>> predicate) where T : class, new()
        {
            using var scope = _scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetService<ApplicationDbContext>();

            return await context.Set<T>().AnyAsync(predicate);
        }

        protected async Task<IEnumerable<T>> WhereAsync<T>(Expression<Func<T, bool>> predicate) where T : class, new()
        {
            using var scope = _scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetService<ApplicationDbContext>();

            return await context.Set<T>().Where(predicate).ToListAsync();
        }

        protected async Task<int> CountAsync<T>() where T : class
        {
            using var scope = _scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            return await context.Set<T>().CountAsync();
        }

        protected async Task<T> FirstOrDefaultAsync<T>(Expression<Func<T, bool>> predicate) where T : class
        {
            using var scope = _scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            return await context.Set<T>().FirstOrDefaultAsync(predicate);
        }

        protected async Task<T> FindAsync<T>(params object[] keyValues) where T : class
        {
            using var scope = _scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            return await context.Set<T>().FindAsync(keyValues);
        }

        protected async Task ClearTableInContext<T>() where T : class, new()
        {
            using var scope = _scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetService<ApplicationDbContext>();

            context.Set<T>().Clear();

            await context.SaveChangesAsync();
        }

        protected async Task ClearNotNecesseryData()
        {
            using var scope = _scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetService<ApplicationDbContext>();

            context.Groups.Clear();
            context.Assignments.Clear();
            context.Users.Clear();
            context.Invitations.Clear();
            context.Solutions.Clear();
            context.Exercises.Clear();
            context.Comparisons.Clear();
            context.Solvings.Clear();
            await context.SaveChangesAsync();
            CurrentUserId = null;
        }

        protected async Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request)
        {
            using var scope = _scopeFactory.CreateScope();
            var mediator = scope.ServiceProvider.GetRequiredService<ISender>();

            return await mediator.Send(request);
        }

        protected async Task<int> RunAsDefaultUserAsync()
        {
            return await RunAsUserAsync("user", "userFirstName", "userLastName", "user@gmail.com", "test123#");
        }

        protected async Task<int> RunAsUserAsync(string login, string firstName, string lastName, string email, string password)
        {
            var potentialExistingUser = await FirstOrDefaultAsync<User>(x => x.Login == login);
            if (potentialExistingUser is not null) { return potentialExistingUser.Id; }

            using var scope = _scopeFactory.CreateScope();
            var user = scope.ServiceProvider.GetRequiredService<IIdentityService>();

            int userId = await user.Register(new RegisterUserCommand
            {
                Login = login,
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                Password = password,
                ConfirmPassword = password,
            });
            CurrentUserId = userId;
            return (int)CurrentUserId;
        }



    }
}
