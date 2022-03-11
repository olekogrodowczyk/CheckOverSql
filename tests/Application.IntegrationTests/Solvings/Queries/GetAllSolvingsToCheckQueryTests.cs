using Application.IntegrationTests.Helpers;
using Application.Solvings.Queries.GetAllSolvingsToCheck;
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
    public class GetAllSolvingsToCheckQueryTests : SharedUtilityClass, IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        public GetAllSolvingsToCheckQueryTests(CustomWebApplicationFactory<Startup> factory) : base(factory)
        {
        }

        [Theory]
        [InlineData(null, 3)]
        [InlineData(0, 2)]
        [InlineData(1, 1)]
        [InlineData(2, 0)]
        public async Task ForGivenData_ReturnsValidNumberOfSolvingsToCheck(int? groupNumber, int expectedResult)
        {
            //Arrange
            await ClearNotNecesseryData();
            var userId = await RunAsDefaultUserAsync();
            var users = await SeedUsers();
            var groupRoles = await SeedPermissionWithGroupRoles();
            var groups = (await AddRangeAsync(new List<Group>
            {
                new Group() { Name = "group1", CreatorId = userId },
                new Group() { Name = "group2", CreatorId = users["user3"] },
                new Group() { Name = "group3", CreatorId = users["user4"] },
            })).ToList();
            var exercise = await AddAsync(GetValidNoDataReturnsExercise(DatabasesIdsHelper.NorthwindSimpleDatabaseId));
            var assignment1 = await AddAsync(new Assignment(userId, groups[0].Id, groupRoles["Owner"]));
            var assignment2 = await AddAsync(new Assignment(userId, groups[1].Id, groupRoles["Checker"]));
            var assignment3 = await AddAsync(new Assignment(userId, groups[2].Id, groupRoles["User"]));
            var solvings = await AddRangeAsync(new List<Solving>
            {
                new Solving()
                {CreatorId = users["user4"], ExerciseId = exercise.Id, AssignmentId = assignment1.Id, Status = SolvingStatusEnum.Done},
                new Solving()
                {CreatorId = users["user4"], ExerciseId = exercise.Id, AssignmentId = assignment2.Id, Status = SolvingStatusEnum.DoneButOverdue},
                new Solving()
                {CreatorId = users["user4"], ExerciseId = exercise.Id, AssignmentId = assignment3.Id, Status = SolvingStatusEnum.Done},
                new Solving()
                {CreatorId = users["user4"], ExerciseId = exercise.Id, AssignmentId = assignment2.Id, Status = SolvingStatusEnum.ToDo},
                new Solving()
                {CreatorId = users["user4"], ExerciseId = exercise.Id, AssignmentId = assignment3.Id, Status = SolvingStatusEnum.DoneButOverdue},
                new Solving()
                {CreatorId = users["user4"], ExerciseId = exercise.Id, AssignmentId = assignment1.Id, Status = SolvingStatusEnum.Overdue},
                new Solving()
                {CreatorId = users["user4"], ExerciseId = exercise.Id, AssignmentId = assignment1.Id, Status = SolvingStatusEnum.Done},
            });

            //Act
            var result = await SendAsync(new GetAllSolvingsToCheckQuery()
            { GroupId = groupNumber == null ? null : groups[(int)groupNumber].Id });

            //Assert
            result.Should().HaveCount(expectedResult);
        }


    }
}
