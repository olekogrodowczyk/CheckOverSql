using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAPI.Seeders
{
    public static class ExerciseSeeder
    {
        public static IEnumerable<Exercise> GetNorthwindSimplePublicExercises(int superUserId, int databaseId)
        {
            return new List<Exercise>()
            {
                new Exercise()
                {
                    CreatorId = superUserId,
                    DatabaseId = databaseId,
                    IsPrivate = false,
                    Title = "Get all data from table Orders",
                    Description = " Get all the data from table orders without any aliases",
                    ValidAnswer = "SELECT * FROM Orders"
                },
                new Exercise()
                {
                    CreatorId = superUserId,
                    DatabaseId = databaseId,
                    IsPrivate = false,
                    Title = "Get all customers with valid data",
                    Description = "Get all customers with only Id, FirstName and LastName columns "+
                    "with following aliases: id, first, last",
                    ValidAnswer = "SELECT Id id, firstName first, lastName last FROM Customers"
                },
                new Exercise()
                {
                    CreatorId=superUserId,
                    DatabaseId = databaseId,
                    IsPrivate=false,
                    Title="Get all data from table Products sorted",
                    Description = "Get all data from table Products sorted by firstly ProductName "+
                    "and secondly by UnitPrice all in descending type",
                    ValidAnswer = "SELECT Id id, firstName first, lastName last FROM Customers"
                },
                new Exercise()
                {
                    CreatorId = superUserId,
                    DatabaseId = databaseId,
                    IsPrivate = false,
                    Title = "Get data with JOINS",
                    Description = "Get Orders.Id as OrderId, OrderItems.UnitPrice as UnitPriceOrderItem " +
                    "Products.Package without alias with the appropriate JOINS and sort the data by Products.UnitPrice",
                    ValidAnswer = @"Select ord.Id OrderId, oi.UnitPrice UnitPriceOrderItem, p.Package from OrderItems oi
                    INNER JOIN Orders ord ON ord.Id = oi.OrderId
                    INNER JOIN Products p ON p.Id = oi.ProductId
                    ORDER BY p.UnitPrice DESC;"
                },
                new Exercise()
                {
                    CreatorId = superUserId,
                    DatabaseId = databaseId,
                    IsPrivate =false,
                    Title = "Grouping",
                    Description = "For every OrderId in OrderItems table show amount of UnitPrices. " +
                    "For amount use \"Amount\" alias",
                    ValidAnswer = "SELECT OrderId, COUNT(UnitPrice) Amount FROM OrderItems GROUP BY OrderId;"
                },
            };
        }

    }
}
