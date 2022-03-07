using Application.Databases.Queries.GetQueryValueAdmin;
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
using Xunit;

namespace Application.IntegrationTests.Databases.Queries
{
    public class GetQueryValueTests : SharedUtilityClass, IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        public GetQueryValueTests(CustomWebApplicationFactory<Startup> factory) : base(factory)
        {
        }

        [Fact]
        public async Task ForGivenData_ReturnsValidResult()
        {
            //Arrange
            await RunAsDefaultUserAsync();
            var query = new GetQueryValueQuery()
            {
                DatabaseName = "NorthwindSimple",
                Query = @"SELECT TOP(10) OrderId, COUNT(*) COUNT FROM OrderItems 
                GROUP BY OrderId;"
            };

            //Act
            var result = await SendAsync(query);

            //Assert
            result.Should().NotBeNull();
            result.Should().NotBeEmpty();
            result.Should().HaveCount(11);
        }

        [Theory]
        [MemberData(nameof(GetInvalidQueries))]
        public async Task ForInvalidQuery_ThrowsSqlException(string invalidQuery)
        {
            //Arrange
            await RunAsDefaultUserAsync();
            var query = new GetQueryValueQuery()
            {
                DatabaseName = "NorthwindSimple",
                Query = invalidQuery
            };

            //Act
            Func<Task> func = () => SendAsync(query);

            //Assert
            await func.Should().ThrowAsync<SqlException>();
        }

        public static IEnumerable<object[]> GetInvalidQueries()
        {
            var list = new List<string>()
            {
                "dnqwu",
                "SELECT",
                "INSERT",
                "SELECT ** FROM OrderItems",
                "SELECT Id, Idd FROM ORDER ITEMS",
                "INSERT INTO ORDERS VALUES(1)",
                "INSERT INTO OrderItems VALUES(1, 11, 14.80, 10)"
            };
            return list.Select(x => new object[] { x
           });
        }


    }
}
