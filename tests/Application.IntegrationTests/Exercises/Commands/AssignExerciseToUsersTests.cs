using Application.Common.Exceptions;
using Application.Dto.AssignExerciseToUsersTo;
using Application.Responses;
using Domain.Entities;
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
            await SeedUsers();
            await SeedPermissionWithGroupRoles();

            var group1 = await AddAsync<Group>(new Group { Name = "Grupa1", CreatorId = 99 });
            var group2 = await AddAsync<Group>(new Group { Name = "Grupa1", CreatorId = 100 });
            List<Assignment> assignments = new List<Assignment>()
            {
                await AddAsync(new Assignment(userId,group1.Id, groupRoleId)),
                await AddAsync(new Assignment(100, group1.Id, 4)),
                await AddAsync(new Assignment(101,group1.Id ,4)),
                await AddAsync(new Assignment(102, group1.Id, 2)),
                await AddAsync(new Assignment(103,group1.Id,4)),
                await AddAsync(new Assignment(100,group2.Id,4))
            };
            var exercise = await AddAsync<Exercise>(GetValidExercise(false));
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
