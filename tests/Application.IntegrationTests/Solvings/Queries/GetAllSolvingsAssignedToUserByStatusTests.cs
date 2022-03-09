using Application.Groups.Queries.GetAllSolvingsAssignedToUserToDo;
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
    public class GetAllSolvingsAssignedToUserByStatusTests : SharedUtilityClass, IClassFixture<CustomWebApplicationFactory<Startup>>
    {

        public GetAllSolvingsAssignedToUserByStatusTests(CustomWebApplicationFactory<Startup> factory) : base(factory)
        {
        }

        [Theory]
        [InlineData(SolvingStatusEnum.ToDo, 2)]
        [InlineData(SolvingStatusEnum.Done, 1)]
        [InlineData(SolvingStatusEnum.Overdue, 2)]
        [InlineData(SolvingStatusEnum.DoneButOverdue, 3)]
        [InlineData(SolvingStatusEnum.Checked, 1)]
        public async Task ForSpecifiedStatus_ReturnsValidData(SolvingStatusEnum status, int expectedResult)
        {
            //Arrange
            await ClearNotNecesseryData();
            int userId = await RunAsDefaultUserAsync();
            var group = await AddAsync(new Group() { CreatorId = userId, Name = "Grupa1" });
            var groupRoles = await SeedPermissionWithGroupRoles();
            var assignment = await AddAsync(new Assignment(userId, group.Id, groupRoles["Owner"]));
            var exercise = await AddAsync(GetValidNoDataReturnsExercise(DatabasesIdsHelper.NorthwindSimpleDatabaseId, false, userId));
            var users = await SeedUsers();
            await AddRangeAsync(getSolvings(exercise.Id, assignment.Id, users["user1"]));

            //Act
            var result = await SendAsync
            (new GetAllSolvingsAssignedToUserByStatusQuery() { Status = status.ToString() });

            //Assert
            result.Should().HaveCount(expectedResult);
        }

        private List<Solving> getSolvings(int exerciseId, int assignmentId, int creatorId)
        {
            return new List<Solving>()
            {
                new Solving() { ExerciseId = exerciseId, AssignmentId = assignmentId, Status = SolvingStatusEnum.ToDo, CreatorId = creatorId },
                new Solving() { ExerciseId = exerciseId, AssignmentId = assignmentId, Status = SolvingStatusEnum.ToDo, CreatorId = creatorId  },
                new Solving() { ExerciseId = exerciseId, AssignmentId = assignmentId, Status = SolvingStatusEnum.Done, CreatorId = creatorId },
                new Solving() { ExerciseId = exerciseId, AssignmentId = assignmentId, Status = SolvingStatusEnum.DoneButOverdue, CreatorId = creatorId },
                new Solving() { ExerciseId = exerciseId, AssignmentId = assignmentId, Status = SolvingStatusEnum.DoneButOverdue, CreatorId = creatorId },
                new Solving() { ExerciseId = exerciseId, AssignmentId = assignmentId, Status = SolvingStatusEnum.DoneButOverdue, CreatorId = creatorId },
                new Solving() { ExerciseId = exerciseId, AssignmentId = assignmentId, Status = SolvingStatusEnum.Overdue, CreatorId = creatorId },
                new Solving() { ExerciseId = exerciseId, AssignmentId = assignmentId, Status = SolvingStatusEnum.Checked, CreatorId = creatorId },
                new Solving() { ExerciseId = exerciseId, AssignmentId = assignmentId, Status = SolvingStatusEnum.Overdue, CreatorId = creatorId },
            };
        }

    }
}

