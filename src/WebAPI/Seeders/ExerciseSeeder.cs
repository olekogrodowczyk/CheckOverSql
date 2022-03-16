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
                    IsPrivate = false,
                    Title = "Grouping",
                    Description = "For every OrderId in OrderItems table show amount of UnitPrices. " +
                    "For amount use \"Amount\" alias",
                    ValidAnswer = "SELECT OrderId, COUNT(UnitPrice) Amount FROM OrderItems GROUP BY OrderId;"
                },
                new Exercise()
                {
                    CreatorId = superUserId,
                    DatabaseId = databaseId,
                    IsPrivate = false,
                    Title = "Number of orders",
                    Description = "Show the number of orders of every customer. " +
                    "Customers data must be their Ids, FirstNames and LastNames, column with number of order " +
                    "call as NumberOfOrders",
                    ValidAnswer = @"SELECT c.Id, c.FirstName, c.LastName, COUNT(o.Id) NumberOfOrders FROM Orders o
                    INNER JOIN Customers c ON c.Id = o.CustomerId
                    GROUP BY c.Id, c.FirstName, c.LastName"
                },
                new Exercise()
                {
                    CreatorId = superUserId,
                    DatabaseId = databaseId,
                    IsPrivate = false,
                    Title = "The best customer",
                    Description = "Show Id, FirstName and LastName of the customer who " +
                    "made the most number of orders.",
                    ValidAnswer = @"SELECT c.Id, c.FirstName, c.LastName, COUNT(o.Id) NumberOfOrders FROM Orders o
                    INNER JOIN Customers c ON c.Id = o.CustomerId
                    GROUP BY c.Id, c.FirstName, c.LastName
                    HAVING COUNT(o.Id) = (SELECT MAX(NumberOfOrders) Max FROM (
                    SELECT c.Id, c.FirstName, c.LastName, COUNT(o.Id) NumberOfOrders FROM Orders o
                    INNER JOIN Customers c ON c.Id = o.CustomerId
                    GROUP BY c.Id, c.FirstName, c.LastName) QUERY)"
                },
                new Exercise()
                {
                    CreatorId = superUserId,
                    DatabaseId = databaseId,
                    IsPrivate = false,
                    Title = "The second best customer",
                    Description = "Show Id, FirstName and LastName of the customer who " +
                    "made the second-most number of orders.",
                    ValidAnswer = @"SELECT c.Id, c.FirstName, c.LastName, COUNT(o.Id) NumberOfOrders FROM Orders o
                    INNER JOIN Customers c ON c.Id = o.CustomerId
                    GROUP BY c.Id, c.FirstName, c.LastName HAVING COUNT(o.Id) = (
                    SELECT MAX(NumberOfOrders) Count FROM (
                    SELECT c.Id, c.FirstName, c.LastName, COUNT(o.Id) NumberOfOrders FROM Orders o
                    INNER JOIN Customers c ON c.Id = o.CustomerId
                    GROUP BY c.Id, c.FirstName, c.LastName
                    HAVING COUNT(o.Id) NOT IN (SELECT MAX(NumberOfOrders) Max FROM (
                    SELECT c.Id, c.FirstName, c.LastName, COUNT(o.Id) NumberOfOrders FROM Orders o
                    INNER JOIN Customers c ON c.Id = o.CustomerId
                    GROUP BY c.Id, c.FirstName, c.LastName) QUERY)) QUERY ) "
                },
                new Exercise()
                {
                    CreatorId = superUserId,
                    DatabaseId = databaseId,
                    IsPrivate = false,
                    Title = "Specific customers and their products",
                    Description = "For every customer with name beginning with " +
                    "\"L\" show their FirstName, LastName and number of products ordered ever" +
                    "(column name of products is \"NumberOfProducts\") ",
                    ValidAnswer = @"SELECT c.FirstName, c.LastName, COUNT(oi.Id) NumberOfProducts FROM OrderItems oi
                    INNER JOIN Orders o ON o.Id = oi.OrderId
                    INNER JOIN Customers c ON c.Id = o.CustomerId
                    WHERE FirstName LIKE 'L%'
                    GROUP BY c.FirstName, c.LastName
                    ORDER BY COUNT(oi.Id) DESC;"
                }
            };
        }

    }
}
