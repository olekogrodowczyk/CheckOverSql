using Application.Solutions.Queries.GetAllSolutionsCreatedByUser;
using Domain.Entities;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPI;
using WebAPI.IntegrationTests;
using Xunit;

namespace Application.IntegrationTests.Solutions.Queries
{
    public class GetAllSolutionsCreatedByUserTests : SharedUtilityClass, IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        public GetAllSolutionsCreatedByUserTests(CustomWebApplicationFactory<Startup> factory) : base(factory)
        {
        }

        [Fact]
        public async Task ForSolutionsCreatedByUser_ReturnsAllTheseSolutions()
        {
            //Arrange
            await ClearNotNecesseryData();
            var user = await RunAsDefaultUserAsync();
            var users = await SeedUsers();

            string query = "SELECT * FROM Orders";
            var exercise = await AddAsync(GetValidNorthwindSimpleExercise());
            await AddAsync
                (new Solution { ExerciseId = exercise.Id, CreatorId = user, Query = query });
            await AddAsync
                (new Solution { ExerciseId = exercise.Id, CreatorId = users["user1"], Query = query });
            await AddAsync
                (new Solution { ExerciseId = exercise.Id, CreatorId = user, Query = query });

            //Act
            var result = await SendAsync(new GetAllSolutionsCreatedByUserQuery());

            //Assert
            result.Should().HaveCount(2);
        }
    }
}
