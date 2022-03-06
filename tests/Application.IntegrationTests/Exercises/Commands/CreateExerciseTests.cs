using Application.Exercises.Commands.CreateExercise;
using FluentAssertions;
using System.Threading.Tasks;
using WebAPI.IntegrationTests.Helpers;
using Xunit;
using MediatR;
using Domain.Entities;
using Application.Common.Exceptions;
using Microsoft.Data.SqlClient;
using System;

namespace WebAPI.IntegrationTests.Exercises.Commands
{
    public class CreateExerciseTests : SharedUtilityClass, IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        public CreateExerciseTests(CustomWebApplicationFactory<Startup> factory) : base(factory)
        {
        }

        [Fact]
        public async Task ForValidDto_ReturnsOk()
        {
            //Arrange
            await ClearNotNecesseryData();
            int userId = await RunAsDefaultUserAsync();
            var command = new CreateExerciseCommand
            {
                DatabaseName = "FootballLeague",
                Description = "Opis2dsadsa",
                Title = "Zadanie2 title",
                ValidAnswer = "SELECT * FROM dbo.Footballers",
                IsPrivate = true,
            };

            //Act
            var itemId = await SendAsync(command);
            var item = await FindAsync<Exercise>(itemId);

            //Assert
            item.Should().NotBeNull();
        }

        [Fact]
        public async Task ForInvalidDto_ReturnsBadRequest()
        {
            //Arrange
            await ClearNotNecesseryData();
            var command = new CreateExerciseCommand
            {
                DatabaseName = "dsadwqdwq",
                Description = "Opis2dsadsa",
                Title = "Zadanie2 title",
                ValidAnswer = "SELECT * FROM dbo.Footballers"
            };

            //Act
            Func<Task> func = async () => await SendAsync(command);

            //Assert
            await func.Should().ThrowAsync<ValidationException>();
        }

        [Fact]
        public async Task ForForbiddenQuery_ReturnsBadRequest()
        {
            //Arrange
            await ClearNotNecesseryData();
            var command = new CreateExerciseCommand
            {
                DatabaseName = "FootballLeague",
                Description = "Opis2dsadsa",
                Title = "Zadanie2 title",
                ValidAnswer = "INSERT INTO dbo.Footballers (FirstName, LastName) VALUES ('Leo','Messi')",
                IsPrivate = true,
            };

            //Act
            Func<Task> func = async () => await SendAsync(command);

            //Assert
            await func.Should().ThrowAsync<SqlException>();
        }
    }
}
