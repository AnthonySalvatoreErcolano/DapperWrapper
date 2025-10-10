# DapperWrapper

A lightweight and extensible abstraction layer for [Dapper](https://github.com/DapperLib/Dapper) that simplifies database access, enforces consistent response handling, and provides a flexible query delegation model for clean separation of data and mapping logic.

---
## Table of Contents

- [Features](#features)
- [Architecture Overview](#architecture-overview)
- [Components Explained](#components-explained)
- [Quick Start](#quick-start)
  - [Define the Query Delegate](#define-the-query-delegate)
  - [Create the QueryService](#create-the-queryservice)
  - [Use the Service](#use-the-service)
  - [Update / Delete Example](#update--delete-example)
- [Configuration](#configuration)
  - [Register in DI](#register-in-di)
  - [Connection Factory Example](#connection-factory-example)
- [License](#license)
## Features

- Unified responses — `OperationResult` and `OperationCollectionResult` provide standardized success/failure handling.  
- Multi-mapping support — handle complex joins via `QueryAsync<T1, T2, TResult>` and similar overloads.  
- Delegate-based query builders — reusable, strongly typed SQL generation for flexible repository design.  
- Optional `QueryService` layer — encapsulate domain-specific logic cleanly on top of `DapperExecutor`.  
- DI-ready connection management — swap or configure database connections through `IDbConnectionFactory`.  
- Minimal overhead — retains all the performance and simplicity of Dapper.  

---

## Architecture Overview

```mermaid
flowchart TD
    A[Application / Repository]
    B[QueryService<br/>Defines delegates & maps DTOs → ViewModels]
    C[DapperExecutor<br/>Handles query execution, errors, responses]
    D[IDbConnectionFactory<br/>(e.g., SqlConnectionFactory)]
    E[Database]

    A --> B
    B --> C
    C --> D
    D --> E
```
## Components Explained

- **Application / Repository**  
  Calls `QueryService` methods and provides filters or delegates.

- **QueryService**  
  Accepts user-defined `QueryBuilder<TFilter>` delegates, optionally maps DTOs to ViewModels, and returns `OperationResult` or `OperationCollectionResult`.

- **DapperExecutor**  
  Executes SQL queries using Dapper, catches exceptions, and wraps results in standardized response objects.

- **IDbConnectionFactory**  
  Provides new `IDbConnection` instances for each operation, enabling DI-friendly and testable database access.

- **Database**  
  SQL Server (or any other DB) queried via the connection.

## Quick Start

### Define the Query Delegate

```csharp
public delegate (string Sql, DynamicParameters Params) QueryBuilder<TFilter>(TFilter filters);

public static class EmployeeQueries
{
    public static (string, DynamicParameters) GetEmployees(EmployeeFilter filter)
    {
        var sql = "SELECT * FROM Employees WHERE DepartmentId = @DeptId";
        var p = new DynamicParameters();
        p.Add("@DeptId", filter.DepartmentId);
        return (sql, p);
    }

    public static (string, DynamicParameters) InsertEmployee(EmployeeCreateModel model)
    {
        var sql = "INSERT INTO Employees (FirstName, LastName, DepartmentId) VALUES (@FirstName, @LastName, @DeptId)";
        var p = new DynamicParameters();
        p.Add("@FirstName", model.FirstName);
        p.Add("@LastName", model.LastName);
        p.Add("@DeptId", model.DepartmentId);
        return (sql, p);
    }
}
```
### Create the QueryService
```csharp
public class EmployeeQueryService : QueryService
{
    public EmployeeQueryService(DapperExecutor executor) : base(executor) { }

    public Task<OperationCollectionResult<EmployeeVM>> GetEmployees(EmployeeFilter filter)
        => Get<EmployeeDTO, EmployeeVM, EmployeeFilter>(
            EmployeeQueries.GetEmployees,
            filter,
            dto => new EmployeeVM(dto)
        );

    public Task<OperationResult> AddEmployee(EmployeeCreateModel model)
        => Execute(EmployeeQueries.InsertEmployee, model);
}
```

### Use the Service
```csharp
var service = new EmployeeQueryService(executor);

// Get employees
var result = await service.GetEmployees(new EmployeeFilter { DepartmentId = "HR" });

if (result.Value == ResponseValue.Success)
{
    foreach (var e in result.Data!)
        Console.WriteLine($"{e.Id}: {e.FirstName} {e.LastName}");
}
else if (result.Value == ResponseValue.NotFound)
{
    Console.WriteLine("No employees found in that department.");
}

// Insert employee
var insertResult = await service.AddEmployee(new EmployeeCreateModel
{
    FirstName = "John",
    LastName = "Doe",
    DepartmentId = "HR"
});

if (insertResult.Value == ResponseValue.Success)
{
    Console.WriteLine("Employee added successfully.");
}
else
{
    Console.WriteLine($"Error adding employee: {insertResult.ResponseText}");
}
```
### Update / Delete Example
```csharp
var updateResult = await service.Execute(EmployeeQueries.UpdateEmployee, updateModel);
if (updateResult.Value == ResponseValue.Success)
    Console.WriteLine("Updated successfully.");

// Delete example
var deleteResult = await service.Execute(EmployeeQueries.DeleteEmployee, deleteModel);
if (deleteResult.Value == ResponseValue.Success)
    Console.WriteLine("Deleted successfully.");

```

## Configuration
### Register in DI:
```csharp
services.AddSingleton<IDbConnectionFactory>(new SqlConnectionFactory(connectionString));
services.AddScoped<DapperExecutor>();
services.AddScoped<QueryService, EmployeeQueryService>();
```

### Connection Factory Example:
```csharp
public class SqlConnectionFactory : IDbConnectionFactory
{
    private readonly string _connectionString;

    public SqlConnectionFactory(string connectionString)
    {
        _connectionString = connectionString;
    }

    public IDbConnection CreateConnection()
        => new SqlConnection(_connectionString);
}
```

## License

This project is licensed under the **MIT License**.  
See the [LICENSE](LICENSE) file for details.
