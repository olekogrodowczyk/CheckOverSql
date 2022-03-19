<img align="center" width="150" height="81" src="https://user-images.githubusercontent.com/15310742/158210434-25cd8e54-1903-449e-a57b-611929ba405b.png" />

# CheckOverSql
CheckOverSql is an application created in purpose to learn SQL by solving exercises checked automatically by an algorithm.
The goal of the project is to make training SQL easier and more fun by the exercises and testing queries.
The project is also extended by the possibility to create groups and assign tasks for other users. This solution includes also permission-based authorization.<br /><br />
*Still in development*

## Technologies
- ASP.NET Core 5
- Entity Framework Core 5
- SQL Server
- Angular 13
- Angular Material 13
- MediatR
- AutoMapper
- FluentValidation
- XUnit, FluentAssertions, Moq
- NSwagStudio

## Getting started
1. Install the latest [.NET 5 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/5.0)
2. Install the latest [Node.js LTS](https://nodejs.org/en/)
3. Install the latest [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)
4. Install the [SSMS](https://docs.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms?view=sql-server-ver15)
5. Download the database [NorthwindSimple](https://www.dofactory.com/sql/sample-database) 
6. Change all the names of tables into plural (Order table causes some problems with "ORDER BY" clause)
7. Create a new database in SQL Server Management studio and execute the tables creation queries there
8. Create a new login in your SQL Server instance 
9. Click twice at the login, click User Mapping and add the login into your NorthwindSimple database with db_readonly role
10. Clone the project
11. In ```src/Seeder/Data/DatabasesSeederData.cs``` specify your own data of databases
12. With Command Prompt navigate into src/WebAPI/ClientApp and run ```npm install```
13. Navigate to src/WebAPI/ClientApp and run ```npm start``` to launch an Angular app.
14. Navigate to src/WebAPI and run ```dotnet run``` to launch the ASP.NET Core app.

## Features
### Queries
- Executing your own query into the selected database directly in the application up to 1000 rows
- Executed queries include specified aliases and NULL values
- While executing an invalid query you get a message from the database what's wrong.
- History of executed queries and executing them without rewriting
- Lazy pagination of executed queries

### Exercises
- Creating your own exercises and choose if they're public or private
- Automatically seeded initial public exercises (To be continued)
- Solving an exercise
- Getting the last answer executed in the purpose of passing the exercise (Or valid answer if the exercise is already passed)
- Assigning the exercise into a group for users to do (The button shows dynamically when you have appropriate permission in at least one group)

### Groups
- Creating a group with the name and picture
- Creating an invitation to a group by specifying an email and group role 

### Tasks
- Getting tasks with specified statuses and solving them
- Getting tasks to check (Button will show only if you're capable of checking exercises an at least one group)
- Check tasks solved by other users in groups where you have an appropriate group role by specifying points for it

### Features to add
- Allow users to add their own databases into the application
- Deleting users from groups and groups
- Changing roles of users
- Changing a name and a picture of a group
- Add more control of assigning exercises like: Not needed to check by a person (Only for the algorithm) and max points for that automatically
- Add badges into links to show pending tasks, invitations etc.
- Implement notification system via WebSockets.

## How is the application secured against unallowed queries?
Checking the query on C# level for keywords like ```GRANT``` or ```INSERT``` can be bypassed easily and is not secured properly.
The best solution there is to add a user into exercises based database with db_readonly role and keep it secured on a database level.
In this way, user can only execute ```SELECT``` queries.

## How is solving exercise alone connected with solving exercises in groups?
It would be weird and inefficient when you solved some exercise, got assigned the same exercise in some group and had to do it again.
The application works in a better way. Every time you solve an exercise, an algorithm will check if you have some pending tasks assigned to you
with this exercise so the assigned task will be solved immediately.

## How the checking algorithm works?
In the project there are two implementations for the algorithm. You can just change a one line and implementation will be different.
```QueryEvaluatorDriverNaive.cs``` file contains a naive implementation, this way of checking exercises is slow and not recommended. However, it may be useful in checking  the small amount of rows because of its accuracy.
```QueryEvaluatorDriverOptimized.cs``` file contains an optimized implementation of checking two queries. in contrast to naive approach it works on a database level, 
not the application one. <br />
### QueryBuilder
Firstly, the query is properly built by QueryBuilder designed as a builder design pattern. Mostly it is wrapped into a subquery, for example function there:
```
public QueryBuilder AddCount()
{
    _query = $"SELECT COUNT(*) AS COUNT FROM ({_query}) QUERY";
    return this;
}
```
will wrap query: ```SELECT * FROM Orders```  INTO  ```"SELECT COUNT(*) FROM ( SELECT * FROM Orders ) QUERY``` and by that we can get amount of rows of the query.
### FirstPhase
The first phase is to check if the two queries are the same so a query is trimmed for spaces and semicolons and all multiple spaces
are converted into one. It's the first and most obvious step and there is no need check anything in a database. <br />
The code: <br />
```
public async Task Handle(QueryEvaluationData data)
{
    data.Phase = QueryEvaluationPhase.Bodies;
    string queryToCheck1 = _queryBuilder.SetInitQuery(data.Query1).HandleSpaces().GetResult();
    string queryToCheck2 = _queryBuilder.SetInitQuery(data.Query2).HandleSpaces().GetResult();
    bool result = queryToCheck1.Equals(queryToCheck2, StringComparison.OrdinalIgnoreCase);
    if (result) { data.Stop = true; data.FinalResult = true; }
    await Task.CompletedTask;
}
```
### Second phase
The second phase is to check column names and compare them via this code:
```
using (SqlDataReader reader = await command.ExecuteReaderAsync(CommandBehavior.SchemaOnly))
{
    dataTable = await reader.GetSchemaTableAsync();
}
```
Columns can be obtained easily without getting other query data.
Next, comparing is even easier with .SequenceEqual method. <br />
The comparison code: <br />
```
public async Task Handle(QueryEvaluationData data)
{
    data.Phase = QueryEvaluationPhase.Columns;
    bool result = await _queryEvaluatorService.CompareColumnNames(data.Query1, data.Query2);
    if (!result) { data.FinalResult = false; data.Stop = true; }
}
```

### Third phase
The next step is to compare the count of rows of two queries.
The example of this was shown in a QueryBuilder section. <br />
The code: <br />
```
public async Task Handle(QueryEvaluationData data)
{
    data.Phase = QueryEvaluationPhase.QueriesCounts;
    data.Query1Count = await _queryEvaluatorService.GetCountOfQuery(data.Query1);
    data.Query2Count = await _queryEvaluatorService.GetCountOfQuery(data.Query2);
    if (data.Query1Count != data.Query2Count) { data.Stop = true; data.FinalResult = false; }
}
```
### Fourth phase 
This step is easy and that comes from getting only three rows from the database: first, middle and last and comparing them. It can be rather useful when checking up the order by clauses. Instead of comparing all rows one by one, the algorithm will only get three of them and compare the results. There's a good chance to catch inequalities without using intersect which is represent in the next phase. <br />
The code: <br />
```
public async Task Handle(QueryEvaluationData data)
{
    data.Phase = QueryEvaluationPhase.FirstMiddleLastRows;
    if (data.Query1Count < 1 || data.Query2Count < 1) { data.Stop = true; return; }
    if (data.Query1Count is null || data.Query2Count is null) { data.Stop = true; return; }

    var matrix1 = await _queryEvaluatorService.GetFirstMiddleLastRows(data.Query1, (int)data.Query1Count);
    var matrix2 = await _queryEvaluatorService.GetFirstMiddleLastRows(data.Query2, (int)data.Query2Count);
    bool result = await _dataComparerService.compareValues(matrix1, matrix2);

    if (!result) { data.Stop = true; data.FinalResult = false; }
}
```
### Fifth phase
This phase is last phase because of checking all the values in the results. It requires the most of calculations that's why it is at the end. <br />
Eveything is happening on the database level which doesn't couse a memory problem by allocating the data by C# like it is in a naive implementation. <br />
Intersecting two queries returns the same rows collected from two queries. If the two queries results are the same then the intersected one query number of rows
will be the same as it is in both the first and the second query. <br />
The code: <br />
```
public async Task Handle(QueryEvaluationData data)
{
    data.Phase = QueryEvaluationPhase.IntersectedCount;
    data.IntersectCount = await _queryEvaluatorService.GetIntersectQueryCount(data.Query1, data.Query2);
    if (data.Query1Count != data.IntersectCount) { data.FinalResult = false; }
    else { data.FinalResult = true; }
}
```

### How the chaining of phases works?
Implementing typical chain of responsibility design pattern is hard due to dependency injection but in this case a dependency injection is highly efficient.
By the shared interface:
```
public interface IEvaluationHandler
{
    Task Handle(QueryEvaluationData data);
}
```
There is possibility to make as many implementations as it is needed.
```
//Order of below injections matters
services.AddScoped<IEvaluationHandler, BodiesHandler>();
services.AddScoped<IEvaluationHandler, ColumnsHandler>();
services.AddScoped<IEvaluationHandler, CountsHandler>();
services.AddScoped<IEvaluationHandler, FirstMiddleLastRowsHandler>();
services.AddScoped<IEvaluationHandler, IntersectHandler>();
```
Next, the injection is following:
```
public class QueryEvaluatorDriverOptimized : IQueryEvaluatorDriver
{
   private readonly IEnumerable<IEvaluationHandler> _handlers;
   public QueryEvaluatorDriverOptimized(IEnumerable<IEvaluationHandler> handlers)
   {
      _handlers = handlers;
   }
}
```
Then handlers are just being iterated over and the result is being logged.
The data object contains data about query evaluation like: queries, userId, phase or queries counts. 
```
foreach (var handler in _handlers)
{
    await handler.Handle(data);
    if (data.Stop) { break; }
}
_queryEvaluationLogging.Log(data);
```
            
### How the algorithm handles order by?
Unfortunately, subqueries don't work with order by and it needs to be treated with a special approach.
If the query contains ```ORDER BY``` clause, we have to add ```OFFSET 0 ROW``` at the end of the query, by this, query can be executed.
### How the algorithm handles duplicates?
Also, unfortunately, intersect doesn't include duplicates, so outcomes can be different.
Because of that we are forced to wrap a query with this code:
```
public QueryBuilder AddRowNumberColumn()
{
    _query = "SELECT * FROM(SELECT *, ROW_NUMBER() OVER(ORDER BY(SELECT NULL)) AS RowNumber" +
        $" FROM ({_query}) QUERY ) AS MyTable";
    return this;
}
```
In this way the query result gains additional unique row number column so no more duplicates.
### How the algorithm will be handling executing queries with prepared data? (To implement)
For now there's a possibility to execute query with well prepared data earlier and pass the exercise. <br />
For example count manually rows with orders of some customer and send solution: ```"SELECT 'Martin', 'Smith', 5"``` <br />
Of course in groups there's also a checking section where checker can see sent solution 
and purpose of self learning is rather not to cheat. <br />
However, the application will be equipped with a better way of checking exercises 
and it's all about an additional database hidden for users where result will be different so in this way it will work like unit tests.

### Example
Let's take these two queries:
```
Select ord.Id OrderId, oi.UnitPrice UnitPriceOrderItem, p.Package from OrderItems oi                   
INNER JOIN Orders ord ON ord.Id = oi.OrderId
INNER JOIN Products p ON p.Id = oi.ProductId          
ORDER BY p.UnitPrice ASC;
```
AND
```
Select ord.Id OrderId, oi.UnitPrice UnitPriceOrderItem, p.Package from OrderItems oi
INNER JOIN Orders ord ON ord.Id = oi.OrderId                
INNER JOIN Products p ON p.Id = oi.ProductId                  
ORDER BY p.UnitPrice DESC;
```
They shouldn't be the same because of different sorting. <br />
Firstly, the algorithm will check if the bodies are the same and will keep checking. <br />
Secondly, the algorithm will check columns and they're the same so also will keep checking. <br />
Thirdly, the algorithm will check only number of columns of these two queries and result is 2155 and 2155 so it will keep checking. <br />
Fourthly, the algorithm will check the first, middle and last row of two queries and compare them. 
In this case just the first row of the first query differs from the first row of the second query so algorithm is done here.
Fifthly, if the previous step don't catch any inequalities of rows the algorithm will check values and make an intersection of these two queries so the following query will be created and executed: <br />
```
SELECT COUNT(*) AS COUNT FROM (
SELECT * FROM (SELECT *, ROW_NUMBER() OVER(ORDER BY(SELECT NULL)) AS RowNumber FROM 
(Select ord.Id OrderId, oi.UnitPrice UnitPriceOrderItem, p.Package from OrderItems oi 
INNER JOIN Orders ord ON ord.Id = oi.OrderId                  
INNER JOIN Products p ON p.Id = oi.ProductId                  
ORDER BY p.UnitPrice ASC OFFSET 0 ROW) QUERY ) AS MyTable 
INTERSECT 
SELECT * FROM (SELECT *, ROW_NUMBER() OVER(ORDER BY(SELECT NULL)) AS RowNumber FROM 
(Select ord.Id OrderId, oi.UnitPrice UnitPriceOrderItem, p.Package from OrderItems oi                   
INNER JOIN Orders ord ON ord.Id = oi.OrderId               
INNER JOIN Products p ON p.Id = oi.ProductId             
ORDER BY p.UnitPrice DESC OFFSET 0 ROW) QUERY ) AS MyTable
) QUERY
```
There are the query results (the subquery of the whole count): <br />
![dane](https://user-images.githubusercontent.com/15310742/158289976-04408092-6708-4270-8c2a-87bf94969011.PNG)

What means there are only two common rows and the rest is different.
Obviously, the algorithm will only get the count of this query and compare it with query1 count.
```2155 != 2``` So the queries cannot be the same. 



## Database Schema
![DBMS ER diagram (UML notation) (1)](https://user-images.githubusercontent.com/15310742/158085866-68029818-715d-4425-ab54-20dd2275f486.png)


## Screenshots

### Query History
![queries](https://user-images.githubusercontent.com/15310742/158085905-9c33af3f-5062-46df-9766-37c45bcc5cd7.PNG)

### Execute Query
![executequery](https://user-images.githubusercontent.com/15310742/158085938-66fe61bc-b453-42e8-86b8-f0969f8967fc.PNG)

### Executing Query Result
![queryresult](https://user-images.githubusercontent.com/15310742/158085968-b572925f-13ce-4e0c-9a16-cbcf0baeea11.PNG)

### Exercises
![exercises](https://user-images.githubusercontent.com/15310742/159134092-c131bb60-ac71-46fc-9622-9aa34ca0df05.png)

### Create Exercise
![createexercise](https://user-images.githubusercontent.com/15310742/158086017-e1b0a12d-fb60-4e9c-94e9-a36ff97e5db2.PNG)

### Assign Exercise For Group
![assignexercise](https://user-images.githubusercontent.com/15310742/158086172-4d37fe48-031a-4f63-8272-e67be85fafb4.png)

### Groups
![groups](https://user-images.githubusercontent.com/15310742/158086032-f9827904-d13a-4396-b8bd-3d36010b19a7.PNG)

### Create Group
![creategroup](https://user-images.githubusercontent.com/15310742/158086049-075060cd-8b5f-49f9-a588-21e62dca0e40.png)

### Group
![group](https://user-images.githubusercontent.com/15310742/158086191-638fd488-9f65-4edb-9344-628803f22944.png)

### Send Invitation
![sendinvitation](https://user-images.githubusercontent.com/15310742/158086246-67bd003b-2a9b-4197-8adc-edaf6fa9f0ab.png)

### Invitations
![invitations](https://user-images.githubusercontent.com/15310742/158086257-03f85c2c-90d0-4978-a073-d12b147b764a.png)

### Tasks
![Tasks](https://user-images.githubusercontent.com/15310742/158086287-0602beba-f5ba-4488-b81d-68cf8560ad21.png)

### Last Answer
![lastanaswer](https://user-images.githubusercontent.com/15310742/158086315-9935dd89-b4ab-4081-b181-f1ebd87e2a0e.png)

### Check Task
![checktask](https://user-images.githubusercontent.com/15310742/158086322-f7556bc0-2187-4f50-956a-d6bc6b1b4b84.png)
