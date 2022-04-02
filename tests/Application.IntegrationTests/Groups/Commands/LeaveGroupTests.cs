using Application.Common.Exceptions;
using Application.Groups.Commands.LeaveGroup;
using Domain.Entities;
using FluentAssertions;
using MediatR;
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
    public class LeaveGroupTests : SharedUtilityClass, IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        public LeaveGroupTests(CustomWebApplicationFactory<Startup> factory) : base(factory)
        {
        }

        [Fact]
        public async Task ForValidData_RemovesAssignment()
        {
            //Arrange 
            await ClearNotNecesseryData();
            var user = await RunAsDefaultUserAsync();
            var users = await SeedUsers();
            var groupRoles = await SeedPermissionWithGroupRoles();
            var group = await AddAsync(new Group { Name = "tmp", CreatorId = users["user1"] });
            var assignmentOwner = await AddAsync(new Assignment(users["user1"], group.Id, groupRoles["Owner"]));
            var assignmentCurrentUser = await AddAsync(new Assignment(user, group.Id, groupRoles["Checker"]));

            //Act
            var result = await SendAsync(new LeaveGroupCommand { GroupId = group.Id });
            var assignment = await FirstOrDefaultAsync<Assignment>(x => x.UserId == user && x.GroupId == group.Id);

            //Assert
            assignment.Should().BeNull();
        }

        [Fact]
        public async Task ForBeingCreator_ThrowsForbidException()
        {
            //Arrange 
            await ClearNotNecesseryData();
            var user = await RunAsDefaultUserAsync();
            var users = await SeedUsers();
            var groupRoles = await SeedPermissionWithGroupRoles();
            var group = await AddAsync(new Group { Name = "tmp", CreatorId = user });
            var assignmentOwner = AddAsync(new Assignment(user, group.Id, groupRoles["Owner"]));

            //Act
            Func<Task<Unit>> func = async () => await SendAsync(new LeaveGroupCommand { GroupId = group.Id });

            //Assert
            await func.Should().ThrowAsync<ForbidException>();
        }

        [Fact]
        public async Task ForNonExistingGroup_ThrowsNotFoundException()
        {
            //Arrange 
            await ClearNotNecesseryData();
            var user = await RunAsDefaultUserAsync();
            var users = await SeedUsers();
            var groupRoles = await SeedPermissionWithGroupRoles();
            var group = await AddAsync(new Group { Name = "tmp", CreatorId = user });
            var assignmentOwner = AddAsync(new Assignment(user, group.Id, groupRoles["Owner"]));

            //Act
            Func<Task<Unit>> func = async () => await SendAsync(new LeaveGroupCommand { GroupId = group.Id + 1 });

            //Assert
            await func.Should().ThrowAsync<NotFoundException>();
        }

        [Fact]
        public async Task ForUserNotBeingInGroup_ThrowsNotFoundException()
        {
            //Arrange 
            await ClearNotNecesseryData();
            var user = await RunAsDefaultUserAsync();
            var users = await SeedUsers();
            var groupRoles = await SeedPermissionWithGroupRoles();
            var group = await AddAsync(new Group { Name = "tmp", CreatorId = user });
            var group2 = await AddAsync(new Group { Name = "tmp2", CreatorId = users["user1"] });
            var assignmentOwner = AddAsync(new Assignment(user, group.Id, groupRoles["User"]));

            //Act
            Func<Task<Unit>> func = async () => await SendAsync(new LeaveGroupCommand { GroupId = group2.Id });

            //Assert
            await func.Should().ThrowAsync<NotFoundException>();
        }

        [Fact]
        public async Task ForEmptyGroupSpecified_ThrowsValidationException()
        {
            //Arrange 
            await ClearNotNecesseryData();
            var user = await RunAsDefaultUserAsync();
            var users = await SeedUsers();
            var groupRoles = await SeedPermissionWithGroupRoles();
            var group = await AddAsync(new Group { Name = "tmp", CreatorId = user });
            var group2 = await AddAsync(new Group { Name = "tmp2", CreatorId = users["user1"] });
            var assignmentOwner = AddAsync(new Assignment(user, group.Id, groupRoles["User"]));

            //Act
            Func<Task<Unit>> func = async () => await SendAsync(new LeaveGroupCommand());

            //Assert
            await func.Should().ThrowAsync<ValidationException>();
        }
    }
}
