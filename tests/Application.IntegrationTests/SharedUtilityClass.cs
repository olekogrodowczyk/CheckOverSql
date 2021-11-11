using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq.Expressions;
using System.Net.Http;
using System.Threading.Tasks;
using WebAPI.IntegrationTests.Helpers;

namespace WebAPI.IntegrationTests
{
    public class SharedUtilityClass
    {
        protected readonly HttpClient _client;
        protected readonly CustomWebApplicationFactory<Startup> _factory;

        public SharedUtilityClass(CustomWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
            _client = factory.CreateClient();
        }
        protected async Task<T> addNewEntity<T>(T value) where T : class, new()
        {
            var scopeFactory = _factory.Services.GetService<IServiceScopeFactory>();
            using var scope = scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetService<ApplicationDbContext>();

            await context.Set<T>().AddAsync(value);
            await context.SaveChangesAsync();
            return value;
        }
        protected async Task ClearTableInContext<T>() where T : class, new()
        {
            var scopeFactory = _factory.Services.GetService<IServiceScopeFactory>();
            using var scope = scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetService<ApplicationDbContext>();

            context.Set<T>().Clear();
            
            await context.SaveChangesAsync();
        }

        protected async Task<bool> EntityExists<T>(Expression<Func<T,bool>> predicate) where T : class, new()
        {
            var scopeFactory = _factory.Services.GetService<IServiceScopeFactory>();
            using var scope = scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetService<ApplicationDbContext>();

            return await context.Set<T>().AnyAsync(predicate);
        }

        protected async Task SeedUsers()
        {
            var scopeFactory = _factory.Services.GetService<IServiceScopeFactory>();
            using var scope = scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetService<ApplicationDbContext>();
            await SeedDataHelper.SeedUsers(context);
        }
    }
}
