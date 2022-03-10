using Application.Common.Exceptions;
using Application.GroupRoles.Queries.CheckPermission;
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

namespace Application.IntegrationTests.GroupRoles.Queries
{
    public class CheckPermissionTests : SharedUtilityClass, IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        public CheckPermissionTests(CustomWebApplicationFactory<Startup> factory) : base(factory)
        {
        }

        [Theory]
        [InlineData(PermissionEnum.DeletingGroup, "Checker", false)]
        [InlineData(PermissionEnum.CheckingExercises, "Checker", true)]
        [InlineData(PermissionEnum.AssigningExercises, "User", false)]
        [InlineData(PermissionEnum.DoingExercises, "Checker", false)]
        [InlineData(PermissionEnum.GettingAssignments, "User", true)]
        public async Task ForGivenPermissionAndGroupRoles_ReturnsValidResult(PermissionEnum permissionEnum, string groupRole, bool expectedResult)
        {
            //Arrange
            await ClearNotNecesseryData();
            int userId = await RunAsDefaultUserAsync();
            var groupRoles = await SeedPermissionWithGroupRoles();
            var users = await SeedUsers();
            var group = await AddAsync(new Group() { Name = "Grupa1", CreatorId = users["user1"] });
            var assignment = await AddAsync(new Assignment(userId, group.Id, groupRoles[groupRole]));

            //Act
            var result = await SendAsync(new CheckPermissionQuery() { Permission = permissionEnum.ToString() });

            //Assert
            result.Should().Be(expectedResult);
        }

        [Fact]
        public async Task ForNonExistingGroup_ThrowsValidationException()
        {
            //Arrange
            await ClearNotNecesseryData();
            int userId = await RunAsDefaultUserAsync();
            var groupRoles = await SeedPermissionWithGroupRoles();
            var users = await SeedUsers();

            //Act
            Func<Task<bool>> func = async () => await SendAsync(new CheckPermissionQuery()
            { Permission = PermissionEnum.DeletingGroup.ToString(), GroupId = 99 });

            //Assert
            await func.Should().ThrowAsync<ValidationException>();
        }

        [Fact]
        public async Task ForNonExistingPermission_ThrowsValidationException()
        {
            //Arrange
            await ClearNotNecesseryData();
            int userId = await RunAsDefaultUserAsync();
            var groupRoles = await SeedPermissionWithGroupRoles();
            var users = await SeedUsers();
            var group = await AddAsync(new Group() { Name = "Grupa1", CreatorId = users["user1"] });
            var assignment = await AddAsync(new Assignment(userId, group.Id, groupRoles["Owner"]));

            //Act
            Func<Task<bool>> func = async () => await SendAsync(new CheckPermissionQuery()
            { Permission = "dnjwqoindqwoi", GroupId = 99 });

            //Assert
            await func.Should().ThrowAsync<ValidationException>();
        }
    }
}
