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
    public class SolutionControllerTests : SharedUtilityClass, IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        public SolutionControllerTests(CustomWebApplicationFactory<Startup> factory) : base(factory) { } 
           
        [Fact]
        public async Task Create_ForValidModel_ReturnsOk()
        {
            //Arrange
            var exercise = await addNewEntity<Exercise>(getValidExercise());

            var httpContent = new CreateSolutionDto
            {
                Query = "SELECT * FROM dbo.Footballers",
                SolvingId = null,
                Dialect = "SQL Server"
            }.ToJsonHttpContent();

            //Act
            var response = await _client.PostAsync($"/api/exercise/{exercise.Id}/solution", httpContent);

            //Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }

        [Fact] 
        public async Task GetQueryData_ForForbiddenSolution_ReturnsForbidden()
        {
            //Arrange
            var exercise = await addNewEntity<Exercise>(getValidExercise());
            var solution = await addNewEntity<Solution>(new Solution
            {
                Query = "INSERT INTO dbo.Footballers (FirstName, LastName) VALUES ('Leo', 'Messi')",
                SolvingId = null,
                Dialect = "SQL Server",
                CreatorId = 1,
                ExerciseId = exercise.Id,
            });

            //Act
            var response = await _client.GetAsync($"/api/exercise/{exercise.Id}/solution/getquerydata/{solution.Id}");

            //Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task GetQueryData_ForValidSolution_ReturnsNotNullValue()
        {
            //Arrange
            var exercise = await addNewEntity<Exercise>(getValidExercise());
            var solution = await addNewEntity<Solution>(new Solution
            {
                Query = "SELECT * FROM dbo.Footballers",
                SolvingId = null,
                Dialect = "SQL Server",
                CreatorId = 1,
                ExerciseId = exercise.Id,
            });

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
            var exercise = await addNewEntity<Exercise>(getValidExercise());

            var httpContent = new CreateSolutionDto
            {
                Query = "",
                SolvingId = null,
                Dialect = "SQL Server"
            }.ToJsonHttpContent();

            //Act
            var response = await _client.PostAsync($"/api/exercise/{exercise.Id}/solution", httpContent);

            //Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task GetAll_ForSolutionsCreatedByUser_ReturnsAllTheseSolutions()
        {
            //Arrange
            string query = "SELECT * FROM dbo.Footballers";
            var exercise = await addNewEntity<Exercise>(getValidExercise());
            await addNewEntity<Solution>
                (new Solution { Dialect = "SQL Server", ExerciseId = exercise.Id, CreatorId = 99, Query = query });
            await addNewEntity<Solution>
                (new Solution { Dialect = "SQL Server", ExerciseId = exercise.Id, CreatorId = 2, Query = query });
            await addNewEntity<Solution>
                (new Solution { Dialect = "SQL Server", ExerciseId = exercise.Id, CreatorId = 99, Query = query });
            
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
            string query = "SELECT * FROM dbo.Footballers";
            var exercise = await addNewEntity<Exercise>(getValidExercise());
            var solution = await addNewEntity<Solution>
                (new Solution { Dialect = "SQL Server", ExerciseId = exercise.Id, CreatorId = 99, Query = query });

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
            string query = "SELECT FirstName, LastName FROM dbo.Footballers";
            var exercise = await addNewEntity<Exercise>(getValidExercise());
            var solution = await addNewEntity<Solution>
                (new Solution { Dialect = "SQL Server", ExerciseId = exercise.Id, CreatorId = 99, Query = query });

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
            string query = "SELECT * FROM dbo.Footballers";
            var exercise = await addNewEntity<Exercise>(getValidExercise());
            var solution = await addNewEntity<Solution>
                (new Solution { Dialect = "SQL Server", ExerciseId = exercise.Id, CreatorId = 99, Query = query });         

            //Act
            var response = await _client.GetAsync($"api/exercise/{exercise.Id}/solution/compare/{solution.Id}");
            
            //Assert
            var scopeFactory = _factory.Services.GetService<IServiceScopeFactory>();
            using var scope = scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetService<ApplicationDbContext>();

            bool comparisonExists = await context.Comparisons.AnyAsync();

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            comparisonExists.Should().BeTrue();
        }

        private Exercise getValidExercise()
        {
            return new Exercise
            {
                DatabaseId = 1,
                MaxPoints = 10,
                IsPrivate = false,
                ValidAnswer = "SELECT * FROM dbo.Footballers",
                CreatorId = 99,
                Title = "Zadanie1",
                Description = "Zadanie1opis",
            };
        }
    }
}
