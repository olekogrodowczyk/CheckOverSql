using Application.Dto.CreateSolutionDto;
using Application.IntegrationTests.FakeAuthentication;
using Application.Responses;
using Application.ViewModels;
using Domain.Entities;
using Domain.Enums;
using FluentAssertions;
using Infrastructure.Data;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
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
           
        private async Task<(int,int)> InitForCreate(DateTime deadline)
        {
            await ClearNotNecesseryData();
            await SeedUsers();

            var exercise = await addNewEntity<Exercise>(getValidExercise());
            var group1 = await addNewEntity<Group>(new Group { Name = "Grupa1", CreatorId = 100 });
            var group2 = await addNewEntity<Group>(new Group { Name = "Grupa2", CreatorId = 102 });
            var assignment1 = await addNewEntity<Assignment>(new Assignment
            {
                UserId = 100,
                GroupId = group1.Id,
                GroupRoleId = 1
            });
            var assignment2 = await addNewEntity<Assignment>(new Assignment
            {
                UserId = 99,
                GroupId = group1.Id,
                GroupRoleId = 4
            });
            var assignment3 = await addNewEntity<Assignment>(new Assignment
            {
                UserId = 99,
                GroupId = group2.Id,
                GroupRoleId = 4
            });
            var solving1 = await addNewEntity<Solving>(new Solving
            {
                AssignmentId = assignment2.Id,
                ExerciseId = exercise.Id,
                CreatorId = 100,
                Status = SolvingStatus.ToDo.ToString(),
                DeadLine = deadline,
                SentAt = DateTime.UtcNow
            });
            var solving2 = await addNewEntity<Solving>(new Solving
            {
                AssignmentId = assignment3.Id,
                ExerciseId = exercise.Id,
                CreatorId = 100,
                Status = SolvingStatus.ToDo.ToString(),
                DeadLine = deadline,
                SentAt = DateTime.UtcNow
            });
            return (exercise.Id, solving1.Id);
        }

        [Theory]
        [InlineData("SELECT * FROM dbo.Footballers", true)]
        [InlineData("SELECT FirstName FROM dbo.Footballers", false)]
        [InlineData("SELECT FirstName, LastName FROM dbo.Footballers", false)]
        [InlineData("SELECT Id, FirstName, LastName FROM dbo.Footballers", true)]
        public async Task Create_ForValidData_ReturnsOkWithExpectedResults(string query, bool expectedResult)
        {
            //Arrange
            var initResult = await InitForCreate(DateTime.UtcNow.AddDays(1));

            var httpContent = new CreateSolutionDto { Query = query }.ToJsonHttpContent();

            //Act
            var response = await _client.PostAsync
                (ApiRoutes.Solution.Create.Replace("{exerciseId}", initResult.Item1.ToString()), httpContent);
            var responseString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<Result<GetComparisonVm>>(responseString);


            //Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            result.Value.SolutionSolver.Should().NotBeNullOrEmpty();
            result.Value.Result.Should().Be(expectedResult);
        }

        [Theory]
        [InlineData("INSERT INTO Footballers (FirstName, LastName) VALUES ('Leo','Messi')")]
        [InlineData("dnwuqidnuwqu")]
        [InlineData("DROP TABLE dbo.Footballers")]
        public async Task Create_ForInvalidQueries_ReturnsForbidden(string query)
        {
            //Arrange
            var initResult = await InitForCreate(DateTime.UtcNow.AddDays(1));
            var httpContent = new CreateSolutionDto { Query = query }.ToJsonHttpContent();

            //Act
            var response = await _client.PostAsync
                (ApiRoutes.Solution.Create.Replace("{exerciseId}", initResult.Item1.ToString()), httpContent);

            //Assert
            var scopeFactory = _factory.Services.GetService<IServiceScopeFactory>();
            using var scope = scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetService<ApplicationDbContext>();

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.Forbidden);
            context.Comparisons.Count().Should().Be(0);
        }

        [Fact]
        public async Task Create_ForValidData_ReturnsOkWithProperlyCreatedData()
        {
            //Arrange
            var initResult = await InitForCreate(DateTime.UtcNow.AddDays(1));

            var httpContent = new CreateSolutionDto { Query = "SELECT * FROM dbo.Footballers" }.ToJsonHttpContent();

            //Act
            var response = await _client.PostAsync
                (ApiRoutes.Solution.Create.Replace("{exerciseId}", initResult.Item1.ToString()), httpContent);

            //Assert
            var scopeFactory = _factory.Services.GetService<IServiceScopeFactory>();
            using var scope = scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetService<ApplicationDbContext>();

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            context.Comparisons.Count().Should().Be(1);

            var solvings = await context.Solvings.Include(x=>x.Solution).ToListAsync();
            solvings.ForEach(solving =>
            {
                solving.Status.Should().Be(SolvingStatus.Done.ToString());
                solving.Solution.Outcome.Should().BeTrue();
            });
            context.Assignments.Include(x=>x.Solvings).SelectMany(x=>x.Solvings).Count().Should().Be(2);
        }

        [Fact]
        public async Task Create_ForValidDataWithOverdueSentSolution_ReturnsOkWithSentButOverdueStatus()
        {
            //Arrange
            var initResult = await InitForCreate(DateTime.UtcNow.AddHours(-1));
            var httpContent = new CreateSolutionDto { Query = "SELECT * FROM dbo.Footballers" }.ToJsonHttpContent();


            //Act
            var response = await _client.PostAsync
                (ApiRoutes.Solution.Create.Replace("{exerciseId}", initResult.Item1.ToString()), httpContent);

            //Assert
            var scopeFactory = _factory.Services.GetService<IServiceScopeFactory>();
            using var scope = scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetService<ApplicationDbContext>();

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            context.Comparisons.Count().Should().Be(1);
            var solvingResult = await context.Solvings.Include(x => x.Solution).FirstOrDefaultAsync(x => x.Id == initResult.Item2);
            solvingResult.Status.Should().Be(SolvingStatus.DoneButOverdue.ToString());
            solvingResult.Solution.Outcome.Should().BeTrue();
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
            var route = ApiRoutes.Solution.GetQueryData.Replace("{exerciseId}", exercise.Id.ToString())
                .Replace("{solutionId}", solution.Id.ToString());
            var response = await _client.GetAsync(route);

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
            var route = ApiRoutes.Solution.GetQueryData.Replace("{exerciseId}", exercise.Id.ToString())
                .Replace("{solutionId}", solution.Id.ToString());

            var response = await _client.GetAsync(route);
            var result = await response.ToResultAsync<Result<List<List<string>>>>();

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
            }.ToJsonHttpContent();

            //Act
            var response = await _client.PostAsync
                (ApiRoutes.Solution.Create.Replace("{exerciseId}", exercise.Id.ToString()), httpContent);

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
            var response = await _client.GetAsync(ApiRoutes.Solution.GetAll.Replace("{exerciseId}", exercise.Id.ToString()));
            var result = await response.ToResultAsync<Result<IEnumerable<GetSolutionVm>>>();

            //Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            result.Value.Should().HaveCount(2);
        }


       

        
    }
}
