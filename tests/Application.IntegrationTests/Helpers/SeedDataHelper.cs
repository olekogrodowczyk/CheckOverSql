using Domain.Entities;
using Infrastructure.Data;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.IntegrationTests.Helpers
{
    public static class SeedDataHelper
    {
        public static async Task SeedData(ApplicationDbContext context, string databaseName)
        {
            string readOnlyConnectionString = GetSecretDataHelper.GetDatabaseReadOnlyConnectionString(databaseName);
            Database newDatabase = new Database
            {
                Id = 1,
                Name = databaseName,
                ConnectionString = readOnlyConnectionString,
            };
            await context.Databases.AddAsync(newDatabase);
            if(!context.Roles.Any())
            {
                await context.Roles.AddAsync(new Role { Id = 1, Name = "Admin" });
                await context.Roles.AddAsync(new Role { Id = 2, Name = "User" });
                await context.SaveChangesAsync();
            }
            
        }
    }
}
