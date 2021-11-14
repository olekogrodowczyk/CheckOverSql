using Application.Responses;
using Application.ViewModels;
using Domain.Entities;
using Domain.Enums;
using FluentAssertions;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using Xunit;

namespace WebAPI.IntegrationTests.Controllers
{
    public class SolvingControllerTests : SharedUtilityClass, IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        public SolvingControllerTests(CustomWebApplicationFactory<Startup> factory) : base(factory) { }

        [Fact]
        public async Task GetSolutionById_ForValidData_ReturnsOkWithNonEmptyResult()
        {
            //Arrange
            await ClearNotNecesseryData();
            await SeedUsers();

            var group = await addNewEntity<Group>(new Group { Name = "Grupa1", CreatorId = 101 });
            var group2 = await addNewEntity<Group>(new Group { Name = "Grupa2", CreatorId = 102 });
            var assignmentSolver = await addNewEntity<Assignment>(new Assignment
            {
                UserId = 103, GroupRoleId = 4, GroupId = group.Id
            });
            var assignmentTest = await addNewEntity<Assignment>(new Assignment
            {
                UserId = 99, GroupRoleId = 3, GroupId = group.Id
            });
            var assignmentTest2 = await addNewEntity<Assignment>(new Assignment
            {
                UserId = 99, GroupRoleId = 2, GroupId = group2.Id
            });
            var exercise = await addNewEntity<Exercise>(getValidExercise());
            var solving = await addNewEntity<Solving>(new Solving
            {
                CreatorId = 102,
                Status = SolvingStatus.ToCheck.ToString(),
                Answer = "SELECT * FROM dbo.Footballers",
                AssignmentId = assignmentSolver.Id,
                Dialect = "SQL Server",
                ExerciseId = exercise.Id
            });

            //Act
            var response = await _client.GetAsync($"/api/solving/getbyid/{solving.Id}");
            var responseString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<Result<GetSolvingVm>>(responseString);

            //Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            result.Value.Should().NotBeNull();
        }

        [Fact]
        public async Task GetSolutionById_ForSolvingAssignedToUser_ReturnsOkWithNonEmptyResult()
        {
            //Arrange
            await ClearNotNecesseryData();
            await SeedUsers();

            var group = await addNewEntity<Group>(new Group { Name = "Grupa1", CreatorId = 101 });
            var assignmentSolver = await addNewEntity<Assignment>(new Assignment
            {
                UserId = 99,
                GroupRoleId = 4,
                GroupId = group.Id
            });
            var exercise = await addNewEntity<Exercise>(getValidExercise());
            var solving = await addNewEntity<Solving>(new Solving
            {
                CreatorId = 99,
                Status = SolvingStatus.ToCheck.ToString(),
                Answer = "SELECT * FROM dbo.Footballers",
                AssignmentId = assignmentSolver.Id,
                Dialect = "SQL Server",
                ExerciseId = exercise.Id
            });

            //Act
            var response = await _client.GetAsync($"/api/solving/getbyid/{solving.Id}");
            var responseString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<Result<GetSolvingVm>>(responseString);

            //Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            result.Value.Should().NotBeNull();
        }

        [Fact]
        public async Task GetSolutionById_ForInvalidRole_ReturnsForbidden()
        {
            //Arrange
            await ClearNotNecesseryData();
            await SeedUsers();

            var group = await addNewEntity<Group>(new Group { Name = "Grupa1", CreatorId = 101 });
            var assignmentSolver = await addNewEntity<Assignment>(new Assignment
            {
                UserId = 103, GroupRoleId = 4, GroupId = group.Id
            });
            var assignmentTest = await addNewEntity<Assignment>(new Assignment
            {
                UserId = 99, GroupRoleId = 4, GroupId = group.Id
            });
            var exercise = await addNewEntity<Exercise>(getValidExercise());
            var solving = await addNewEntity<Solving>(new Solving
            {
                CreatorId = 102,
                Status = SolvingStatus.ToCheck.ToString(),
                Answer = "SELECT * FROM dbo.Footballers",
                AssignmentId = assignmentSolver.Id,
                Dialect = "SQL Server",
                ExerciseId = exercise.Id
            });

            //Act
            var response = await _client.GetAsync($"/api/solving/getbyid/{solving.Id}");

            //Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task GetSolutionById_ForNotBeingInGroup_ReturnsForbidden()
        {
            //Arrange
            await ClearNotNecesseryData();
            await SeedUsers();

            var group = await addNewEntity<Group>(new Group { Name = "Grupa1", CreatorId = 101 });
            var group2 = await addNewEntity<Group>(new Group { Name = "Grupa1", CreatorId = 100 });
            var assignmentSolver = await addNewEntity<Assignment>(new Assignment
            {
                UserId = 103,
                GroupRoleId = 4,
                GroupId = group.Id
            });
            var assignmentTest = await addNewEntity<Assignment>(new Assignment
            {
                UserId = 99,
                GroupRoleId = 2,
                GroupId = group2.Id
            });
            var exercise = await addNewEntity<Exercise>(getValidExercise());
            var solving = await addNewEntity<Solving>(new Solving
            {
                CreatorId = 102,
                Status = SolvingStatus.ToCheck.ToString(),
                Answer = "SELECT * FROM dbo.Footballers",
                AssignmentId = assignmentSolver.Id,
                Dialect = "SQL Server",
                ExerciseId = exercise.Id
            });

            //Act
            var response = await _client.GetAsync($"/api/solving/getbyid/{solving.Id}");

            //Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.Forbidden);
        }
    }
}
