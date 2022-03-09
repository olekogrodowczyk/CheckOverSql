﻿using Application.Exercises.Queries.GetAllCreated;
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
    public class GetAllCreatedExercisesTests : SharedUtilityClass, IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        public GetAllCreatedExercisesTests(CustomWebApplicationFactory<Startup> factory) : base(factory)
        {
        }

        [Fact]
        public async Task ForCreatedSampleData_ReturnsCreatedExercises()
        {
            //Arrange
            await ClearNotNecesseryData();
            int userId = await RunAsDefaultUserAsync();
            var users = await SeedUsers();

            var exercise1 = await AddAsync<Exercise>(GetValidFootballersExercise(true, userId));
            var exercise2 = await AddAsync<Exercise>(GetValidFootballersExercise(false, userId));
            var exercise3 = await AddAsync<Exercise>(GetValidFootballersExercise(false, users["user3"]));

            //Act
            var result = await SendAsync(new GetAllCreatedExercisesQuery() { PageNumber = 1, PageSize = 8 });

            //Assert
            result.Items.Should().HaveCount(2);
        }

        [Fact]
        public async Task ForSampleData_ReturnsValidPaginationResult()
        {
            //Arrange
            int userId = await RunAsDefaultUserAsync();
            for (int i = 0; i < 10; i++)
            {
                await AddAsync<Exercise>(GetValidFootballersExercise(true, userId));
            }
            await AddAsync<Exercise>(GetValidFootballersExercise(false, userId));

            //Act
            var result = await SendAsync(new GetAllCreatedExercisesQuery() { PageNumber = 1, PageSize = 8 });
            var result2 = await SendAsync(new GetAllCreatedExercisesQuery() { PageNumber = 2, PageSize = 8 });
            var result3 = await SendAsync(new GetAllCreatedExercisesQuery() { PageNumber = 3, PageSize = 8 });

            //Assert
            result.Items.Should().HaveCount(8);
            result2.Items.Should().HaveCount(3);
            result3.Items.Should().HaveCount(0);
        }
    }
}
