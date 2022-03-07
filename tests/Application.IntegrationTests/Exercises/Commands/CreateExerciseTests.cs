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
        public async Task ForValidDto_ReturnsValidResult()
        {
            //Arrange
            await ClearNotNecesseryData();
            int userId = await RunAsDefaultUserAsync();
            var command = new CreateExerciseCommand
            {
                DatabaseName = "NorthwindSimple",
                Description = "Opis2dsadsa",
                Title = "Zadanie2 title",
                ValidAnswer = "SELECT * FROM Products",
                IsPrivate = true,
            };

            //Act
            var itemId = await SendAsync(command);
            var item = await FindAsync<Exercise>(itemId);

            //Assert
            item.Should().NotBeNull();
        }

        [Fact]
        public async Task ForInvalidDto_ThrowsValidationException()
        {
            //Arrange
            await ClearNotNecesseryData();
            var command = new CreateExerciseCommand
            {
                DatabaseName = "dsadwqdwq",
                Description = "Opis2dsadsa",
                Title = "Zadanie2 title",
                ValidAnswer = "SELECT * FROM Products"
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
                DatabaseName = "NorthwindSimple",
                Description = "Opis2dsadsa",
                Title = "Zadanie2 title",
                ValidAnswer = "INSERT INTO OrderItem VALUES(1, 11, 14.80, 12)",
                IsPrivate = true,
            };

            //Act
            Func<Task> func = async () => await SendAsync(command);

            //Assert
            await func.Should().ThrowAsync<SqlException>();
        }
    }
}
