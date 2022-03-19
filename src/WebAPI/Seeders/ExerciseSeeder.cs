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
                },
                new Exercise()
                {
                    CreatorId = superUserId,
                    DatabaseId = databaseId,
                    IsPrivate = false,
                    Title = "Cities' delivers",
                    Description = "For each supplier's city show how many products the city delivered. " +
                    "The column of amount should be called as \"NumberOfProducts\". Sort results by the amounts " +
                    "in ascending order.",
                    ValidAnswer = @"SELECT s.City, COUNT(p.Id) NumberOfProducts FROM Suppliers s
                    INNER JOIN Products p ON p.SupplierId = s.Id
                    GROUP BY s.City
                    ORDER BY Count(p.Id) ASC;"
                },
                new Exercise()
                {
                    CreatorId = superUserId,
                    DatabaseId = databaseId,
                    IsPrivate = false,
                    Title = "Delivers of Paris",
                    Description = "Show all delivers of products from Paris (City, ProductName, UnitPrice)",
                    ValidAnswer = @"SELECT s.City, p.ProductName, p.UnitPrice FROM Suppliers s
                    INNER JOIN Products p ON p.SupplierId = s.Id
                    WHERE s.City = 'Paris'"
                },
                new Exercise()
                {
                    CreatorId = superUserId,
                    DatabaseId = databaseId,
                    IsPrivate = false,
                    Title = "Sum of orders amounts",
                    Description = "Show all delivers of products from Paris (City, ProductName, UnitPrice)",
                    ValidAnswer = @"SELECT s.City, p.ProductName, p.UnitPrice FROM Suppliers s
                    INNER JOIN Products p ON p.SupplierId = s.Id
                    WHERE s.City = 'Paris'"
                },
                new Exercise()
                {
                    CreatorId = superUserId,
                    DatabaseId = databaseId,
                    IsPrivate = false,
                    Title = "Best orders",
                    Description = "For every customer show their order with the biggest number of items" +
                    " (FirstName, LastName, OrderDate, Quantity). Remember to include all the items of the order",
                    ValidAnswer = @"SELECT FirstName, LastName, Date AS OrderDate, Quantity FROM (
                    SELECT c.Id, c.FirstName, c.LastName, o.OrderDate Date, SUM(oi.Quantity) Quantity from Customers c
                    INNER JOIN Orders o ON o.CustomerId = c.Id
                    INNER JOIN OrderItems oi ON oi.OrderId = o.Id
                    GROUP BY c.Id, c.FirstName, c.LastName, o.OrderDate
                    HAVING SUM(oi.Quantity) =
                    (
                    SELECT MAX(Quantity) FROM (
                    SELECT c2.Id, c2.FirstName, c2.LastName, o2.OrderDate, SUM(oi2.Quantity) Quantity from Customers c2
                    INNER JOIN Orders o2 ON o2.CustomerId = c2.Id
                    INNER JOIN OrderItems oi2 ON oi2.OrderId = o2.Id
                    GROUP BY c2.Id, c2.FirstName, c2.LastName, o2.OrderDate
                    ) QUERY
                    WHERE QUERY.Id = c.Id
                    )) QUERY2"
                },
                new Exercise()
                {
                    CreatorId = superUserId,
                    DatabaseId = databaseId,
                    IsPrivate = false,
                    Title = "TotalAmounts",
                    Description = "Show sum of total amounts of all orders, column should be named as \"TotalAmounts\"",
                    ValidAnswer="SELECT SUM(TotalAmount) AS TotalAmounts FROM Orders"
                },
                new Exercise()
                {
                    CreatorId = superUserId,
                    DatabaseId = databaseId,
                    IsPrivate = false,
                    Title = "Most expensive products",
                    Description = "For every country of suppliers show the product with" +
                    " the biggest UnitPrice (Coutry, ProductName, UnitPrice)",
                    ValidAnswer = @"SELECT s.Country, p.ProductName, p.UnitPrice FROM Suppliers s
                    INNER JOIN Products p on p.SupplierId = s.Id
                    WHERE p.UnitPrice = (
                    SELECT MAX(UnitPrice) FROM (
                    SELECT s2.Country, MAX(p2.UnitPrice) UnitPrice FROM Suppliers s2
                    INNER JOIN Products p2 on p2.SupplierId = s2.Id 
                    GROUP BY s2.Country) QUERY
                    WHERE Query.Country = s.Country
                    )"
                },
            };
        }

    }
}
