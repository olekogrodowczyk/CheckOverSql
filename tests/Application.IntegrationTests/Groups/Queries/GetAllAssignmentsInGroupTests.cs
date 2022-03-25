using Application.Common.Exceptions;
using Application.Groups;
using Application.Groups.Queries.GetAllAssignmentsInGroup;
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

namespace Application.IntegrationTests.Groups.Queries
{
    public class GetAllAssignmentsInGroupTests : SharedUtilityClass, IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        public GetAllAssignmentsInGroupTests(CustomWebApplicationFactory<Startup> factory) : base(factory)
        {
        }

        [Fact]
        public async Task GetAllAssignmentsInGroup_ForCreatedAssignments_ReturnsAllTheseAssignments()
        {
            //Arrange   
            await ClearNotNecesseryData();
            var users = await SeedUsers();
            var userId = await RunAsDefaultUserAsync();
            var group1 = await AddAsync(new Group { Name = "Grupa1", CreatorId = users["user1"] });
            var group2 = await AddAsync(new Group { Name = "Grupa2", CreatorId = users["user1"] });

            await AddAsync(new Assignment { UserId = userId, GroupId = group1.Id, GroupRoleId = 4 });
            await AddAsync(new Assignment { UserId = users["user1"], GroupId = group1.Id, GroupRoleId = 1 });
            await AddAsync(new Assignment { UserId = users["user2"], GroupId = group1.Id, GroupRoleId = 3 });
            await AddAsync(new Assignment { UserId = users["user1"], GroupId = group2.Id, GroupRoleId = 1 });
            await AddAsync(new Assignment { UserId = users["user3"], GroupId = group2.Id, GroupRoleId = 4 });
            await AddAsync(new Assignment { UserId = users["user3"], GroupId = group1.Id, GroupRoleId = 4 });
            await AddAsync(new Assignment { UserId = users["user4"], GroupId = group1.Id, GroupRoleId = 4 });
            await AddAsync(new Assignment { UserId = userId, GroupId = group2.Id, GroupRoleId = 4 });

            var query = new GetAllAssignmentsInGroupQuery() { GroupId = group1.Id };

            //Act
            var result = await SendAsync(query);


            //Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(5);
        }

        [Fact]
        public async Task ForUserNotBeingInGroup_ReturnsForbidden()
        {
            //Arrange
            await ClearNotNecesseryData();
            var users = await SeedUsers();
            var userId = await RunAsDefaultUserAsync();
            var groupRoles = await SeedPermissionWithGroupRoles();
            var group1 = await AddAsync(new Group { Name = "Grupa1", CreatorId = users["user1"] });
            var group2 = await AddAsync(new Group { Name = "Grupa2", CreatorId = users["user2"] });

            await AddAsync(new Assignment { UserId = users["user1"], GroupId = group1.Id, GroupRoleId = groupRoles["User"] });
            await AddAsync(new Assignment { UserId = users["user2"], GroupId = group2.Id, GroupRoleId = groupRoles["User"] });
            await AddAsync(new Assignment { UserId = users["user3"], GroupId = group1.Id, GroupRoleId = groupRoles["Checker"] });
            await AddAsync(new Assignment { UserId = users["user3"], GroupId = group1.Id, GroupRoleId = groupRoles["Moderator"] });
            await AddAsync(new Assignment { UserId = users["user4"], GroupId = group1.Id, GroupRoleId = groupRoles["Owner"] });
            await AddAsync(new Assignment { UserId = userId, GroupId = group2.Id, GroupRoleId = groupRoles["Checker"] });

            var query = new GetAllAssignmentsInGroupQuery() { GroupId = group1.Id };

            //Act
            Func<Task<IEnumerable<GetAssignmentDto>>> func = async () => await SendAsync(query);

            //Assert
            await func.Should().ThrowAsync<ForbidException>();
        }
    }
}
