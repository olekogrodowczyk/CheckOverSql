using Application.Dto.AssignExerciseToUsersTo;
using Application.Dto.CreateExerciseDto;
using Application.Exercises.Commands.CreateExercise;
using Application.Exercises.Queries;
using Application.Exercises.Queries.GetAllCreated;
using Application.Responses;
using Application.Groups;
using Domain.Entities;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.IntegrationTests.Helpers;
using Xunit;

namespace WebAPI.IntegrationTests.Controllers
{
    public class ExerciseControllerTests : SharedUtilityClass, IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        public ExerciseControllerTests(CustomWebApplicationFactory<Startup> factory) : base(factory) { }



        [Fact]
        public async Task GetAll_ForCreatedSampleData_ReturnsOkWithThisData()
        {
            //Arrange
            //Only public exercises can be returned
            await ClearTableInContext<Exercise>();

            var exercise1 = await addNewEntity<Exercise>(getValidExercise(true));
            var exercise2 = await addNewEntity<Exercise>(getValidExercise(false));
            var exercise3 = await addNewEntity<Exercise>(getValidExercise(false));

            //Act

            var response = await _client.GetAsync(ApiRoutes.Exercise.GetAllPublic);
            var result = await response.ToResultAsync<Result<IEnumerable<GetExerciseDto>>>();

            //Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            result.Value.Should().HaveCount(2);
        }

        [Fact]
        public async Task Create_ForValidDto_ReturnsOk()
        {
            //Arrange
            var httpContent = new CreateExerciseCommand
            {
                DatabaseName = "FootballLeague",
                Description = "Opis2dsadsa",
                Title = "Zadanie2 title",
                ValidAnswer = "SELECT * FROM dbo.Footballers",
                IsPrivate = true,
            }.ToJsonHttpContent();

            //Act
            var response = await _client.PostAsync(ApiRoutes.Exercise.Create, httpContent);

            //Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }

        [Fact]
        public async Task Create_ForInvalidDto_ReturnsBadRequest()
        {
            //Arrange
            var httpContent = new CreateExerciseCommand
            {
                DatabaseName = "dsadwqdwq",
                Description = "Opis2dsadsa",
                Title = "Zadanie2 title",
                ValidAnswer = "SELECT * FROM dbo.Footballers"
            }.ToJsonHttpContent();

            //Act
            var response = await _client.PostAsync(ApiRoutes.Exercise.Create, httpContent);

            //Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Create_ForInvalidQuery_ReturnsForbidden()
        {
            //Arrange
            var httpContent = new CreateExerciseCommand
            {
                DatabaseName = "FootballLeague",
                Description = "Opis2dsadsa",
                Title = "Zadanie2 title",
                ValidAnswer = "INSERT INTO dbo.Footballers (FirstName, LastName) VALUES ('Leo','Messi')",
                IsPrivate = true,
            }.ToJsonHttpContent();


            //Act
            var response = await _client.PostAsync(ApiRoutes.Exercise.Create, httpContent);

            //Assert
            //There is no option to send a query which writes something in database
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.Forbidden);
        }

        [Theory]
        [InlineData(1, 1000, System.Net.HttpStatusCode.OK)]
        [InlineData(2, 110, System.Net.HttpStatusCode.OK)]
        [InlineData(3, 150, System.Net.HttpStatusCode.OK)]
        [InlineData(4, 120, System.Net.HttpStatusCode.Forbidden)]
        [InlineData(1, 100, System.Net.HttpStatusCode.OK)]
        [InlineData(1, 30, System.Net.HttpStatusCode.BadRequest)]
        public async Task AssignExerciseToUsersInGroup_ForGivenParameteres_ReturnsValidData
            (int groupRoleId, int deadLineMinutesAhead, System.Net.HttpStatusCode expectedStatusCode)
        {
            //Arrange 
            await ClearNotNecesseryData();
            await SeedUsers();
            var group1 = await addNewEntity<Group>(new Group { Name = "Grupa1", CreatorId = 99 });
            var group2 = await addNewEntity<Group>(new Group { Name = "Grupa1", CreatorId = 100 });
            List<Assignment> assignments = new List<Assignment>()
            {
                await addNewEntity<Assignment>
                ( new Assignment { GroupId = group1.Id, GroupRoleId = groupRoleId, UserId = 99} ),
                await addNewEntity<Assignment>
                (new Assignment { GroupId = group1.Id, GroupRoleId = 4, UserId = 100 }),
                await addNewEntity<Assignment>
                (new Assignment { GroupId = group1.Id, GroupRoleId = 4, UserId = 101 }),
                await addNewEntity<Assignment>
                (new Assignment { GroupId = group1.Id, GroupRoleId = 2, UserId = 102 }),
                await addNewEntity<Assignment>
                (new Assignment { GroupId = group1.Id, GroupRoleId = 4, UserId = 103 }),
                await addNewEntity<Assignment>
                (new Assignment { GroupId = group2.Id, GroupRoleId = 4, UserId = 100 }),
            };
            var exercise = await addNewEntity<Exercise>(getValidExercise(false));
            var httpContent = new AssignExerciseToUsersCommand 
            {
                DeadLine = DateTime.UtcNow.AddMinutes(deadLineMinutesAhead),
                GroupId = group1.Id,
                ExerciseId = exercise.Id,
            }.ToJsonHttpContent();

            //Act
            var query = ApiRoutes.Exercise.AssignExercise + "/" + exercise.Id;
            var response = await _client.PostAsync(query, httpContent);
            var result = await response.ToResultAsync<Result<IEnumerable<int>>>();


            //Assert
            response.StatusCode.Should().Be(expectedStatusCode);
            if (expectedStatusCode == System.Net.HttpStatusCode.OK) { result.Value.Should().HaveCount(3); }
        }
    }
}
