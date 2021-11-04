using Application.Dto.CreateExerciseDto;
using Application.Dto.SendQueryDto;
using Application.IntegrationTests.FakeAuthentication;
using Application.Responses;
using Application.ViewModels;
using Domain.Entities;
using FluentAssertions;
using Infrastructure.Data;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WebAPI;
using WebAPI.IntegrationTests.Helpers;
using Xunit;

namespace WebAPI.IntegrationTests.Controllers
{
    public class ExerciseControllerTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private HttpClient _client;
        private WebApplicationFactory<Startup> _factory;

        public ExerciseControllerTests(WebApplicationFactory<Startup> factory)
        {
            _factory = factory
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    var dbContextOptions = services
                           .SingleOrDefault(service => service.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));
                    services.Remove(dbContextOptions);
                    services.AddDbContext<ApplicationDbContext>(options => options.UseInMemoryDatabase("InMemoryDatabase"));
                    services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();
                    services.AddMvc(option => option.Filters.Add(new FakeUserFilter()));
                });
            });

            var scopeFactory = _factory.Services.GetService<IServiceScopeFactory>();
            using var scope = scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetService<ApplicationDbContext>();

            //Seed necessary data like roles and other databases
            SeedDataHelper.SeedDatabeses(context, "FootballLeague").Wait();
            SeedDataHelper.SeedRoles(context).Wait();

            _client = _factory.CreateClient();
        }
       
        private Exercise getValidExercise(bool isPrivate)
        {
            var model = new Exercise
            {
                DatabaseId = 1,
                Description = "Opis2dsadsa",
                MaxPoints = 1,
                Title = "Zadanie2 title",
                ValidAnswer = "SELECT * FROM dbo.Footballers",
                IsPrivate=isPrivate,  
                CreatorId=1,                
            };
            return model;
        }

        [Fact]
        public async Task GetAll_ForCreatedSampleData_ReturnsOkWithThisData()
        {
            //Arrange
            //Only public exercises can be returned
            var exercise1 = getValidExercise(true);
            var exercise2 = getValidExercise(false);
            var exercise3 = getValidExercise(false);

            var scopeFactory = _factory.Services.GetService<IServiceScopeFactory>();
            using var scope = scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetService<ApplicationDbContext>();

            await context.Exercises.AddAsync(exercise1);
            await context.Exercises.AddAsync(exercise2);
            await context.Exercises.AddAsync(exercise3);
            await context.SaveChangesAsync();

            //Act
            var response = await _client.GetAsync("api/exercise/getallpublic");
            var responseString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<Result<IEnumerable<GetExerciseVm>>>(responseString);

            //Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            result.Value.Should().HaveCount(2);
        }

        [Fact]
        public async Task Create_ForValidDto_ReturnsOk()
        {
            //Arrange
            var exerciseDto = new CreateExerciseDto
            {
                Database = "FootballLeague",
                Description = "Opis2dsadsa",
                MaxPoints = 1,
                Title = "Zadanie2 title",
                ValidAnswer = "SELECT * FROM dbo.Footballers"
            };

            var httpContent = exerciseDto.ToJsonHttpContent();

            //Act
            var response = await _client.PostAsync("api/exercise/create/", httpContent);

            //Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }

        [Fact]
        public async Task Create_ForInvalidDto_ReturnsBadRequest()
        {
            //Arrange
            var exerciseDto = new CreateExerciseDto
            {
                Database = "dsadwqdwq",
                Description = "Opis2dsadsa",
                MaxPoints = 0,
                Title = "Zadanie2 title",
                ValidAnswer = "SELECT * FROM dbo.Footballers"
            };

            var httpContent = exerciseDto.ToJsonHttpContent();

            //Act
            var response = await _client.PostAsync("api/exercise/create/", httpContent);

            //Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Create_ForInvalidQuery_ReturnsForbidden()
        {
            //Arrange
            var exerciseDto = new CreateExerciseDto
            {
                Database = "FootballLeague",
                Description = "Opis2dsadsa",
                MaxPoints = 1,
                Title = "Zadanie2 title",
                ValidAnswer = "INSERT INTO dbo.Footballers (FirstName, LastName) VALUES ('Leo','Messi')"
            };

            var httpContent = exerciseDto.ToJsonHttpContent();

            //Act
            var response = await _client.PostAsync("api/exercise/create/", httpContent);

            //Assert
            //There is no option to send a query which writes something in database
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.Forbidden);
        }


    }
}
