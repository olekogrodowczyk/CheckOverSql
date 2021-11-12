﻿using Application.Dto.CreateGroupVm;
using Application.Dto.CreateSolutionDto;
using Application.IntegrationTests.FakeAuthentication;
using Application.Responses;
using Application.ViewModels;
using Domain.Entities;
using FluentAssertions;
using Infrastructure.Data;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using WebAPI.IntegrationTests.Helpers;
using Xunit;

namespace WebAPI.IntegrationTests.Controllers
{
    public class GroupControllerTests : SharedUtilityClass, IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        public GroupControllerTests(CustomWebApplicationFactory<Startup> factory) : base(factory) { }

        [Fact]
        public async Task Create_ForValidModel_ReturnsOk()
        {
            //Arrange
            var httpContent = new CreateGroupDto
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
            var httpContent = new CreateGroupDto
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
            await ClearTableInContext<Group>();

            //Act
            var response = await _client.GetAsync(ApiRoutes.Group.GetUserGroups);
            string responseString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<Result<IEnumerable<GetGroupVm>>>(responseString);

            //Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            result.Value.Should().HaveCount(0);
        }

        [Fact]
        public async Task GetUserGroups_ForCreatedGroupsByUser_ReturnsAllTheseGroups()
        {
            //Arrange
            await ClearTableInContext<Group>();
            var group1 = await addNewEntity<Group>(new Group { Name = "Grupa1", CreatorId = 99 });
            var group2 = await addNewEntity<Group>(new Group { Name = "Grupa1", CreatorId = 102 });
            var group3 = await addNewEntity<Group>(new Group { Name = "Grupa1", CreatorId = 99 });

            //Act
            var response = await _client.GetAsync(ApiRoutes.Group.GetUserGroups);
            string responseString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<Result<IEnumerable<GetGroupVm>>>(responseString);

            //Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            result.Value.Should().HaveCount(2);
        }

        [Fact]
        public async Task Create_ForCreatedGroup_CreatesAssignmentWithOwnerGroupRole()
        {
            //Arrange
            var httpContent = new CreateGroupDto
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
            await ClearNotNecesseryData();
            var group = await addNewEntity<Group>(new Group { Name = "Grupa1", CreatorId = 99 });
            var assignment1 = await addNewEntity<Assignment>
                (new Assignment { UserId = 99, GroupId = group.Id, GroupRoleId = 1 });
            var assignment2 = await addNewEntity<Assignment>
                (new Assignment { UserId = 100, GroupId = group.Id, GroupRoleId = 1 });

            //Act
            var response = await _client.DeleteAsync
                (ApiRoutes.Group.DeleteGroup.Replace("{groupId}", group.Id.ToString()));

            bool groupExists = await EntityExists<Group>(x => x.Id == group.Id);
            bool assignment1Exists = await EntityExists<Assignment>(x => x.Id == assignment1.Id);
            bool assignment2Exists = await EntityExists<Assignment>(x => x.Id == assignment2.Id);

            //Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            groupExists.Should().BeFalse();
            assignment1Exists.Should().BeFalse();
            assignment2Exists.Should().BeFalse();
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



    }
}
