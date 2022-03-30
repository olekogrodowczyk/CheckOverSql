using Application.Databases.Queries.GetQueryValueAdmin;
using Application.Solutions.Commands.CreateSolution;
using Domain.Entities;
using FluentAssertions;
using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPI;
using WebAPI.IntegrationTests;
using Xunit;
using Domain.Enums;
using Application.Solutions;
using Application.Common.Exceptions;

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

        private async Task<(int, int)> InitForCreate(DateTime deadline)
        {
            await ClearNotNecesseryData();
            var user = await RunAsDefaultUserAsync();
            var users = await SeedUsers();
            var groupRoles = await SeedPermissionWithGroupRoles();
            var exercise = await AddAsync(GetValidNorthwindSimpleExercise());
            var group1 = await AddAsync(new Group { Name = "Grupa1", CreatorId = 100 });
            var group2 = await AddAsync(new Group { Name = "Grupa2", CreatorId = 102 });
            var assignment1 = await AddAsync(new Assignment(users["user1"], group1.Id, groupRoles["Owner"]));
            var assignment2 = await AddAsync(new Assignment(user, group1.Id, groupRoles["User"]));
            var assignment3 = await AddAsync(new Assignment(user, group2.Id, groupRoles["User"]));

            var solving1 = await AddAsync(new Solving
            {
                AssignmentId = assignment2.Id,
                ExerciseId = exercise.Id,
                CreatorId = users["user1"],
                Status = SolvingStatusEnum.ToDo,
                DeadLine = deadline,
                SentAt = DateTime.UtcNow
            });
            var solving2 = await AddAsync(new Solving
            {
                AssignmentId = assignment3.Id,
                ExerciseId = exercise.Id,
                CreatorId = users["user1"],
                Status = SolvingStatusEnum.ToDo,
                DeadLine = deadline,
                SentAt = DateTime.UtcNow
            });
            return (exercise.Id, solving1.Id);
        }


        [Theory]
        [MemberData(nameof(GetSampleNorthwindSimpleQueries))]
        public async Task ForTwoQueriesAndNorthWindSimpleDb_ReturnsValidResult(QueryTest queryTest)
        {
            //Arrange
            await ClearNotNecesseryData();
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

            //Act
            var result = await SendAsync(new CreateSolutionCommand()
            {
                ExerciseId = exercise.Id,
                Query = queryTest.SolutionQuery
            });

            //Assert
            result.Result.Should().Be(queryTest.Result);
        }


        [Fact]
        public async Task ForUnallowedQuery_ThrowsSqlException()
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
                ValidAnswer = "SELECT * FROM Orders;"
            });

            //Act
            Func<Task> func = () => SendAsync(new CreateSolutionCommand()
            {
                ExerciseId = exercise.Id,
                Query = "INSERT INTO OrderItems VALUES (1, 11, 14.0, 10)",
            });

            //Assert
            await func.Should().ThrowAsync<SqlException>();
        }

        [Fact]
        public async Task ForValidData_CreatesValidAmountOfComparisonsAndSolvingData()
        {
            //Arrange
            var initResult = await InitForCreate(DateTime.UtcNow.AddDays(1));

            var command = new CreateSolutionCommand { Query = "SELECT * FROM Orders", ExerciseId = initResult.Item1 };

            //Act
            var result = await SendAsync(command);
            var comparisonCount = await CountAsync<Comparison>();
            var solvings = await WhereAsync<Solving>(x => x.SolutionId == result.SolutionId);

            //Assert
            comparisonCount.Should().Be(1);
            solvings.Count().Should().Be(2);

            foreach (var solving in solvings)
            {
                solving.Status.Should().Be(SolvingStatusEnum.Done);
                solving.SolutionId.Should().Be(result.SolutionId);
            }
        }

        [Fact]
        public async Task ForValidDataWithOverdueSentSolution_ReturnsOkWithSentButOverdueStatus()
        {
            //Arrange
            var initResult = await InitForCreate(DateTime.UtcNow.AddHours(-1));
            var command = new CreateSolutionCommand { Query = "SELECT * FROM Orders", ExerciseId = initResult.Item1 };

            //Act
            var result = await SendAsync(command);
            var comparisonCount = await CountAsync<Comparison>();
            var solving = await FindAsync<Solving>(initResult.Item2);

            //Assert
            comparisonCount.Should().Be(1);
            solving.Status.Should().Be(SolvingStatusEnum.DoneButOverdue);
            solving.SolutionId.Should().Be(result.SolutionId);
        }

        [Fact]
        public async Task ForInvalidModel_ThrowsValidationException()
        {
            //Arrange
            var exercise = await AddAsync(GetValidNorthwindSimpleExercise());

            var command = new CreateSolutionCommand
            {
                Query = "",
                ExerciseId = exercise.Id
            };

            //Act
            Func<Task<GetComparisonDto>> func = async () => await SendAsync(command);

            //Assert
            await func.Should().ThrowAsync<ValidationException>();
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
                    SolutionQuery = @"SELECT ProductId, COUNT(UnitPrice) COUNT FROM OrderItems
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
