using Domain.Entities;
using Infrastructure.Data;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.IntegrationTests.Helpers
{
    public static class SeedDataHelper
    {
        public static async Task SeedDatabases(ApplicationDbContext context, string databaseName)
        {
            if(!context.Databases.Any())
            {
                string readOnlyConnectionString = GetSecretDataHelper.GetDatabaseReadOnlyConnectionString(databaseName);
                Database newDatabase = new Database
                {
                    Id = 1,
                    Name = databaseName,
                    ConnectionString = readOnlyConnectionString,
                };
                await context.Databases.AddAsync(newDatabase);
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
    }
}
