using Application.Exercises.Queries.GetAllPublicExercises;
using Domain.Entities;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace WebAPI.IntegrationTests.Exercises.Queries
{
    public class GetAllPublicExercisesTests : SharedUtilityClass, IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        public GetAllPublicExercisesTests(CustomWebApplicationFactory<Startup> factory) : base(factory)
        {
        }

        [Fact]
        public async Task ForSampleData_ReturnsValidResult()
        {
            //Arrange
            await ClearNotNecesseryData();
            var users = await SeedUsers();
            int userId = await RunAsDefaultUserAsync();

            for (int i = 0; i < 10; i++)
            {
                await AddAsync(GetValidExercise(false, users["user1"]));
            }
            await AddAsync(GetValidExercise(false, userId));
            await AddAsync(GetValidExercise(true, userId));

            //Act
            var result = await SendAsync(new GetAllPublicExercisesQuery() { PageNumber = 1, PageSize = 8 });
            var result2 = await SendAsync(new GetAllPublicExercisesQuery() { PageNumber = 2, PageSize = 8 });
            var result3 = await SendAsync(new GetAllPublicExercisesQuery() { PageNumber = 3, PageSize = 8 });

            //Assert
            result.Items.Should().HaveCount(8);
            result2.Items.Should().HaveCount(3);
            result3.Items.Should().HaveCount(0);
        }
    }
}
