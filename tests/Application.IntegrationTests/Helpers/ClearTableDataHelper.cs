using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace WebAPI.IntegrationTests.Helpers
{
    public static class ClearTableDataHelper
    {
        public static async Task cleanTable<T>(WebApplicationFactory<Startup> factory) where T : class
        {
            var scopeFactory = factory.Services.GetService<IServiceScopeFactory>();
            using var scope = scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetService<ApplicationDbContext>();

            context.Groups.Clear();
            await context.SaveChangesAsync();
        }
    }
}
