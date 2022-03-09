using Application.Common.Exceptions;
using Application.Dto.AssignExerciseToUsersTo;
using Application.Responses;
using Domain.Entities;
using Domain.Enums;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.IntegrationTests.Helpers;
using Xunit;

namespace WebAPI.IntegrationTests.Exercises.Commands.AssignExerciseToUsers
{
    public class AssignExerciseToUsersTests : SharedUtilityClass, IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        public AssignExerciseToUsersTests(CustomWebApplicationFactory<Startup> factory) : base(factory)
        {
        }

        [Theory]
        [InlineData(1, 1000, null)]
        [InlineData(2, 110, null)]
        [InlineData(3, 150, null)]
        [InlineData(4, 120, typeof(ForbidException))]
        [InlineData(1, 100, null)]
        [InlineData(1, 30, typeof(ValidationException))]
        public async Task ForGivenParameteres_ReturnsValidData
            (int groupRoleId, int deadLineMinutesAhead, Type expectedExceptionType)
        {
            //Arrange 
            await ClearNotNecesseryData();
            var userId = await RunAsDefaultUserAsync();
            var users = await SeedUsers();
            var groupRoles = await SeedPermissionWithGroupRoles();

            var group1 = await AddAsync<Group>(new Group { Name = "Grupa1", CreatorId = userId });
            var group2 = await AddAsync<Group>(new Group { Name = "Grupa1", CreatorId = users["user1"] });
            List<Assignment> assignments = new List<Assignment>()
            {
                await AddAsync(new Assignment(userId,group1.Id, groupRoleId)),
                await AddAsync(new Assignment(users["user1"], group1.Id, groupRoles["User"])),
                await AddAsync(new Assignment(users["user2"], group1.Id , groupRoles["User"])),
                await AddAsync(new Assignment(users["user3"], group1.Id, groupRoles["Moderator"])),
                await AddAsync(new Assignment(users["user4"], group1.Id, groupRoles["User"])),
                await AddAsync(new Assignment(users["user1"], group2.Id, groupRoles["User"])),
            };
            var exercise = await AddAsync<Exercise>(GetValidFootballersExercise(false));
            var command = new AssignExerciseToUsersCommand
            {
                DeadLine = DateTime.UtcNow.AddMinutes(deadLineMinutesAhead),
                GroupId = group1.Id,
                ExerciseId = exercise.Id,
            };

            //Act
            Func<Task<IEnumerable<int>>> func = async () => await SendAsync(command);

            //Assert
            if (expectedExceptionType is null) { (await func()).Should().HaveCount(3); }
            if (expectedExceptionType == typeof(ValidationException)) { await func.Should().ThrowAsync<ValidationException>(); }
            if (expectedExceptionType == typeof(ForbidException)) { await func.Should().ThrowAsync<ForbidException>(); }
        }
    }
}
