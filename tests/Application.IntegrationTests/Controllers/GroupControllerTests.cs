using Application.Dto.CreateGroupVm;
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
    public class GroupControllerTests : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly HttpClient _client;
        private readonly CustomWebApplicationFactory<Startup> _factory;

        public GroupControllerTests(CustomWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Create_ForValidModel_ReturnsOk()
        {
            //Arrange
            var createGroupDto = new CreateGroupDto
            {
                Name = "Group 1 - C# programming"
            };
            var model = createGroupDto.ToJsonHttpContent();

            //Act
            var response = await _client.PostAsync("api/group/", model);

            //Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }

        [Fact]
        public async Task Create_ForInvalidModel_ReturnsBadRequest()
        {
            //Arrange
            var createGroupDto = new CreateGroupDto
            {
                Name = "Gr"
            };
            var model = createGroupDto.ToJsonHttpContent();

            //Act
            var response = await _client.PostAsync("api/group/", model);

            //Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task GetUserGroups_ForNoCreatedGroups_ReturnsCountZero()
        {
            //Arrange
            await ClearTableDataHelper.cleanTable<Group>(_factory);

            //Act
            var response = await _client.GetAsync("api/group/getusergroups");
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
            var group1 = getGroup(1);
            var group2 = getGroup(2);
            var group3 = getGroup(1);

            var scopeFactory = _factory.Services.GetService<IServiceScopeFactory>();
            using var scope = scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetService<ApplicationDbContext>();
            context.Groups.Clear();

            await context.AddAsync(group1);
            await context.AddAsync(group2);
            await context.AddAsync(group3);
            await context.SaveChangesAsync();

            //Act
            var response = await _client.GetAsync("api/group/getusergroups");
            string responseString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<Result<IEnumerable<GetGroupVm>>>(responseString);

            //Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            result.Value.Should().HaveCount(2);
        }

        [Fact]
        public async Task Create_ForCreatedGroup_CreatesRelationshipBetweenGroupAndUser()
        {
            //Arrange
            var createGroupDto = new CreateGroupDto
            {
                Name = "Group1"
            };

            var scopeFactory = _factory.Services.GetService<IServiceScopeFactory>();
            using var scope = scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetService<ApplicationDbContext>();

            var httpContent = createGroupDto.ToJsonHttpContent();

            //Act
            var response = await _client.PostAsync("api/group/", httpContent);
            bool result = await context.Assignments.AnyAsync();

            //Assert
            result.Should().BeTrue();

        }
      
        private Group getGroup(int creatorId)
        {
            return new Group
            {
                CreatorId = creatorId,
                Name = "Grupa1",
            };
        }

    }
}
