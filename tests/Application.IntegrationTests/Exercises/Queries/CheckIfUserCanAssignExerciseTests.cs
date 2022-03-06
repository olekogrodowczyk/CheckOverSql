using Application.Exercises.Queries.CheckIfUserCanAssigneExercise;
using Domain.Entities;
using Domain.Enums;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace WebAPI.IntegrationTests.Exercises.Queries
{
    public class CheckIfUserCanAssignExerciseTests : SharedUtilityClass, IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        public CheckIfUserCanAssignExerciseTests(CustomWebApplicationFactory<Startup> factory) : base(factory)
        {
        }

        [Theory]
        [InlineData(GroupRoleEnum.User, false)]
        [InlineData(GroupRoleEnum.Checker, true)]
        [InlineData(GroupRoleEnum.Moderator, true)]
        [InlineData(GroupRoleEnum.Owner, true)]
        public async Task ForGivenData_GivesValidResults(GroupRoleEnum groupRoleEnum, bool expectedResult)
        {
            //Arrange
            await ClearNotNecesseryData();
            var users = await SeedUsers();
            int userId = await RunAsDefaultUserAsync();
            var groupRoles = await SeedPermissionWithGroupRoles();
            var group = await AddAsync(new Group() { Name = "group", CreatorId = users["user3"] });
            var assignment = await AddAsync(new Assignment(userId, group.Id, groupRoles[groupRoleEnum.ToString()]));

            //Act
            var result = await SendAsync(new CheckIfUserCanAssignExerciseQuery());

            //Assert
            result.Should().Be(expectedResult);

        }
    }
}
