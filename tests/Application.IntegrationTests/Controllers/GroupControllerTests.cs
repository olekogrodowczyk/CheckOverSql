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
            var response = await _client.PostAsync("api/group/", httpContent);

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
            var response = await _client.PostAsync("api/group/", httpContent);

            //Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task GetUserGroups_ForNoCreatedGroups_ReturnsCountZero()
        {
            //Arrange
            await ClearTableInContext<Group>();

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
            await ClearTableInContext<Group>();
            var group1 = addNewEntity<Group>(new Group { Name = "Grupa1", CreatorId = 99 });
            var group2 = addNewEntity<Group>(new Group { Name = "Grupa1", CreatorId = 102 });
            var group3 = addNewEntity<Group>(new Group { Name = "Grupa1", CreatorId = 99 });

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
            var httpContent = new CreateGroupDto
            {
                Name = "Group1"
            }.ToJsonHttpContent();
          
            //Act
            var response = await _client.PostAsync("api/group/", httpContent);
            
            //Assert
            var scopeFactory = _factory.Services.GetService<IServiceScopeFactory>();
            using var scope = scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetService<ApplicationDbContext>();

            bool result = await context.Assignments.AnyAsync();

            result.Should().BeTrue();
        }
      
        

    }
}
