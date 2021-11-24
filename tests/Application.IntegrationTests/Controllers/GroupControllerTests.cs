using Application.Dto.CreateGroupVm;
using Application.Groups.Commands.CreateGroup;
using Application.Responses;
using Application.Groups;
using Domain.Entities;
using FluentAssertions;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.IntegrationTests.Helpers;
using Xunit;
using Application.Groups.Queries;

namespace WebAPI.IntegrationTests.Controllers
{
    public class GroupControllerTests : SharedUtilityClass, IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        public GroupControllerTests(CustomWebApplicationFactory<Startup> factory) : base(factory) { }

        [Fact]
        public async Task Create_ForValidModel_ReturnsOk()
        {
            //Arrange
            var httpContent = new CreateGroupCommand
            {
                Name = "Group 1 - C# programming"
            }.ToJsonHttpContent();

            //Act
            var response = await _client.PostAsync(ApiRoutes.Group.Create, httpContent);

            //Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }

        [Fact]
        public async Task Create_ForInvalidModel_ReturnsBadRequest()
        {
            //Arrange
            var httpContent = new CreateGroupCommand
            {
                Name = "Gr"
            }.ToJsonHttpContent();

            //Act
            var response = await _client.PostAsync(ApiRoutes.Group.Create, httpContent);

            //Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task GetUserGroups_ForNoCreatedGroups_ReturnsCountZero()
        {
            //Arrange
            await ClearNotNecesseryData();

            //Act
            var response = await _client.GetAsync(ApiRoutes.Group.GetUserGroups);
            var result = await response.ToResultAsync<Result<IEnumerable<GetGroupDto>>>();

            //Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            result.Value.Should().HaveCount(0);
        }

        [Fact]
        public async Task GetUserGroups_ForCreatedGroupsByUser_ReturnsAllTheseGroups()
        {
            //Arrange
            await ClearNotNecesseryData();
            var group1 = await addNewEntity<Group>(new Group { Name = "Grupa1", CreatorId = 99 });
            var group2 = await addNewEntity<Group>(new Group { Name = "Grupa1", CreatorId = 102 });
            var group3 = await addNewEntity<Group>(new Group { Name = "Grupa1", CreatorId = 99 });
            var assignment1 = await addNewEntity<Assignment>(new Assignment { GroupId = group1.Id, GroupRoleId = 1, UserId = 99 });
            var assignment2 = await addNewEntity<Assignment>(new Assignment { GroupId = group2.Id, GroupRoleId = 1, UserId = 102 });
            var assignment3 = await addNewEntity<Assignment>(new Assignment { GroupId = group3.Id, GroupRoleId = 1, UserId = 99 });

            //Act
            var response = await _client.GetAsync(ApiRoutes.Group.GetUserGroups);
            var result = await response.ToResultAsync<Result<IEnumerable<GetGroupDto>>>();

            //Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            result.Value.Should().HaveCount(2);
        }

        [Fact]
        public async Task Create_ForCreatedGroup_CreatesAssignmentWithOwnerGroupRole()
        {
            //Arrange
            var httpContent = new CreateGroupCommand
            {
                Name = "Group1"
            }.ToJsonHttpContent();

            //Act
            var response = await _client.PostAsync(ApiRoutes.Group.Create, httpContent);

            var scopeFactory = _factory.Services.GetService<IServiceScopeFactory>();
            using var scope = scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetService<ApplicationDbContext>();

            bool result = await context.Assignments.AnyAsync();
            bool isOwnerGroupRole = await context.Assignments
                .Include(x => x.GroupRole)
                .AnyAsync(x => x.GroupRole.Name == "Owner");

            //Assert
            isOwnerGroupRole.Should().BeTrue();
            result.Should().BeTrue();
        }

        [Fact]
        public async Task DeleteGroup_ForValidData_DeletesGroupWithAssignments()
        {
            //Arrange
            var scopeFactory = _factory.Services.GetService<IServiceScopeFactory>();
            using var scope = scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetService<ApplicationDbContext>();

            await ClearNotNecesseryData();
            var group = await addNewEntity<Group>(new Group { Name = "Grupa1", CreatorId = 99 });
            var assignment1 = await addNewEntity<Assignment>
                (new Assignment { UserId = 99, GroupId = group.Id, GroupRoleId = 1 });
            var assignment2 = await addNewEntity<Assignment>
                (new Assignment { UserId = 100, GroupId = group.Id, GroupRoleId = 2 });

            await context.Groups
                .Include(x => x.Assignments)
                .FirstOrDefaultAsync(x => x.Id == group.Id);

            //Act
            var response = await _client.DeleteAsync
                (ApiRoutes.Group.DeleteGroup.Replace("{groupId}", group.Id.ToString()));

            bool groupExists = await EntityExists<Group>(x => x.Id == group.Id);



            //Assert


            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            groupExists.Should().BeFalse();
            context.Assignments.Count().Should().Be(0);
        }

        [Fact]
        public async Task DeleteGroup_ForInvalidGroupRole_ReturnsForbidden()
        {
            //Arrange
            await ClearNotNecesseryData();
            var group = await addNewEntity<Group>(new Group { Name = "Grupa1", CreatorId = 99 });
            var assignment1 = await addNewEntity<Assignment>
                (new Assignment { UserId = 99, GroupId = group.Id, GroupRoleId = 2 });

            //Act
            var response = await _client.DeleteAsync
                (ApiRoutes.Group.DeleteGroup.Replace("{groupId}", group.Id.ToString()));

            //Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task GetAllAssignmentsInGroup_ForCreatedAssignments_ReturnsAllTheseAssignments()
        {
            //Arrange   
            await ClearNotNecesseryData();

            await SeedUsers();
            var group1 = await addNewEntity<Group>(new Group { Name = "Grupa1", CreatorId = 100 });
            var group2 = await addNewEntity<Group>(new Group { Name = "Grupa2", CreatorId = 100 });

            await addNewEntity<Assignment>(new Assignment { UserId = 99, GroupId = group1.Id, GroupRoleId = 4 });
            await addNewEntity<Assignment>(new Assignment { UserId = 100, GroupId = group1.Id, GroupRoleId = 1 });
            await addNewEntity<Assignment>(new Assignment { UserId = 101, GroupId = group1.Id, GroupRoleId = 3 });
            await addNewEntity<Assignment>(new Assignment { UserId = 100, GroupId = group2.Id, GroupRoleId = 1 });
            await addNewEntity<Assignment>(new Assignment { UserId = 102, GroupId = group2.Id, GroupRoleId = 4 });
            await addNewEntity<Assignment>(new Assignment { UserId = 102, GroupId = group1.Id, GroupRoleId = 4 });
            await addNewEntity<Assignment>(new Assignment { UserId = 103, GroupId = group1.Id, GroupRoleId = 4 });
            await addNewEntity<Assignment>(new Assignment { UserId = 99, GroupId = group2.Id, GroupRoleId = 4 });

            //Act
            var response = await _client.GetAsync
                (ApiRoutes.Group.GetAllAssignmentsInGroup.Replace("{groupId}", group1.Id.ToString()));
            var result = await response.ToResultAsync<Result<IEnumerable<GetAssignmentDto>>>();

            //Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            result.Value.Should().NotBeNull();
            result.Value.Should().HaveCount(5);
        }

        [Fact]
        public async Task GetAllAssignmentsInGroup_ForUserNotBeingInGroup_ReturnsForbidden()
        {
            //Arrange
            await ClearNotNecesseryData();

            await SeedUsers();
            var group1 = await addNewEntity<Group>(new Group { Name = "Grupa1", CreatorId = 100 });
            var group2 = await addNewEntity<Group>(new Group { Name = "Grupa2", CreatorId = 101 });

            await addNewEntity<Assignment>(new Assignment { UserId = 100, GroupId = group1.Id, GroupRoleId = 1 });
            await addNewEntity<Assignment>(new Assignment { UserId = 101, GroupId = group2.Id, GroupRoleId = 1 });
            await addNewEntity<Assignment>(new Assignment { UserId = 101, GroupId = group1.Id, GroupRoleId = 2 });
            await addNewEntity<Assignment>(new Assignment { UserId = 102, GroupId = group1.Id, GroupRoleId = 3 });
            await addNewEntity<Assignment>(new Assignment { UserId = 103, GroupId = group1.Id, GroupRoleId = 4 });
            await addNewEntity<Assignment>(new Assignment { UserId = 99, GroupId = group2.Id, GroupRoleId = 2 });

            //Act
            var response = await _client.GetAsync
                (ApiRoutes.Group.GetAllAssignmentsInGroup.Replace("{groupId}", group1.Id.ToString()));

            //Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.Forbidden);
        }


    }
}
