using Application.Dto.CreateExerciseDto;
using Application.Interfaces;
using Application.Responses;
using Application.Groups;
using Domain.Entities;
using FluentAssertions;
using Infrastructure.Data;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WebAPI;
using WebAPI.IntegrationTests.Helpers;
using Xunit;
[assembly: CollectionBehavior(CollectionBehavior.CollectionPerAssembly)]

namespace WebAPI.IntegrationTests
{
    public class CustomWebApplicationFactory<TStartup>
    : WebApplicationFactory<TStartup> where TStartup : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var dbContextOptions = services
                           .SingleOrDefault(service => service.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));
                services.Remove(dbContextOptions);
                services.AddDbContext<ApplicationDbContext>(options => options.UseInMemoryDatabase("InMemoryDatabase"));

                var currentUserServiceDescriptor = services.FirstOrDefault(d =>
                      d.ServiceType == typeof(IUserContextService));
                if (currentUserServiceDescriptor != null)
                {
                    services.Remove(currentUserServiceDescriptor);
                }
                services.AddTransient(provider =>
                Mock.Of<IUserContextService>(s => s.GetUserId == SharedUtilityClass.CurrentUserId));
                services.BuildServiceProvider();

                var sp = services.BuildServiceProvider();

                using (var scope = sp.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;
                    var context = scopedServices.GetRequiredService<ApplicationDbContext>();

                    context.Database.EnsureCreated();
                    SeedDataHelper.SeedDatabases(context, "NorthwindSimple").Wait();
                }
            });
        }


    }
}
