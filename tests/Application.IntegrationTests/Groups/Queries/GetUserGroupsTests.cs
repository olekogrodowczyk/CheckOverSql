using Application.Groups.Queries.GetUserGroups;
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
    public class GetUserGroupsTests : SharedUtilityClass, IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        public GetUserGroupsTests(CustomWebApplicationFactory<Startup> factory) : base(factory)
        {
        }

        [Fact]
        public async Task ForCreatedGroups_ReturnsAllUserGroups()
        {
            //Arrange
            await ClearNotNecesseryData();
            var users = await SeedUsers();
            var userId = await RunAsDefaultUserAsync();
            var group1 = await AddAsync(new Group { Name = "Grupa1", CreatorId = userId });
            var group2 = await AddAsync(new Group { Name = "Grupa1", CreatorId = users["user3"] });
            var group3 = await AddAsync(new Group { Name = "Grupa1", CreatorId = userId });
            var assignment1 = await AddAsync(new Assignment { GroupId = group1.Id, GroupRoleId = 1, UserId = userId });
            var assignment2 = await AddAsync(new Assignment { GroupId = group2.Id, GroupRoleId = 1, UserId = users["user3"] });
            var assignment3 = await AddAsync(new Assignment { GroupId = group3.Id, GroupRoleId = 1, UserId = userId });
            var query = new GetUserGroupsQuery();

            //Act
            var result = await SendAsync(query);

            //Assert
            result.Should().NotBeNull();
            result.Count().Should().Be(2);
        }

        [Fact]
        public async Task ForNoCreatedGroups_ReturnsEmptyResult()
        {
            //Arrange
            await ClearNotNecesseryData();
            var userId = await RunAsDefaultUserAsync();
            var query = new GetUserGroupsQuery();

            //Act
            var result = await SendAsync(query);

            //Assert
            result.Should().BeEmpty();
        }
    }
}
