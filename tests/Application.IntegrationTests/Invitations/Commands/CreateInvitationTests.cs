using Application.Common.Exceptions;
using Application.Invitations.Commands.CreateInvitation;
using Domain.Entities;
using FluentAssertions;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPI;
using WebAPI.IntegrationTests;
using Xunit;

namespace Application.IntegrationTests.Invitations.Commands
{
    public class CreateInvitationTests : SharedUtilityClass, IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        public CreateInvitationTests(CustomWebApplicationFactory<Startup> factory) : base(factory)
        {
        }

        [Fact]
        public async Task ForValidModel_ReturnsOkWithValidProperties()
        {
            //Arrange
            await ClearNotNecesseryData();
            var userId = await RunAsDefaultUserAsync();
            var users = await SeedUsers();
            var groupRoles = await SeedPermissionWithGroupRoles();
            var group = await AddAsync(new Group { Name = "Grupa1", CreatorId = userId });
            var assignment = await AddAsync
                (new Assignment { GroupId = group.Id, UserId = userId, GroupRoleId = groupRoles["Owner"] });

            var command = new CreateInvitationCommand
            {
                ReceiverEmail = "johnsmith@gmail.com",
                RoleName = "Moderator",
                GroupId = group.Id,
            };

            //Act
            var result = await SendAsync(command);

            //Assert
            var scopeFactory = _factory.Services.GetService<IServiceScopeFactory>();
            using var scope = scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetService<ApplicationDbContext>();

            var invitation = await context.Invitations.Include(x => x.GroupRole).Include(x => x.Receiver).Include(x => x.Group)
                .SingleOrDefaultAsync();
            invitation.Receiver.Email.Should().Be("johnsmith@gmail.com");
            invitation.Group.Should().NotBeNull();
            invitation.GroupRole.Name.Should().Be("Moderator");
        }

        [Fact]
        public async Task ForSenderIsNotInTheGroup_ReturnsBadRequest()
        {
            //Arrange
            await ClearNotNecesseryData();
            var userId = await RunAsDefaultUserAsync();
            var users = await SeedUsers();
            var groupRoles = await SeedPermissionWithGroupRoles();
            var group = await AddAsync(new Group { Name = "Grupa1", CreatorId = userId });
            var assignment = await AddAsync
                (new Assignment { GroupId = group.Id, UserId = users["user3"], GroupRoleId = groupRoles["Checker"] });

            var command = new CreateInvitationCommand
            {
                ReceiverEmail = "johnsmith@gmail.com",
                RoleName = "Moderator",
                GroupId = group.Id,
            };

            //Act
            Func<Task> func = async () => await SendAsync(command);

            //Assert
            await func.Should().ThrowAsync<ValidationException>();
        }

        [Theory]
        [MemberData(nameof(GetSampleInvalidCreateModels))]
        public async Task ForInvalidModels_ReturnsBadRequest(CreateInvitationCommand createInvitationDto)
        {
            //Arrange
            await ClearNotNecesseryData();
            var userId = await RunAsDefaultUserAsync();
            var users = await SeedUsers();
            var groupRoles = await SeedPermissionWithGroupRoles();
            await SeedUsers();

            var group = await AddAsync(new Group { Name = "Grupa1", CreatorId = userId });
            var assignment = await AddAsync
                (new Assignment { GroupId = group.Id, UserId = userId, GroupRoleId = groupRoles["Checker"] });

            //Act
            Func<Task> func = async () => await SendAsync(createInvitationDto);

            //Assert
            await func.Should().ThrowAsync<ValidationException>();
        }

        [Fact]
        public async Task ForTwoSameInvitations_ReturnsBadRequest()
        {
            //Arrange
            await ClearNotNecesseryData();
            var userId = await RunAsDefaultUserAsync();
            var users = await SeedUsers();
            var groupRoles = await SeedPermissionWithGroupRoles();
            var group = await AddAsync(new Group { Name = "Grupa1", CreatorId = userId });
            var assignment = await AddAsync
                (new Assignment { GroupId = group.Id, UserId = userId, GroupRoleId = groupRoles["Moderator"] });

            var command = new CreateInvitationCommand
            {
                ReceiverEmail = "johnsmith@gmail.com",
                RoleName = "Moderator",
                GroupId = group.Id,
            };

            //Act
            var result = await SendAsync(command);
            Func<Task> func = async () => await SendAsync(command);

            //Assert
            result.Should().BeGreaterThan(0);
            await func.Should().ThrowAsync<ValidationException>();
        }

        [Fact]
        public async Task ForUserAlreadyInGroup_ReturnsBadRequest()
        {
            //Arrange
            await ClearNotNecesseryData();
            var userId = await RunAsDefaultUserAsync();
            var users = await SeedUsers();
            var groupRoles = await SeedPermissionWithGroupRoles();
            var group = await AddAsync(new Group { Name = "Grupa1", CreatorId = userId });
            var assignment = await AddAsync
                (new Assignment { GroupId = group.Id, UserId = users["user1"], GroupRoleId = groupRoles["Checker"] });

            var command = new CreateInvitationCommand
            {
                ReceiverEmail = "testinvitation@gmail.com",
                RoleName = "Moderator"
            };

            //Act
            Func<Task> func = async () => await SendAsync(command);

            //Assert
            await func.Should().ThrowAsync<ValidationException>();
        }

        [Fact]
        public async Task ForInvalidGroupRoleInAssignment_ReturnsForbidden()
        {
            //Arrange
            await ClearNotNecesseryData();
            var userId = await RunAsDefaultUserAsync();
            var users = await SeedUsers();
            var groupRoles = await SeedPermissionWithGroupRoles();

            var group = await AddAsync(new Group { Name = "Grupa1", CreatorId = userId });
            var assignment = await AddAsync
                (new Assignment { GroupId = group.Id, UserId = userId, GroupRoleId = groupRoles["Checker"] });

            var command = new CreateInvitationCommand
            {
                ReceiverEmail = "johnsmith@gmail.com",
                RoleName = "Moderator",
                GroupId = group.Id
            };

            //Act
            Func<Task> func = async () => await SendAsync(command);

            //Assert
            await func.Should().ThrowAsync<ForbidException>();
        }

        [Fact]
        public async Task ForSendingInvitationForYourself_ReturnsBadRequest()
        {
            //Arrange
            await ClearNotNecesseryData();
            var userId = await RunAsDefaultUserAsync();
            var users = await SeedUsers();
            var groupRoles = await SeedPermissionWithGroupRoles();
            var group = await AddAsync(new Group { Name = "Grupa1", CreatorId = userId });
            var assignment = await AddAsync
                (new Assignment { GroupId = group.Id, UserId = userId, GroupRoleId = groupRoles["Moderator"] });

            var command = new CreateInvitationCommand
            {
                ReceiverEmail = "user@gmail.com",
                RoleName = "Moderator",
                GroupId = group.Id,
            };

            //Act
            Func<Task> func = async () => await SendAsync(command);

            //Assert
            await func.Should().ThrowAsync<ValidationException>();
        }

        public static IEnumerable<object[]> GetSampleInvalidCreateModels()
        {
            var list = new List<CreateInvitationCommand>()
            {
                new CreateInvitationCommand
                {
                    ReceiverEmail = "dsnaudnas@dsnmaiod.ssa",
                    RoleName = "Moderator",
                    GroupId = 1
                },
                new CreateInvitationCommand
                {
                    ReceiverEmail = "testinvitation@gmail.com",
                    RoleName = "ndsjas",
                    GroupId = 1
                },
                new CreateInvitationCommand
                {
                    ReceiverEmail = "dsnaudnas@dsnmaiod.ssa",
                    RoleName = "ndsjas",
                    GroupId = 1
                }
            };
            return list.Select(x => new object[] { x });
        }
    }
}
