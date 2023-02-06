# Upwork.TaskService

## Database initialization
Use initialization script **Upwork.TaskService.Database/Upwork.TaskService.Database.publish.sql**.

It was generated by Visual Studio and can be easly apply to database using SQL Server Management Studio and enabled **SQLCMD** mode. To enable this mode click the 'SQLCMD Mode' option under the 'Query' menu.

The script will create tables, stored procudures and funtions, and automatically populate missed data (ex.: list of holidays, dictionaries of priorities and statuses).

*Option: If you have trouble running this script using SQLCMD, simply create the database manually and apply all sql commands bellow of command 'USE [$(DatabaseName)]' on it.*

## Database configuration
Check and correct the connection strings in the following configuration files:
* Upwork.TaskService.Api/appsettings.json
* Upwork.TaskService.Tests/appsettings.Test.json

Set values for **ConnectionStrings.TaskServiceDb** key:
```
"ConnectionStrings": {
    "TaskServiceDb": "Server=(LocalDb)\\MSSQLLocalDB;Database=TaskServiceDb;Trusted_Connection=True;MultipleActiveResultSets=true"
}
```

## Run of web API application
1. Open *Upwork.TaskService.sln* using Visual Studio
2. Select *Upwork.TaskService.Api* project
3. Click *Build | Build Solution* to build the project
4. Press *F5* to run the project
5. Visual Studio wil open url *https://localhost:7097/swagger/index.html* in browser
6. Run API requests manually using Swagger UI

## Run unit tests
1. Open *Upwork.TaskService.sln* using Visual Studio
2. Select *Upwork.TaskService.Tests* project
3. Click 'Run Tests' option from context menu
3. Open '*Test Explorer*' to see the tests running progress (Visual Studio: menu Test → Test Explorer)

## Tools and libraries
* Visual Studio Community 2022 v17.4.4
* SQL Server Management Studio v15.0.18206.0
* SQL Server 2019 v15.0.4153.1
* Asp.Net Core 6
* .NET 6
* NUnit v3.13.3
* FluentValidation v11.4.0
* AutoMapper v12.0.1

## To do
* Optimaze population of tblHoliday table
