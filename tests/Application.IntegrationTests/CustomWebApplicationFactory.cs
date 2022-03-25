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
using Application.IntegrationTests.Helpers;
using System.Security.Claims;
using Application.Common.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;

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
                Mock.Of<IUserContextService>(s => s.UserClaimPrincipal == getClaims()
                && s.GetUserId == SharedUtilityClass.CurrentUserId));

                IFormFile file = new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("dummy image")), 0, 0, "Data", "image.png");
                var uploadMockService = Mock.Of<IUploadFileService>();
                Mock.Get(uploadMockService).Setup(x => x.UploadFile(file, It.IsAny<string>())).ReturnsAsync("image.png");
                services.AddScoped<IUploadFileService>(provider =>
                uploadMockService);


                services.BuildServiceProvider();

                var sp = services.BuildServiceProvider();

                using (var scope = sp.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;
                    var context = scopedServices.GetRequiredService<ApplicationDbContext>();

                    context.Database.EnsureCreated();
                    SeedDataHelper.SeedDatabase(context, "NorthwindSimple", DatabasesIdsHelper.NorthwindSimpleDatabaseId)
                    .Wait();
                }
            });
        }

        private ClaimsPrincipal getClaims()
        {
            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.NameIdentifier, SharedUtilityClass.CurrentUserId.ToString()));
            var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(claims));
            return claimsPrincipal;
        }


    }
}
