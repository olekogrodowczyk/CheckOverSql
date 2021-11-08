using Application.Dto.CreateInvitationDto;
using Application.Dto.RegisterUserVm;
using Application.Responses;
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
using WebAPI.IntegrationTests.FakeAuthentication;
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

        private async Task<int> registerUser(string email)
        {
            var registerUserDto = new RegisterUserDto
            {
                Email = email,
                FirstName = "John",
                LastName = "Smith",
                DateOfBirth = DateTime.UtcNow.AddYears(-20),
                Password = "password123",
                ConfirmPassword = "password123"
            };

            var httpContent = registerUserDto.ToJsonHttpContent();

            var response = await _client.PostAsync("api/account/register", httpContent);
            var responseString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<Result<int>>(responseString);
            return result.Value;
        }

        private Group getValidGroup()
        {
            return new Group
            {
                Name = "Grupa1",
                CreatorId = FakeUserId.Value,
            };
        }

        private async Task<(Group,int)> initGroupAndRegisterUser()
        {
            var scopeFactory = _factory.Services.GetService<IServiceScopeFactory>();
            using var scope = scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetService<ApplicationDbContext>();

            var group = getValidGroup();
            await context.Groups.AddAsync(group);
            await context.SaveChangesAsync();

            int userId = await registerUser("testinvitation@gmail.com");

            return (group,userId);
        }


        [Fact]
        public async Task Create_ForValidModel_ReturnsOkWithValidProperties()
        {
            //Arrange
            await ClearContext();
            var scopeFactory = _factory.Services.GetService<IServiceScopeFactory>();
            using var scope = scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetService<ApplicationDbContext>();

            var initResult = await initGroupAndRegisterUser();

            CreateInvitationDto createInvitationDto = new CreateInvitationDto
            {
                ReceiverEmail = "testinvitation@gmail.com",
                RoleName = "Moderator"
            };
            var httpContent = createInvitationDto.ToJsonHttpContent();

            //Act
            var response = await _client.PostAsync($"api/group/{initResult.Item1.Id}/invitation", httpContent);

            //Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            var invitation = await context.Invitations
                .Include(x => x.GroupRole)
                .Include(x => x.Receiver)
                .Include(x => x.Group)
                .SingleOrDefaultAsync();
            invitation.Receiver.Email.Should().Be("testinvitation@gmail.com");
            invitation.Group.Should().NotBeNull();
            invitation.GroupRole.Name.Should().Be("Moderator");
        }

        [Theory]
        [MemberData(nameof(GetSampleInvalidCreateModels))]
        public async Task Create_ForInvalidModels_ReturnsBadRequest(CreateInvitationDto createInvitationDto)
        {
            //Arrange
            await ClearContext();
            var initResult = await initGroupAndRegisterUser();

            var httpContent = createInvitationDto.ToJsonHttpContent();

            //Act
            var response = await _client.PostAsync($"api/group/{initResult.Item1.Id}/invitation", httpContent);

            //Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Create_ForTwoSameInvitations_ReturnsBadRequest()
        {
            //Arrange
            await ClearContext();
            var initResult = await initGroupAndRegisterUser();

            CreateInvitationDto createInvitationDto = new CreateInvitationDto
            {
                ReceiverEmail = "testinvitation@gmail.com",
                RoleName = "Moderator"
            };

            var httpContent = createInvitationDto.ToJsonHttpContent();

            //Act
            var response = await _client.PostAsync($"api/group/{initResult.Item1.Id}/invitation", httpContent);
            var response2 = await _client.PostAsync($"api/group/{initResult.Item1.Id}/invitation", httpContent);

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

            var initResult = await initGroupAndRegisterUser();

            var assignment = new Assignment
            {
                GroupId = initResult.Item1.Id,
                UserId = initResult.Item2,
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
            var response = await _client.PostAsync($"api/group/{initResult.Item1.Id}/invitation", httpContent);
            var responseString = await response.Content.ReadAsStringAsync();

            //Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }

    }
}
