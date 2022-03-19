using Application.Exercises.Commands.CreateExercise;
using Application.IntegrationTests.Helpers;
using Domain.Entities;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPI;
using WebAPI.IntegrationTests;
using WebAPI.Seeders;
using Xunit;

namespace Application.IntegrationTests.Seeders
{
    public class ExercisesSeederTests : SharedUtilityClass, IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        int id = DatabasesIdsHelper.NorthwindSimpleDatabaseId;

        public ExercisesSeederTests(CustomWebApplicationFactory<Startup> factory) : base(factory)
        {
        }

        [Theory]
        [MemberData(nameof(getNorthwindSimpleExercises))]
        public async Task ForNorthwindSimpleExercisesFromSeeder_ShouldNotThrowSqlException(Exercise exercise)
        {
            //Arrange
            await RunAsDefaultUserAsync();
            var command = new CreateExerciseCommand()
            {
                DatabaseName = "NorthwindSimple",
                IsPrivate = false,
                Title = exercise.Title,
                Description = exercise.Description,
                ValidAnswer = exercise.ValidAnswer,
            };

            //Act
            Func<Task> func = async () => await SendAsync(command);

            //Assert
            await func.Should().NotThrowAsync<SqlException>();
        }

        public static IEnumerable<object[]> getNorthwindSimpleExercises()
        {
            var list = ExercisesSeederData.GetNorthwindSimplePublicExercises(0, DatabasesIdsHelper.NorthwindSimpleDatabaseId);
            return list.Select(x => new object[] { x });
        }
    }
}
