using Application.Common.Exceptions;
using Application.IntegrationTests.Helpers;
using Application.Solvings.Commands.CheckExercise;
using Domain.Entities;
using FluentAssertions;
using System;
using System.Threading.Tasks;
using WebAPI;
using WebAPI.IntegrationTests;
using Xunit;

namespace Application.IntegrationTests.Solvings.Commands
{
    public class CheckSolvingTests : SharedUtilityClass, IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        public CheckSolvingTests(CustomWebApplicationFactory<Startup> factory) : base(factory)
        {
        }

        [Theory]
        [InlineData("Owner")]
        [InlineData("Moderator")]
        [InlineData("Checker")]
        public async Task ForCreatedCheckingAndDefinedRoles_ReturnsValidIdAndChangedSolvingStatus(string roleName)
        {
            //Arrange
            Solving solving = await arrangeData(roleName);

            //Act
            var result = await SendAsync
                (new CheckSolvingCommand() { Points = 10, Remarks = "Nice!", SolvingId = solving.Id });
            var checking = await FindAsync<Checking>(result);

            //Assert
            checking.Should().NotBeNull();
            result.Should().Be(checking.Id);
        }

        [Fact]
        public async Task ForCreatedCheckingAndUser_ThrowsForbidException()
        {
            //Arrange
            Solving solving = await arrangeData("User");

            //Act
            Func<Task<int>> func = async () => await SendAsync
                (new CheckSolvingCommand() { Points = 10, Remarks = "Nice!", SolvingId = solving.Id });

            //Assert
            await func.Should().ThrowAsync<ForbidException>();
        }

        [Fact]
        public async Task ForInvalidData_ThrowsValidationException()
        {
            //Arrange
            Solving solving = await arrangeData("User");

            //Act
            Func<Task<int>> func = async () => await SendAsync
                (new CheckSolvingCommand() { Remarks = "Nice!", SolvingId = solving.Id });

            //Assert
            await func.Should().ThrowAsync<ValidationException>();
        }

        [Fact]
        public async Task ForInvalidSolvingId_ThrowsNotFoundException()
        {
            //Arrange
            Solving solving = await arrangeData("User");

            //Act
            Func<Task<int>> func = async () => await SendAsync
                (new CheckSolvingCommand() { Points = 10, Remarks = "Nice!", SolvingId = solving.Id + 1 });

            //Assert
            await func.Should().ThrowAsync<NotFoundException>();
        }

        private async Task<Solving> arrangeData(string roleName)
        {
            await ClearNotNecesseryData();
            int userId = await RunAsDefaultUserAsync();
            var users = await SeedUsers();
            var groupRoles = await SeedPermissionWithGroupRoles();
            var group = await AddAsync(new Group() { CreatorId = users["user3"] });
            var assignment = await AddAsync(new Assignment(userId, group.Id, groupRoles[roleName]));
            var exercise = await AddAsync(GetValidNoDataReturnsExercise(DatabasesIdsHelper.NorthwindSimpleDatabaseId));
            var solving = await AddAsync(new Solving()
            {
                Status = Domain.Enums.SolvingStatusEnum.Done,
                CreatorId = userId,
                AssignmentId = assignment.Id,
                ExerciseId = exercise.Id,
            });
            return solving;
        }
    }
}
