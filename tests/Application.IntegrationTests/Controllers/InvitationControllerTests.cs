using Application.Dto.CreateInvitationDto;
using Application.Dto.RegisterUserVm;
using Application.Responses;
using Application.ViewModels;
using Domain.Entities;
using Domain.Enums;
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
    

    public class InvitationControllerTests : SharedUtilityClass, IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        public InvitationControllerTests(CustomWebApplicationFactory<Startup> factory) : base(factory) { }

        [Fact]
        public async Task Create_ForValidModel_ReturnsOkWithValidProperties()
        {
            //Arrange
            await ClearInvitationContext();
            await SeedUsers();

            var group = await addNewEntity<Group>(new Group { Name = "Grupa1", CreatorId = 99 });
            var assignment = await addNewEntity<Assignment>(new Assignment { GroupId = group.Id, UserId = 99, GroupRoleId = 2 });

            var httpContent = new CreateInvitationDto
            {
                ReceiverEmail = "johnsmith@gmail.com",
                RoleName = "Moderator"
            }.ToJsonHttpContent();

            //Act
            var response = await _client.PostAsync
                (ApiRoutes.Invitation.Create.Replace("{groupId}", group.Id.ToString()), httpContent);
            var responseString = await response.Content.ReadAsStringAsync();

            //Assert
            var scopeFactory = _factory.Services.GetService<IServiceScopeFactory>();
            using var scope = scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetService<ApplicationDbContext>();

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
            await ClearInvitationContext();
            await SeedUsers();

            var group = await addNewEntity<Group>(new Group { Name = "Grupa1", CreatorId = 99 });
            var assignment = await addNewEntity<Assignment>
                (new Assignment { GroupId = group.Id, UserId = 102, GroupRoleId = 2 });

            var httpContent = new CreateInvitationDto
            {
                ReceiverEmail = "johnsmith@gmail.com",
                RoleName = "Moderator"
            }.ToJsonHttpContent();

            //Act
            var response = await _client.
                PostAsync(ApiRoutes.Invitation.Create.Replace("{groupId}", group.Id.ToString()), httpContent);

            //Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }

        [Theory]
        [MemberData(nameof(GetSampleInvalidCreateModels))]
        public async Task Create_ForInvalidModels_ReturnsBadRequest(CreateInvitationDto createInvitationDto)
        {
            //Arrange
            await ClearInvitationContext();
            await SeedUsers();

            var group = await addNewEntity<Group>(new Group { Name = "Grupa1", CreatorId = 99 });
            var assignment = await addNewEntity<Assignment>
                (new Assignment { GroupId = group.Id, UserId = 99, GroupRoleId = 2 });

            var httpContent = createInvitationDto.ToJsonHttpContent();

            //Act
            var response = await _client.PostAsync
                (ApiRoutes.Invitation.Create.Replace("{groupId}", group.Id.ToString()), httpContent);

            //Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Create_ForTwoSameInvitations_ReturnsBadRequest()
        {
            //Arrange
            await ClearInvitationContext();
            await SeedUsers();

            var group = await addNewEntity<Group>(new Group { Name = "Grupa1", CreatorId = 99 });
            var assignment = await addNewEntity<Assignment>
                (new Assignment { GroupId = group.Id, UserId = 99, GroupRoleId = 2 });

            var httpContent = new CreateInvitationDto
            {
                ReceiverEmail = "johnsmith@gmail.com",
                RoleName = "Moderator"
            }.ToJsonHttpContent();

            //Act
            var response = await _client.PostAsync
                (ApiRoutes.Invitation.Create.Replace("{groupId}", group.Id.ToString()), httpContent);
            var response2 = await _client.PostAsync
                (ApiRoutes.Invitation.Create.Replace("{groupId}", group.Id.ToString()), httpContent);

            //Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            response2.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Create_ForUserAlreadyInGroup_ReturnsBadRequest()
        {
            //Arrange
            await ClearInvitationContext();
            await SeedUsers();

            var group = await addNewEntity<Group>(new Group { Name = "Grupa1", CreatorId = 99 });
            var assignment = await addNewEntity<Assignment>
                (new Assignment { GroupId = group.Id, UserId = 100, GroupRoleId = 2 });

            var httpContent = new CreateInvitationDto
            {
                ReceiverEmail = "testinvitation@gmail.com",
                RoleName = "Moderator"
            }.ToJsonHttpContent();

            //Act
            var response = await _client.PostAsync
                (ApiRoutes.Invitation.Create.Replace("{groupId}", group.Id.ToString()), httpContent);
            var responseString = await response.Content.ReadAsStringAsync();

            //Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }

        [Theory]
        [InlineData(99, 100, 2, "getallsent")]
        [InlineData(100, 99, 1, "getallsent")]
        [InlineData(99, 101, 2, "getallsent")]
        [InlineData(99, 100, 1, "getallreceived")]
        [InlineData(100, 99, 2, "getallreceived")]
        [InlineData(100, 99, 3, "getall")]
        public async Task GetAllWithCondition_ForValidModel_ReturnsOkWithValidCount
            (int senderId, int receiverId, int expectedCount, string queryType)
        {
            //Arrange
            await ClearInvitationContext();
            await SeedUsers();

            var group = await addNewEntity<Group>(new Group { Name = "Grupa1", CreatorId = 99 });

            var invitation1 = await addNewEntity<Invitation>
                (new Invitation { SenderId = senderId, ReceiverId = receiverId, GroupId = group.Id, GroupRoleId = 2
                    , Status = InvitationStatus.Sent.ToString() });

            var invitation2 = await addNewEntity<Invitation>
                (new Invitation { SenderId = 101, ReceiverId = 99, GroupId = group.Id, GroupRoleId = 2
                    , Status = InvitationStatus.Accepted.ToString() });

            var invitation3 = await addNewEntity<Invitation>
                (new Invitation { SenderId = 99, ReceiverId = 102, GroupId = group.Id, GroupRoleId = 2
                    , Status = InvitationStatus.Sent.ToString() });

            //Act
            var response = await _client.GetAsync
                (ApiRoutes.Invitation.Base.Replace("{groupId}", group.Id.ToString()) + $"/{queryType}");
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
            await ClearInvitationContext();
            await SeedUsers();

            var group = await addNewEntity<Group>(new Group { Name = "Grupa1", CreatorId = 99 });
            var assignment = await addNewEntity<Assignment>
                (new Assignment { GroupId = group.Id, UserId = 99, GroupRoleId = 3 });

            var httpContent = new CreateInvitationDto
            {
                ReceiverEmail = "johnsmith@gmail.com",
                RoleName = "Moderator"
            }.ToJsonHttpContent();

            //Act
            var response = await _client.PostAsync(ApiRoutes.Invitation.Create.Replace("{groupId}", group.Id.ToString()), httpContent);

            //Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.Forbidden);
        }

        [Theory]
        [InlineData("accept")]
        [InlineData("reject")]
        public async Task AcceptAndReject_ForValidInvitation_ReturnsOk(string queryType)
        {
            //Arrange
            await ClearInvitationContext();
            await SeedUsers();

            var group = await addNewEntity<Group>(new Group { Name = "Grupa1", CreatorId = 100 });
            var assignment = await addNewEntity<Assignment>
                (new Assignment { GroupId = group.Id, UserId = 100, GroupRoleId = 2 });
            var invitation = await addNewEntity<Invitation>(new Invitation
            {
                SenderId = 100,
                ReceiverId = 99,
                GroupId = group.Id,
                Status = InvitationStatus.Sent.ToString(),
            });

            //Act
            string routeBase = queryType == "accept" ? ApiRoutes.Invitation.Accept : ApiRoutes.Invitation.Reject;
            string route = routeBase.Replace("{invitationId}", invitation.Id.ToString());

            var response = await _client.PatchAsync(route, null);
            var responseString = await response.Content.ReadAsStringAsync();
  
            //Assert
            var scopeFactory = _factory.Services.GetService<IServiceScopeFactory>();
            using var scope = scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetService<ApplicationDbContext>();

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            var assignmentExists = await context.Assignments.AnyAsync(x => x.UserId == invitation.SenderId);
            assignmentExists.Should().BeTrue();
        }

        [Fact]
        public async Task Create_ForSendingInvitationForYourself_ReturnsBadRequest()
        {
            //Arrange
            await ClearInvitationContext();
            await SeedUsers();
            var group = await addNewEntity<Group>(new Group { Name = "Grupa1", CreatorId = 99 });
            var assignment = await addNewEntity<Assignment>(new Assignment { GroupId = group.Id, UserId = 99, GroupRoleId = 3 });

            var httpContent = new CreateInvitationDto
            {
                ReceiverEmail = "testfakeuser@gmail.com",
                RoleName = "Moderator"
            }.ToJsonHttpContent();

            //Act
            var response = await _client.PostAsync(ApiRoutes.Invitation.Create, httpContent);
            var responseString = await response.Content.ReadAsStringAsync();

            //Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
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

        private async Task ClearInvitationContext()
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
    }
}
