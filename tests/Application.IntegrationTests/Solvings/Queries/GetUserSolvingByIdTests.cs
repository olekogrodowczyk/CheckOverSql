using Application.Common.Exceptions;
using Application.Groups.Queries;
using Application.Groups.Queries.GetUserSolvingById;
using Application.IntegrationTests.Helpers;
using Domain.Entities;
using Domain.Enums;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPI;
using WebAPI.IntegrationTests;
using Xunit;

namespace Application.IntegrationTests.Solvings.Queries
{
    public class GetUserSolvingByIdTests : SharedUtilityClass, IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        public GetUserSolvingByIdTests(CustomWebApplicationFactory<Startup> factory) : base(factory)
        {
        }

        [Fact]
        public async Task ForValidData_ReturnsNonEmptySolvingDto()
        {
            //Arrange
            await ClearNotNecesseryData();
            var user = await RunAsDefaultUserAsync();
            var users = await SeedUsers();
            var groupRoles = await SeedPermissionWithGroupRoles();
            var group = await AddAsync(new Group { Name = "Grupa1", CreatorId = users["user2"] });
            var group2 = await AddAsync(new Group { Name = "Grupa2", CreatorId = users["user3"] });
            var assignmentSolver = await AddAsync(new Assignment(users["user4"], group.Id, groupRoles["User"]));
            var assignmentTest = await AddAsync(new Assignment(user, group.Id, groupRoles["Checker"]));
            var assignmentTest2 = await AddAsync(new Assignment(user, group2.Id, groupRoles["Moderator"]));
            var exercise = await AddAsync(GetValidNoDataReturnsExercise(DatabasesIdsHelper.NorthwindSimpleDatabaseId));
            var solving = await AddAsync(new Solving
            {
                CreatorId = users["user3"],
                Status = SolvingStatusEnum.Done,
                AssignmentId = assignmentSolver.Id,
                ExerciseId = exercise.Id
            });

            //Act
            var result = await SendAsync(new GetUserSolvingByIdQuery { SolvingId = solving.Id });

            //Assert
            result.Should().NotBeNull();
        }

        [Fact]
        public async Task ForSolvingAssignedToUser_ReturnsNonEmptySolvingDto()
        {
            //Arrange
            await ClearNotNecesseryData();
            var user = await RunAsDefaultUserAsync();
            var users = await SeedUsers();
            var groupRoles = await SeedPermissionWithGroupRoles();
            var group = await AddAsync(new Group { Name = "Grupa1", CreatorId = users["user2"] });
            var assignmentSolver = await AddAsync(new Assignment
            {
                UserId = user,
                GroupRoleId = groupRoles["User"],
                GroupId = group.Id
            });
            var exercise = await AddAsync(GetValidNorthwindSimpleExercise());
            var solving = await AddAsync(new Solving
            {
                CreatorId = user,
                Status = SolvingStatusEnum.Done,
                AssignmentId = assignmentSolver.Id,
                ExerciseId = exercise.Id
            });

            //Act
            var result = await SendAsync(new GetUserSolvingByIdQuery { SolvingId = solving.Id });

            //Assert
            result.Should().NotBeNull();
        }

        [Fact]
        public async Task ForInvalidRole_ThrowsForbiddenException()
        {
            //Arrange
            await ClearNotNecesseryData();
            var user = await RunAsDefaultUserAsync();
            var users = await SeedUsers();
            var groupRoles = await SeedPermissionWithGroupRoles();

            var group = await AddAsync(new Group { Name = "Grupa1", CreatorId = users["user2"] });
            var assignmentSolver = await AddAsync(new Assignment(users["user4"], group.Id, groupRoles["User"]));
            var assignmentTest = await AddAsync(new Assignment(user, group.Id, groupRoles["User"]));
            var exercise = await AddAsync(GetValidNorthwindSimpleExercise());
            var solving = await AddAsync(new Solving
            {
                CreatorId = users["user3"],
                Status = SolvingStatusEnum.Done,
                AssignmentId = assignmentSolver.Id,
                ExerciseId = exercise.Id
            });

            //Act
            Func<Task<GetSolvingDto>> func = async () => await SendAsync(new GetUserSolvingByIdQuery { SolvingId = solving.Id });

            //Assert
            await func.Should().ThrowAsync<ForbidException>();
        }

        [Fact]
        public async Task ForNotBeingInGroup_ThrowsForbiddenException()
        {
            //Arrange
            await ClearNotNecesseryData();
            var user = await RunAsDefaultUserAsync();
            var users = await SeedUsers();
            var groupRoles = await SeedPermissionWithGroupRoles();

            var group = await AddAsync(new Group { Name = "Grupa1", CreatorId = users["user2"] });
            var group2 = await AddAsync(new Group { Name = "Grupa1", CreatorId = users["user1"] });
            var assignmentSolver = await AddAsync(new Assignment(users["user4"], group.Id, groupRoles["User"]));
            var assignmentTest = await AddAsync(new Assignment(user, group2.Id, groupRoles["Moderator"]));

            var exercise = await AddAsync(GetValidNorthwindSimpleExercise());
            var solving = await AddAsync(new Solving
            {
                CreatorId = users["user3"],
                Status = SolvingStatusEnum.Done,
                AssignmentId = assignmentSolver.Id,
                ExerciseId = exercise.Id
            });

            //Act
            Func<Task<GetSolvingDto>> func = async () => await SendAsync(new GetUserSolvingByIdQuery { SolvingId = solving.Id });

            //Assert
            await func.Should().ThrowAsync<ForbidException>();
        }

        [Fact]
        public async Task ForNonExistingSolving_ThrowsNotFoundException()
        {
            //Arrange
            await ClearNotNecesseryData();
            var user = await RunAsDefaultUserAsync();
            var users = await SeedUsers();
            var groupRoles = await SeedPermissionWithGroupRoles();
            var exercise = await AddAsync(GetValidNorthwindSimpleExercise());
            var group = await AddAsync(new Group { Name = "Grupa1", CreatorId = users["user2"] });
            var assignmentSolver = await AddAsync(new Assignment(users["user4"], group.Id, groupRoles["User"]));

            var solving = await AddAsync(new Solving
            {
                CreatorId = users["user3"],
                Status = SolvingStatusEnum.Done,
                AssignmentId = assignmentSolver.Id,
                ExerciseId = exercise.Id
            });

            //Act
            Func<Task<GetSolvingDto>> func = async () => await SendAsync(new GetUserSolvingByIdQuery { SolvingId = solving.Id + 1 });

            //Assert
            await func.Should().ThrowAsync<NotFoundException>();
        }
    }
}
