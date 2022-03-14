# CheckOverSql
CheckOverSql is an application created in purpose to learn SQL by solving exercises checked automatically by an algorithm.
The goal of the project is to make training SQL easier and more fun by the exercises and testing queries.
The project is also extended by the possibility to create groups and assign tasks for other users. This solution includes also permission-based authorization.<br /><br />
*Still in development*

<br />

## Technologies
- ASP.NET Core 5
- Entity Framework Core 5
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
10. Insert the database and readonly user data into the Databases table in the application database  
11. With Command Prompt navigate into src/WebAPI/ClientApp and run ```npm install```
12. Navigate to src/WebAPI/ClientApp and run ```npm start``` to launch an Angular app.
13. Navigate to src/WebAPI and run dotnet run to launch the ASP.NET Core app.

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

## How the application is secured against unallowed queries?
Checking the query on C# level for keywords like ```GRANT``` or ```INSERT``` can be bypassed easily and is not secured properly.
The best solution there is to add a user into exercises based database with db_readonly role and keep it secured on a database level.
In this way, user can only execute ```SELECT``` queries.

## How is solving exercise alone connected with solving exercises in groups?
It would be weird and inefficient when you solved some exercise, got assigned the same exercise in some group and had to do it again.
The application works in a better way. Every time you solve an exercise, an algorithm will check if you have some pending tasks assigned to you
with this exercise so the assigned task will be solved immediately.

## How the checking algorithm works?
In the project there are two implementations for the algorithm. You can just change a one line and implementation will be different.
QueryEvaluatorDriverNaive.cs file contains a naive implementation, this way of checking exercises is slow and not recommended. However, it may be useful in checking 
the small amount of rows because of its accuracy.
QueryEvaluatorDriverOptimized.cs file contains an optimized implementation of checking two queries. in contrast to naive approach it works on a database level, 
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
are converted into one. It's the first and most obvious step and there is no need check anything in a database.
### Second phase
The second phase is to check column names and compare them via this code:
```
using (SqlDataReader reader = await command.ExecuteReaderAsync(CommandBehavior.SchemaOnly))
{
    dataTable = await reader.GetSchemaTableAsync();
}
```
Columns can be obtained easily without getting other query data.
Next, comparing is even easier with .SequenceEqual method.
### Third phase
The next step is to compare the count of rows of two queries.
The example of this was shown in a QueryBuilder section.
### Fourth phase
Probably the most important phase is where the algorithm compares values. In this case the key is an Intersect keyword in an SQL.
Intersect will show you how many rows are common among two queries. In practice it is checking the count of rows of the first query and second query as it in third phase.
Comparing them, if they're not equal, return false, if they're equal, the algorithm checks the the count of the intersection of them. There is no need for getting all the common 
data of the two queries. We only need the count of rows and compare it with either the amount of rows of the first or second query. If they're not equal, then return false what means
query is invalid.
### Fifth phase (optional)
This step is optional and that comes from getting only three rows from the database: first, middle and last and comparing them. It can be rather useful when checking up
the order by clauses.
### How the algorithm handles order by?
Unfortunately, subqueries don't work with order by and it needs to be treated with a special approach.
If the query contains "ORDER BY" clause, we have to add "OFFSET 0 ROW" at the end of the query, by this, query can be executed.
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
![exercises](https://user-images.githubusercontent.com/15310742/158085998-963931a0-12a7-4039-aef9-dabcb3c55502.PNG)

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
