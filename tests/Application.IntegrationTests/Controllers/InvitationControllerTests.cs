using Application.Dto.CreateInvitationDto;
using Application.Dto.RegisterUserVm;
using Application.Responses;
using Application.ViewModels;
using Domain.Entities;
using FluentAssertions;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using WebAPI.IntegrationTests.Helpers;
using Xunit;

namespace WebAPI.IntegrationTests.Controllers
{
    public class InvitationControllerTests : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly HttpClient _client;
        private readonly CustomWebApplicationFactory<Startup> _factory;

        public InvitationControllerTests(CustomWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
            _client = factory.CreateClient();
        }
     
        [Fact]
        public async Task Create_ForValidModel_ReturnsOkWithValidProperties()
        {
            //Arrange
            await ClearContext();
            var scopeFactory = _factory.Services.GetService<IServiceScopeFactory>();
            using var scope = scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetService<ApplicationDbContext>();
            await SeedDataHelper.SeedUsers(context);

            var group = getValidGroup(99);
            
            await context.Groups.AddAsync(group);
            await context.SaveChangesAsync();
            await context.Assignments.AddAsync(new Assignment { GroupId = group.Id, UserId = 99, GroupRoleId = 2 });
            await context.SaveChangesAsync();

            CreateInvitationDto createInvitationDto = new CreateInvitationDto
            {
                ReceiverEmail = "johnsmith@gmail.com",
                RoleName = "Moderator"
            };
            var httpContent = createInvitationDto.ToJsonHttpContent();

            //Act
            var response = await _client.PostAsync($"api/group/{group.Id}/invitation", httpContent);
            var responseString = await response.Content.ReadAsStringAsync();

            //Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            var invitation = await context.Invitations.Include(x => x.GroupRole).Include(x => x.Receiver).Include(x => x.Group)
                .SingleOrDefaultAsync();
            invitation.Receiver.Email.Should().Be("johnsmith@gmail.com");
            invitation.Group.Should().NotBeNull();
            invitation.GroupRole.Name.Should().Be("Moderator");
        }

        [Fact]
        public async Task Create_ForSenderIsNotInTheGroup_ReturnsBadRequest()
        {
            //Arrange
            await ClearContext();
            var scopeFactory = _factory.Services.GetService<IServiceScopeFactory>();
            using var scope = scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetService<ApplicationDbContext>();
            await SeedDataHelper.SeedUsers(context);

            var group = getValidGroup(99);

            await context.Groups.AddAsync(group);
            await context.SaveChangesAsync();
            await context.Assignments.AddAsync(new Assignment { GroupId = group.Id, UserId = 102, GroupRoleId = 2 });
            await context.SaveChangesAsync();

            CreateInvitationDto createInvitationDto = new CreateInvitationDto
            {
                ReceiverEmail = "johnsmith@gmail.com",
                RoleName = "Moderator"
            };
            var httpContent = createInvitationDto.ToJsonHttpContent();

            //Act
            var response = await _client.PostAsync($"api/group/{group.Id}/invitation", httpContent);

            //Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }

        [Theory]
        [MemberData(nameof(GetSampleInvalidCreateModels))]
        public async Task Create_ForInvalidModels_ReturnsBadRequest(CreateInvitationDto createInvitationDto)
        {
            //Arrange
            await ClearContext();
            var scopeFactory = _factory.Services.GetService<IServiceScopeFactory>();
            using var scope = scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetService<ApplicationDbContext>();

            var group = getValidGroup(99);
            await context.AddAsync(group);
            await context.SaveChangesAsync();

            var httpContent = createInvitationDto.ToJsonHttpContent();

            //Act
            var response = await _client.PostAsync($"api/group/{group.Id}/invitation", httpContent);

            //Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Create_ForTwoSameInvitations_ReturnsBadRequest()
        {
            //Arrange
            await ClearContext();
            var scopeFactory = _factory.Services.GetService<IServiceScopeFactory>();
            using var scope = scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetService<ApplicationDbContext>();

            var group = getValidGroup(99);
            await context.AddAsync(group);
            await context.SaveChangesAsync();

            CreateInvitationDto createInvitationDto = new CreateInvitationDto
            {
                ReceiverEmail = "johnsmith@gmail.com",
                RoleName = "Moderator"
            };

            var httpContent = createInvitationDto.ToJsonHttpContent();

            //Act
            var response = await _client.PostAsync($"api/group/{group.Id}/invitation", httpContent);
            var response2 = await _client.PostAsync($"api/group/{group.Id}/invitation", httpContent);

            //Assert
            response2.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Create_ForUserAlreadyInGroup_ReturnsBadRequest()
        {
            //Arrange
            await ClearContext();
            var scopeFactory = _factory.Services.GetService<IServiceScopeFactory>();
            using var scope = scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetService<ApplicationDbContext>();

            var group = getValidGroup(99);
            await context.AddAsync(group);
            await context.SaveChangesAsync();

            var assignment = new Assignment
            {
                GroupId = group.Id,
                UserId = 100,
                GroupRoleId = 2,                    
            };
            await context.Assignments.AddAsync(assignment);
            await context.SaveChangesAsync();

            CreateInvitationDto createInvitationDto = new CreateInvitationDto
            {
                ReceiverEmail = "testinvitation@gmail.com",
                RoleName = "Moderator"
            };
            var httpContent = createInvitationDto.ToJsonHttpContent();

            //Act
            var response = await _client.PostAsync($"api/group/{group.Id}/invitation", httpContent);
            var responseString = await response.Content.ReadAsStringAsync();

            //Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }

        [Theory]
        [InlineData(99, 100, 2, "getallsent")]
        [InlineData(100, 99, 1, "getallsent")]
        [InlineData(99, 101, 2, "getallsent")]
        [InlineData(99,100, 1, "getallreceived")]
        [InlineData(100,99, 2, "getallreceived")]
        [InlineData(100,99, 3, "getall")]
        public async Task GetAllWithCondition_ForValidModel_ReturnsOkWithValidCount
            (int senderId, int receiverId, int expectedCount, string queryType)
        {
            //Arrange
            await ClearContext();            
            var scopeFactory = _factory.Services.GetService<IServiceScopeFactory>();
            using var scope = scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetService<ApplicationDbContext>();
            
            await SeedDataHelper.SeedUsers(context);
            var group = getValidGroup(99);
            await context.AddAsync(group);
            await context.SaveChangesAsync();

            var invitation1 =
                await context.AddAsync(getInvitation(senderId, receiverId, group.Id, 2, "Sent"));
            var invitation2 =
                await context.AddAsync(getInvitation(101, 99, group.Id, 2, "Accepted"));
            var invitation3 =
                await context.AddAsync(getInvitation(99, 102, group.Id, 2, "Sent"));

            await context.SaveChangesAsync();

            //Act
            var response = await _client.GetAsync($"api/group/{group.Id}/invitation/{queryType}");
            var responseString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<Result<IEnumerable<GetInvitationVm>>>(responseString);

            //Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            result.Value.Should().HaveCount(expectedCount);
        }

        [Fact]
        public async Task Create_ForInvalidGroupRoleInAssignment_ReturnsForbidden()
        {
            //Arrange
            await ClearContext();
            var scopeFactory = _factory.Services.GetService<IServiceScopeFactory>();
            using var scope = scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetService<ApplicationDbContext>();
            await SeedDataHelper.SeedUsers(context);

            var group = getValidGroup(99);
            await context.AddAsync(group);
            await context.SaveChangesAsync();

            var assignment = new Assignment
            {
                GroupId = group.Id,
                UserId = 99,
                GroupRoleId = 3,
            };
            await context.Assignments.AddAsync(assignment);
            await context.SaveChangesAsync();

            CreateInvitationDto createInvitationDto = new CreateInvitationDto
            {
                ReceiverEmail = "johnsmith@gmail.com",
                RoleName = "Moderator"
            };
            var httpContent = createInvitationDto.ToJsonHttpContent();

            //Act
            var response = await _client.PostAsync($"api/group/{group.Id}/invitation", httpContent);
            var responseString = await response.Content.ReadAsStringAsync();

            //Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.Forbidden);
        }

        public static IEnumerable<object[]> GetSampleInvalidCreateModels()
        {
            var list = new List<CreateInvitationDto>()
            {
                new CreateInvitationDto
                {
                    ReceiverEmail = "testinvitation@gmail.com",
                    RoleName = "ndsjas"
                },
                new CreateInvitationDto
                {
                    ReceiverEmail = "dsnaudnas@dsnmaiod.ssa",
                    RoleName = "Moderator"
                },
                new CreateInvitationDto
                {
                    ReceiverEmail = "dsnaudnas@dsnmaiod.ssa",
                    RoleName = "ndsjas"
                }
            };
            return list.Select(x => new object[] { x });
        }


        private async Task ClearContext()
        {
            var scopeFactory = _factory.Services.GetService<IServiceScopeFactory>();
            using var scope = scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetService<ApplicationDbContext>();

            context.Groups.Clear();
            context.Assignments.Clear();
            context.Users.Clear();
            context.Invitations.Clear();

            await context.SaveChangesAsync();
        }      
        private Group getValidGroup(int creatorId)
        {
            return new Group
            {
                Name = "Grupa1",
                CreatorId = creatorId
            };
        }
      
        private Invitation getInvitation(int senderId, int receiverId, int groupId, int groupRoleId, string status)
        {
            return new Invitation
            {
                SenderId = senderId,
                ReceiverId = receiverId,
                GroupId = groupId,
                GroupRoleId = groupRoleId,
                Status = status
            };
        }       
    }
}
