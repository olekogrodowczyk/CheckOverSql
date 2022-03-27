using Application.Invitations.Commands.AcceptInvitation;
using Application.Invitations.Commands.RejectInvitation;
using Domain.Entities;
using Domain.Enums;
using FluentAssertions;
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
    public class AcceptInvitationTests : SharedUtilityClass, IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        public AcceptInvitationTests(CustomWebApplicationFactory<Startup> factory) : base(factory)
        {
        }

        [Theory]
        [InlineData("accept", true)]
        [InlineData("reject", false)]
        public async Task ForValidInvitation_ReturnsOk(string queryType, bool assignmentExists)
        {
            //Arrange
            await ClearNotNecesseryData();
            var userId = await RunAsDefaultUserAsync();
            var users = await SeedUsers();
            var groupRoles = await SeedPermissionWithGroupRoles();
            var group = await AddAsync(new Group { Name = "Grupa1", CreatorId = users["user1"] });
            var assignment = await AddAsync
                (new Assignment { GroupId = group.Id, UserId = users["user1"], GroupRoleId = groupRoles["Checker"] });
            var invitation = await AddAsync(new Invitation
            {
                SenderId = users["user1"],
                ReceiverId = userId,
                GroupId = group.Id,
                Status = InvitationStatusEnum.Sent,
            });

            //Act
            Func<Task> func = queryType == "accept"
            ? async () => await SendAsync(new AcceptInvitationCommand { InvitationId = invitation.Id })
            : async () => await SendAsync(new RejectInvitationCommand { InvitationId = invitation.Id });
            await func.Invoke();

            //Assert
            var assignmentsExists = await AnyAsync<Assignment>(x => x.UserId == invitation.ReceiverId);
            assignmentExists.Should().Be(assignmentExists);
        }
    }
}
