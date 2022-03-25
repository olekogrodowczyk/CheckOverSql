using Application.Common.Exceptions;
using Application.Groups.Commands.DeleteGroup;
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

namespace Application.IntegrationTests.Groups.Commands
{
    public class DeleteGroupTests : SharedUtilityClass, IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        public DeleteGroupTests(CustomWebApplicationFactory<Startup> factory) : base(factory)
        {
        }

        [Fact]
        public async Task ForValidData_DeletesGroupWithAssignments()
        {
            //Arrange
            await ClearNotNecesseryData();
            var userId = await RunAsDefaultUserAsync();
            var users = await SeedUsers();
            var groupRoles = await SeedPermissionWithGroupRoles();
            var group = await AddAsync(new Group { Name = "Grupa1", CreatorId = userId });
            var assignment1 = await AddAsync(new Assignment
            { UserId = userId, GroupId = group.Id, GroupRoleId = groupRoles["Owner"] });
            var assignment2 = await AddAsync(new Assignment
            { UserId = users["user2"], GroupId = group.Id, GroupRoleId = groupRoles["Checker"] });

            var command = new DeleteGroupCommand { GroupId = group.Id };

            //Act
            await SendAsync(command);
            var groupCheck = await FindAsync<Group>(group.Id);
            var assignment1Check = await FindAsync<Assignment>(assignment1.Id);
            var assignment2Check = await FindAsync<Assignment>(assignment2.Id);

            //Assert
            groupCheck.Should().BeNull();
            assignment1Check.Should().BeNull();
            assignment2Check.Should().BeNull();
        }

        [Fact]
        public async Task DeleteGroup_ForInvalidGroupRole_ReturnsForbidden()
        {
            //Arrange
            await ClearNotNecesseryData();
            var userId = await RunAsDefaultUserAsync();
            var groupRoles = await SeedPermissionWithGroupRoles();
            var group = await AddAsync(new Group { Name = "Grupa1", CreatorId = userId });
            var assignment1 = await AddAsync
                (new Assignment { UserId = userId, GroupId = group.Id, GroupRoleId = groupRoles["Checker"] });
            var command = new DeleteGroupCommand { GroupId = group.Id };

            //Act
            Func<Task> func = async () => await SendAsync(command);

            //Assert
            await func.Should().ThrowAsync<ForbidException>();
        }
    }
}
