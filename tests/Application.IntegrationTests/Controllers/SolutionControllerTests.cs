using Application.Dto.CreateSolutionDto;
using Application.IntegrationTests.FakeAuthentication;
using Application.Responses;
using Application.ViewModels;
using Domain.Entities;
using FluentAssertions;
using Infrastructure.Data;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using WebAPI.IntegrationTests.Helpers;
using Xunit;


namespace WebAPI.IntegrationTests.Controllers
{
    public class SolutionControllerTests : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly HttpClient _client;
        private readonly CustomWebApplicationFactory<Startup> _factory;

        public SolutionControllerTests(CustomWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
            _client = factory.CreateClient();
        }   

        private Solution getSolution(Exercise exercise, int creatorId)
        {
            return new Solution
            {
                Dialect = "SQL Server",
                Exercise = exercise,
                CreatorId = creatorId,
                ExerciseId = exercise.Id,
                Query = "SELECT * FROM dbo.Footballers",
            };
        }

        private Exercise getValidExercise()
        {
            return new Exercise
            {
                DatabaseId = 1,
                MaxPoints = 10,
                IsPrivate = false,
                ValidAnswer = "SELECT * FROM dbo.Footballers",
                CreatorId = 1,
                Title = "Zadanie1",
                Description = "Zadanie1opis",
            };
        }

        [Fact]
        public async Task Create_ForValidModel_ReturnsOk()
        {
            //Arrange
            var scopeFactory = _factory.Services.GetService<IServiceScopeFactory>();
            using var scope = scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetService<ApplicationDbContext>();

            var solution = new CreateSolutionDto
            {
                Query = "SELECT * FROM dbo.Footballers",
                SolvingId = null,
                Dialect = "SQL Server"
            };
            var exercise = getValidExercise();
            await context.Exercises.AddAsync(exercise);
            await context.SaveChangesAsync();

            var httpContent = solution.ToJsonHttpContent();

            //Act
            var response = await _client.PostAsync($"/api/exercise/{exercise.Id}/solution", httpContent);

            //Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }

        [Fact] 
        public async Task GetQueryData_ForForbiddenSolution_ReturnsForbidden()
        {
            //Arrange
            var scopeFactory = _factory.Services.GetService<IServiceScopeFactory>();
            using var scope = scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetService<ApplicationDbContext>();
           
            var exercise = getValidExercise();
            await context.Exercises.AddAsync(exercise);
           
            var solution = new Solution
            {
                Query = "INSERT INTO dbo.Footballers (FirstName, LastName) VALUES ('Leo', 'Messi')",
                SolvingId = null,
                Dialect = "SQL Server",
                CreatorId = 1,
                ExerciseId = exercise.Id,                
            };
            await context.Solutions.AddAsync(solution);
            await context.SaveChangesAsync();

            //Act
            var response = await _client.GetAsync($"/api/exercise/{exercise.Id}/solution/getquerydata/{solution.Id}");

            //Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task GetQueryData_ForValidSolution_ReturnsNotNullValue()
        {
            //Arrange
            var scopeFactory = _factory.Services.GetService<IServiceScopeFactory>();
            using var scope = scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetService<ApplicationDbContext>();

            var exercise = getValidExercise();
            await context.Exercises.AddAsync(exercise);

            var solution = new Solution
            {
                Query = "SELECT * FROM dbo.Footballers",
                SolvingId = null,
                Dialect = "SQL Server",
                CreatorId = 1,
                ExerciseId = exercise.Id,
            };
            await context.Solutions.AddAsync(solution);
            await context.SaveChangesAsync();

            //Act
            var response = await _client.GetAsync($"/api/exercise/{exercise.Id}/solution/getquerydata/{solution.Id}");
            var responseString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<Result<List<List<string>>>>(responseString);

            //Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            result.Value.Should().NotBeNull();
        }

        [Fact]
        public async Task Create_ForInvalidModel_ReturnsBadRequest()
        {
            //Arrange
            var scopeFactory = _factory.Services.GetService<IServiceScopeFactory>();
            using var scope = scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetService<ApplicationDbContext>();

            var solutionDto = new CreateSolutionDto
            {
                Query = "",
                SolvingId = null,
                Dialect = "SQL Server"
            };
            var exercise = getValidExercise();
            await context.Exercises.AddAsync(exercise);
            await context.SaveChangesAsync();

            var httpContent = solutionDto.ToJsonHttpContent();

            //Act
            var response = await _client.PostAsync($"/api/exercise/{exercise.Id}/solution", httpContent);

            //Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task GetAll_ForSolutionsCreatedByUser_ReturnsAllTheseSolutions()
        {
            //Arrange
            var scopeFactory = _factory.Services.GetService<IServiceScopeFactory>();
            using var scope = scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetService<ApplicationDbContext>();

            var exercise = getValidExercise();

            await context.Exercises.AddAsync(exercise);
            await context.Solutions.AddAsync(getSolution(exercise,1));
            await context.Solutions.AddAsync(getSolution(exercise,2));
            await context.Solutions.AddAsync(getSolution(exercise,1));
            await context.SaveChangesAsync();
            
            //Act
            var response = await _client.GetAsync($"/api/exercise/{exercise.Id}/solution/getall");
            var responseString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<Result<IEnumerable<GetSolutionVm>>>(responseString);

            //Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            result.Value.Should().HaveCount(2);
        }

        [Fact]
        public async Task Compare_ForValidSolution_ReturnsTrueWithOk()
        {
            //Arrange
            var scopeFactory = _factory.Services.GetService<IServiceScopeFactory>();
            using var scope = scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetService<ApplicationDbContext>();

            var exercise = getValidExercise();
            await context.Exercises.AddAsync(exercise);

            var solution = getSolution(exercise, 1);
            await context.Solutions.AddAsync(solution);
            await context.SaveChangesAsync();

            //Act
            var response = await _client.GetAsync($"/api/exercise/{exercise.Id}/solution/compare/{solution.Id}");
            var responseString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<Result<bool>>(responseString);

            //Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            result.Value.Should().BeTrue();
        }

        [Fact]
        public async Task Compare_ForInvalidSolution_ReturnsFalseWithOk()
        {
            //Arrange
            var scopeFactory = _factory.Services.GetService<IServiceScopeFactory>();
            using var scope = scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetService<ApplicationDbContext>();

            var exercise = getValidExercise();
            await context.Exercises.AddAsync(exercise);

            var solution = new Solution
            {
                Exercise = exercise,
                CreatorId = 1,
                Query = "SELECT FirstName, LastName FROM dbo.Footballers",
                Dialect = "SQL Server"
            };
            await context.Solutions.AddAsync(solution);
            await context.SaveChangesAsync();

            //Act
            var response = await _client.GetAsync($"/api/exercise/{exercise.Id}/solution/compare/{solution.Id}");
            var responseString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<Result<bool>>(responseString);

            //Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            result.Value.Should().BeFalse();
        }

        [Fact]
        public async Task Compare_ForMakingComparison_CreatesComparison()
        {
            //Arrange
            var exercise = getValidExercise();
            var solution = getSolution(exercise, 1);

            var scopeFactory = _factory.Services.GetService<IServiceScopeFactory>();
            using var scope = scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetService<ApplicationDbContext>();

            await context.AddAsync(exercise);
            await context.SaveChangesAsync();
            await context.AddAsync(solution);
            await context.SaveChangesAsync();

            //Act
            var response = await _client.GetAsync($"api/exercise/{exercise.Id}/solution/compare/{solution.Id}");
            bool comparisonExists = await context.Comparisons.AnyAsync();

            //Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            comparisonExists.Should().BeTrue();
        }
    }
}
