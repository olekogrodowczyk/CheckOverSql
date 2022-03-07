using Application.Databases.Queries.GetQueryValueAdmin;
using Application.Solutions.Commands.CreateSolution;
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

namespace Application.IntegrationTests.Solutions.Commands
{
    public class QueryTest
    {
        public int DatabaseId { get; set; }
        public string ValidQuery { get; set; }
        public string SolutionQuery { get; set; }
        public bool Result { get; set; }
    }


    public class CreateSolutionTests : SharedUtilityClass, IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private const int northwindSimpleDbId = 1;

        public CreateSolutionTests(CustomWebApplicationFactory<Startup> factory) : base(factory)
        {
        }


        [Theory]
        [MemberData(nameof(GetSampleNorthwindSimpleQueries))]
        public async Task ForTwoQueriesAndNorthWindSimpleDb_ReturnsValidResult(QueryTest queryTest)
        {
            //Arrange
            int userId = await RunAsDefaultUserAsync();
            var exercise = await AddAsync(new Exercise()
            {
                DatabaseId = northwindSimpleDbId,
                Title = "Test",
                Description = "Test description",
                CreatorId = userId,
                IsPrivate = false,
                ValidAnswer = queryTest.ValidQuery
            });
            CreateSolutionCommand command = new CreateSolutionCommand()
            {
                ExerciseId = exercise.Id,
                Query = queryTest.SolutionQuery
            };

            //Act
            var result = await SendAsync(command);

            //Assert
            result.Result.Should().Be(queryTest.Result);
        }

        public static IEnumerable<object[]> GetSampleNorthwindSimpleQueries()
        {
            var list = new List<QueryTest>()
            {
                new QueryTest
                {
                    DatabaseId = northwindSimpleDbId,
                    ValidQuery = "SELECT * FROM Customers;",
                    SolutionQuery = "SELECT * FROM Customers;",
                    Result = true
                },
                new QueryTest
                {
                    DatabaseId = northwindSimpleDbId,
                    ValidQuery = "SELECT * FROM Customers;",
                    SolutionQuery = "SELECT FirstName FROM Customers;",
                    Result = false
                },
                new QueryTest
                {
                    DatabaseId = northwindSimpleDbId,
                    ValidQuery = "SELECT firstName from Customers",
                    SolutionQuery = "Select firstName from Customers",
                    Result = true
                },
                new QueryTest
                {
                    DatabaseId = northwindSimpleDbId,
                    ValidQuery = "SELECT firstName first from Customers",
                    SolutionQuery = "Select firstName from Customers",
                    Result = false
                },
                new QueryTest
                {
                    DatabaseId = northwindSimpleDbId,
                    ValidQuery = @"Select o.Id, o.TotalAmount, c.FirstName from Orders o 
                    INNER JOIN Customers c ON c.Id = o.CustomerId; ",
                    SolutionQuery = @"Select o.Id, o.TotalAmount, c.FirstName from Orders o 
                    INNER JOIN Customers c ON c.Id = o.CustomerId; ",
                    Result = true
                },
                new QueryTest
                {
                    DatabaseId = northwindSimpleDbId,
                    ValidQuery = @"Select o.Id, o.TotalAmount TotalAmount, c.FirstName from Orders o  
                    INNER JOIN Customers c ON c.Id = o.CustomerId; ",
                    SolutionQuery = @"Select o.Id, o.TotalAmount, c.FirstName from Orders o 
                    RIGHT JOIN Customers c ON c.Id = o.CustomerId; ",
                    Result = false
                },
                new QueryTest
                {
                    DatabaseId = northwindSimpleDbId,
                    ValidQuery = @"Select ord.Id OrderId, oi.UnitPrice UnitPriceOrderItem, p.Package from OrderItems oi
                    INNER JOIN Orders ord ON ord.Id = oi.OrderId
                    INNER JOIN Products p ON p.Id = oi.ProductId
                    ORDER BY p.UnitPrice DESC;",
                    SolutionQuery = @"Select ord.Id OrderId, oi.UnitPrice UnitPriceOrderItem, p.Package from OrderItems oi
                    INNER JOIN Orders ord ON ord.Id = oi.OrderId
                    INNER JOIN Products p ON p.Id = oi.ProductId
                    ORDER BY p.UnitPrice ASC;",
                    Result =false
                },
                new QueryTest
                {
                    DatabaseId = northwindSimpleDbId,
                    ValidQuery = @"Select ord.Id OrderId, oi.UnitPrice UnitPriceOrderItem, p.Package from OrderItems oi
                    INNER JOIN Orders ord ON ord.Id = oi.OrderId
                    INNER JOIN Products p ON p.Id = oi.ProductId
                    ORDER BY p.UnitPrice DESC;",
                    SolutionQuery = @"Select ord.Id OrderId, oi.UnitPrice UnitPriceOrderItem, p.Package from OrderItems oi
                    INNER JOIN Orders ord ON ord.Id = oi.OrderId
                    INNER JOIN Products p ON p.Id = oi.ProductId
                    ORDER BY p.Package ASC;",
                    Result = false
                },
                new QueryTest
                {
                    DatabaseId = northwindSimpleDbId,
                    ValidQuery = @"Select ord.Id OrderId, oi.UnitPrice UnitPriceOrderItem, p.Package from OrderItems oi
                    INNER JOIN Orders ord ON ord.Id = oi.OrderId
                    INNER JOIN Products p ON p.Id = oi.ProductId
                    ORDER BY p.UnitPrice DESC;",
                    SolutionQuery = @"Select ord.Id OrderId, oi.UnitPrice UnitPriceOrderItem, p.Package from OrderItems oi
                    RIGHT JOIN Orders ord ON ord.Id = oi.OrderId
                    RIGHT JOIN Products p ON p.Id = oi.ProductId
                    ORDER BY p.UnitPrice DESC;",
                    Result = false
                },
                new QueryTest
                {
                    DatabaseId = northwindSimpleDbId,
                    ValidQuery = @"SELECT OrderId, COUNT(UnitPrice) FROM OrderItems
                    GROUP BY OrderId;",
                    SolutionQuery = @"SELECT ProductId, COUNT(UnitPrice) FROM OrderItems
                    GROUP BY ProductId;",
                    Result = false
                },
                new QueryTest
                {
                    DatabaseId = northwindSimpleDbId,
                    ValidQuery = @"SELECT ProductId, COUNT(UnitPrice) COUNT FROM OrderItems
                    GROUP BY ProductId;",
                    SolutionQuery = @"SELECT ProductId, COUNT(*) COUNT FROM OrderItems
                    GROUP BY ProductId;",
                    Result = true
                },
                new QueryTest
                {
                    DatabaseId = northwindSimpleDbId,
                    ValidQuery = @"SELECT OrderId, SUM(UnitPrice) Sum FROM OrderItems
		            GROUP BY ROLLUP(OrderId);",
                    SolutionQuery = @"SELECT OrderId, SUM(UnitPrice) Sum FROM OrderItems
		            GROUP BY (OrderId) ",
                    Result = false
                },
                new QueryTest
                {
                    DatabaseId = northwindSimpleDbId,
                    ValidQuery = @"SELECT OrderId, SUM(UnitPrice) Sum FROM OrderItems
		            GROUP BY OrderId HAVING Sum(UnitPrice) < 50;",
                    SolutionQuery = @"SELECT OrderId, SUM(UnitPrice) Sum FROM OrderItems
		            GROUP BY OrderId;",
                    Result = false
                },
            };
            return list.Select(x => new object[] { x
           });
        }
    }
}
