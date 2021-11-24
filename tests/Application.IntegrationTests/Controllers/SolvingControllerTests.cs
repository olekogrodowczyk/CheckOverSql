using Application.Responses;
using Application.Groups;
using Application.Groups.Queries;
using Domain.Entities;
using Domain.Enums;
using FluentAssertions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.IntegrationTests.Helpers;
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
                Status = SolvingStatus.Done.ToString(),
                AssignmentId = assignmentSolver.Id,
                ExerciseId = exercise.Id
            });

            //Act
            var response = await _client.GetAsync($"/api/solving/getbyid/{solving.Id}");
            var result = await response.ToResultAsync<Result<GetSolvingDto>>();

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
                Status = SolvingStatus.Done.ToString(),
                AssignmentId = assignmentSolver.Id,
                ExerciseId = exercise.Id
            });

            //Act
            var response = await _client.GetAsync($"/api/solving/getbyid/{solving.Id}");
            var result = await response.ToResultAsync<Result<GetSolvingDto>>();

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
                Status = SolvingStatus.Done.ToString(),
                AssignmentId = assignmentSolver.Id,
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
                Status = SolvingStatus.Done.ToString(),
                AssignmentId = assignmentSolver.Id,
                ExerciseId = exercise.Id
            });

            //Act
            var response = await _client.GetAsync($"/api/solving/getbyid/{solving.Id}");

            //Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task GetAllSolvingsAssignedToUserToDo_ForGivenData_ReturnsOkWithProperDataAmount()
        {
            //Arrange
            await ClearNotNecesseryData();
            await SeedUsers();

            var group = await addNewEntity<Group>(new Group { Name = "Grupa1", CreatorId = 101 });
            var assignment = await addNewEntity<Assignment>(new Assignment
            {
                UserId = 99,
                GroupRoleId = 2,
                GroupId = group.Id
            });
            List<Exercise> exercises = new List<Exercise>();
            for(int i = 0; i < 10; i++) { exercises.Add(await addNewEntity<Exercise>(getValidExercise())); }
            List<Solving> solvings = new List<Solving>()
            {
                await addNewEntity<Solving>(new Solving{ CreatorId = 102, Status = SolvingStatus.Done.ToString()
                , AssignmentId = assignment.Id, ExerciseId = exercises[0].Id}),
                await addNewEntity<Solving>(new Solving{ CreatorId = 103, Status = SolvingStatus.ToDo.ToString()
                , AssignmentId = assignment.Id, ExerciseId = exercises[1].Id}),
                await addNewEntity<Solving>(new Solving{ CreatorId = 102, Status = SolvingStatus.DoneButOverdue.ToString()
                , AssignmentId = assignment.Id, ExerciseId = exercises[2].Id}),
                await addNewEntity<Solving>(new Solving{ CreatorId = 100, Status = SolvingStatus.ToDo.ToString()
                , AssignmentId = assignment.Id, ExerciseId = exercises[3].Id}),
                await addNewEntity<Solving>(new Solving{ CreatorId = 101, Status = SolvingStatus.Done.ToString()
                , AssignmentId = assignment.Id, ExerciseId = exercises[4].Id}),
                await addNewEntity<Solving>(new Solving{ CreatorId = 102, Status = SolvingStatus.ToDo.ToString()
                , AssignmentId = assignment.Id, ExerciseId = exercises[5].Id}),
                await addNewEntity<Solving>(new Solving{ CreatorId = 102, Status = SolvingStatus.DoneButOverdue.ToString()
                , AssignmentId = assignment.Id, ExerciseId = exercises[6].Id}),
                await addNewEntity<Solving>(new Solving{ CreatorId = 102, Status = SolvingStatus.ToDo.ToString()
                , AssignmentId = assignment.Id, ExerciseId = exercises[7].Id}),
                await addNewEntity<Solving>(new Solving{ CreatorId = 103, Status = SolvingStatus.ToDo.ToString()
                , AssignmentId = assignment.Id, ExerciseId = exercises[8].Id}),
                await addNewEntity<Solving>(new Solving{ CreatorId = 100, Status = SolvingStatus.Done.ToString()
                , AssignmentId = assignment.Id, ExerciseId = exercises[9].Id}),
            };

            //Act
            var responseGetAll = await _client.GetAsync(ApiRoutes.Solving.GetAllSolvingsAssignedToUser);
            var responseGetAllResult = await responseGetAll.ToResultAsync<Result<IEnumerable<GetSolvingDto>>>();

            var responseGetAllToDo = await _client.GetAsync(ApiRoutes.Solving.GetAllSolvingsAssignedToUserToDo);
            var responseGetAllToDoResult = await responseGetAllToDo.ToResultAsync<Result<IEnumerable<GetSolvingDto>>>();

            //Assert
            responseGetAll.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            responseGetAllToDo.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

            responseGetAllResult.Value.Should().HaveCount(10);
            responseGetAllToDoResult.Value.Should().HaveCount(5);

        }

        
    }
}
