using Application.Common.Exceptions;
using Application.GroupRoles.Commands;
using Domain.Entities;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPI;
using WebAPI.IntegrationTests;
using Xunit;

namespace Application.IntegrationTests.GroupRoles.Commands
{
    public class ChangeRoleTests : SharedUtilityClass, IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        public ChangeRoleTests(CustomWebApplicationFactory<Startup> factory) : base(factory)
        {
        }

        [Theory]
        [InlineData("User")]
        [InlineData("Checker")]
        [InlineData("Moderator")]
        [InlineData("Owner")]
        public async Task ForGivenUserRoleAndValidData_ChangesTheRole(string desiredGroupRoleName)
        {
            //Arrange
            await ClearNotNecesseryData();
            var user = await RunAsDefaultUserAsync();
            var users = await SeedUsers();
            var groupRoles = await SeedPermissionWithGroupRoles();
            var group = await AddAsync(new Group { CreatorId = user, Name = "tmp" });
            var ownerAssignment = await AddAsync(new Assignment(user, group.Id, groupRoles["Owner"]));
            var roleToChangeAssignment = await AddAsync(new Assignment(users["user1"], group.Id, groupRoles["User"]));

            //Act
            var result = await SendAsync
                (new ChangeRoleCommand { RoleName = desiredGroupRoleName, UserId = users["user1"], GroupId = group.Id });
            var assignment = await FirstOrDefaultAsync<Assignment>(x => x.UserId == users["user1"] && x.GroupId == group.Id);
            var desiredGroupRole = await FirstOrDefaultAsync<GroupRole>(x => x.Name == desiredGroupRoleName);

            //Assert
            result.Should().BeGreaterThan(0);
            assignment.GroupRoleId.Should().Be(desiredGroupRole.Id);
        }

        [Theory]
        [InlineData("User")]
        [InlineData("Checker")]
        [InlineData("Moderator")]
        public async Task ForGroupRoleInAssignmentOtherThanOwner_ThrowsForbidException(string assignmentGroupRole)
        {
            //Arrange
            await ClearNotNecesseryData();
            var user = await RunAsDefaultUserAsync();
            var users = await SeedUsers();
            var groupRoles = await SeedPermissionWithGroupRoles();
            var group = await AddAsync(new Group { CreatorId = user, Name = "tmp" });
            var ownerAssignment = await AddAsync(new Assignment(user, group.Id, groupRoles[assignmentGroupRole]));
            var roleToChangeAssignment = await AddAsync(new Assignment(users["user1"], group.Id, groupRoles["User"]));
            var command = new ChangeRoleCommand { RoleName = "Checker", UserId = users["user1"], GroupId = group.Id };

            //Act
            Func<Task<int>> func = async () => await SendAsync(command);


            //Assert
            await func.Should().ThrowAsync<ForbidException>();
        }

        [Fact]
        public async Task ForTryingToChangeCreatorGroupRole_ThrowsForbidException()
        {
            //Arrange
            await ClearNotNecesseryData();
            var user = await RunAsDefaultUserAsync();
            var users = await SeedUsers();
            var groupRoles = await SeedPermissionWithGroupRoles();
            var group = await AddAsync(new Group { CreatorId = user, Name = "tmp" });
            var ownerAssignment = await AddAsync(new Assignment(user, group.Id, groupRoles["Owner"]));
            var command = new ChangeRoleCommand { RoleName = "Checker", UserId = user, GroupId = group.Id };

            //
            //Act
            Func<Task<int>> func = async () => await SendAsync(command);


            //Assert
            await func.Should().ThrowAsync<ValidationException>();
        }

        [Fact]
        public async Task ForUserNotBeingInGroup_ThrowsNotFoundException()
        {
            //Arrange
            await ClearNotNecesseryData();
            var user = await RunAsDefaultUserAsync();
            var users = await SeedUsers();
            var groupRoles = await SeedPermissionWithGroupRoles();
            var group = await AddAsync(new Group { CreatorId = user, Name = "tmp" });
            var ownerAssignment = await AddAsync(new Assignment(user, group.Id, groupRoles["Owner"]));
            var command = new ChangeRoleCommand { RoleName = "Checker", UserId = user + 1, GroupId = group.Id };

            //Act
            Func<Task<int>> func = async () => await SendAsync(command);

            //Assert
            await func.Should().ThrowAsync<NotFoundException>();
        }

        [Fact]
        public async Task ForNonExistingGroup_ThrowsNotFoundException()
        {
            //Arrange
            await ClearNotNecesseryData();
            var user = await RunAsDefaultUserAsync();
            var users = await SeedUsers();
            var groupRoles = await SeedPermissionWithGroupRoles();
            var group = await AddAsync(new Group { CreatorId = user, Name = "tmp" });
            var ownerAssignment = await AddAsync(new Assignment(user, group.Id, groupRoles["Owner"]));
            var roleToChangeAssignment = await AddAsync(new Assignment(users["user1"], group.Id, groupRoles["User"]));
            var command = new ChangeRoleCommand { RoleName = "Checker", UserId = users["user1"], GroupId = group.Id + 1 };

            //Act
            Func<Task<int>> func = async () => await SendAsync(command);

            //Assert
            await func.Should().ThrowAsync<NotFoundException>();
        }

        [Fact]
        public async Task ForNonExistingGroupRoleName_ThrowsValidationException()
        {
            //Arrange
            await ClearNotNecesseryData();
            var user = await RunAsDefaultUserAsync();
            var users = await SeedUsers();
            var groupRoles = await SeedPermissionWithGroupRoles();
            var group = await AddAsync(new Group { CreatorId = user, Name = "tmp" });
            var ownerAssignment = await AddAsync(new Assignment(user, group.Id, groupRoles["Owner"]));
            var command = new ChangeRoleCommand { RoleName = "Checker", UserId = user, GroupId = group.Id + 1 };

            //Act
            Func<Task<int>> func = async () => await SendAsync(command);

            //Assert
            await func.Should().ThrowAsync<ValidationException>();
        }


    }
}
