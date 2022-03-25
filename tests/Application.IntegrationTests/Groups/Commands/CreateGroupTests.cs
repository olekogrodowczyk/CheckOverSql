using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Groups.Commands.CreateGroup;
using Domain.Entities;
using Domain.Enums;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPI;
using WebAPI.IntegrationTests;
using Xunit;

namespace Application.IntegrationTests.Groups.Commands
{
    public class CreateGroupTests : SharedUtilityClass, IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        public CreateGroupTests(CustomWebApplicationFactory<Startup> factory) : base(factory)
        {
        }

        [Fact]
        public async Task ForValidModel_ReturnsOk()
        {
            //Arrange
            await ClearNotNecesseryData();
            var userId = await RunAsDefaultUserAsync();
            var command = new CreateGroupCommand
            {
                Name = "Group 1 - C# programming"
            };

            //Act
            var result = await SendAsync(command);
            var group = await FindAsync<Group>(result);

            //Assert
            group.Should().NotBeNull();
            if (group is not null)
            {
                group.Id.Should().Be(result);
            }
        }

        [Fact]
        public async Task Create_ForInvalidModel_ReturnsBadRequest()
        {
            //Arrange
            await ClearNotNecesseryData();
            var userId = await RunAsDefaultUserAsync();
            var command = new CreateGroupCommand
            {
                Name = "Gr"
            };

            //Act
            Func<Task<int>> func = async () => await SendAsync(command);

            //Assert
            await func.Should().ThrowAsync<ValidationException>();
        }

        [Fact]
        public async Task ForCreatedGroup_CreatesAssignmentWithOwnerGroupRole()
        {
            //Arrange
            await ClearNotNecesseryData();
            var userId = await RunAsDefaultUserAsync();
            var command = new CreateGroupCommand
            {
                Name = "Group1",
                Image = new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("dummy image")), 0, 0, "Data", "image.png")
            };

            //Act
            var result = await SendAsync(command);
            var group = await FindAsync<Group>(result);
            var assignment = await FirstOrDefaultAsync<Assignment>(x => x.UserId == userId && x.GroupId == result);
            var ownerGroupRole = await FirstOrDefaultAsync<GroupRole>(x => x.Name == GroupRoleEnum.Owner.ToString());

            //Assert
            group.Should().NotBeNull();
            ownerGroupRole.Should().NotBeNull();
            assignment.Should().NotBeNull();
            if (group is not null && ownerGroupRole is not null && assignment is not null)
            {
                group.Id.Should().Be(result);
                group.CreatorId.Should().Be(userId);
                assignment.GroupRoleId.Should().Be(ownerGroupRole.Id);
            }
        }
    }
}
