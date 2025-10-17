# DapperWrapper

A lightweight and extensible abstraction layer for [Dapper](https://github.com/DapperLib/Dapper) that simplifies database access, enforces consistent response handling, and provides a flexible query delegation model for clean separation of data and mapping logic.

---
## Table of Contents

- [Features](#features)
- [How It Works](#how-it-works)
- [Quick Start](#quick-start)
  - [Register in DI](#register-in-di)
  - [Define the Query Delegate](#define-the-query-delegate)
  - [Create Repositories](#create-repositories)
  - [Use the Service](#use-the-service)
  - [Update / Delete Example](#update--delete-example)
- [Configuration](#configuration)
  - [Connection Factory Example](#connection-factory-example)
- [License](#license)
## Features

- Unified responses — `OperationResult` and `OperationCollectionResult` provide standardized success/failure handling.  
- Defines an `Executor` service that  wraps Dappers core functionality and returns unified responses `OperationResult` and `OperationCollectionResult` and standardized success/failure handling.
- Multi-mapping support — handle complex joins via `QueryAsync<T1, T2, TResult>` and similar overloads.  
- Delegate-based query builders — reusable, strongly typed SQL generation for flexible repository design.  
- Optional `QueryService` layer which wraps `Executor`, unifying standard querying commands and mappings.
- Optional `CommandService` layer which also wraps `Executor` which implements automatic generation of insert/update/delete SQL commands based on the model.
- DI-ready connection management — swap or configure database connections through `IDbConnectionFactory`.  
- Minimal overhead — retains all the performance and simplicity of Dapper.  

---
## How It Works

- **Overview**
 Creates a small wrapper around Dappers base functionality to allow for easier setup when working with the database. The first layer, the `Executor` directly enhances Dappers functionality by implementing standardized return types and status codes for handling different errors. Ontop of the `Executor` there are two services, `DapperQueryService` and `DapperCommandService`; each uses the `Executor` to further extend querying and the other CRUD operations. `DapperQueryService` contains `Get` methods that consume the query delegates and allow for mapping (like a DTO to a view model). `DapperCommandService` directly wraps the `Executor` with no new functionality as of yet, the `Executor` use the automatic command builder `SqlGenerator` to consume the model and based on the attributes `ColumneAttribute` and `TableAttribute` creates a sql command and the dynamic parameters.  

- **Repository**
   Calls `QueryService` or `CommandService` methods and provides filters or delegates.
- **QueryService**  
  Accepts user-defined `QueryBuilder<TFilter>` delegates, optionally maps DTOs to ViewModels, and returns `OperationResult` or `OperationCollectionResult`.

- **QueryService**  
  Handles non-query operations (insert/update/delete) with standardized `OperationResult`. Uses the automatic command builder `SqlGenerator`

- **Executor**  
  Executes SQL queries/commands using Dapper, catches exceptions, and wraps results in standardized response objects.

- **IDbConnectionFactory**  
  Provides new `IDbConnection` instances for each operation, enabling DI-friendly and testable database access.

## Quick Start

### Register in DI

```csharp
    services.AddSingleton<IDbConnectionFactory>(new SqlConnectionFactory(connectionString));
    services.AddScoped<Executor>();
    services.AddScoped<QueryService, EmployeeQueryService>();
    services.AddScoped<CommandService, EmployeeCommandService>();
```

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
### Create Repositories
```csharp
public class EmployeeRepository
{
    private readonly DapperQueryService _query;
    private readonly DapperCommandService _command;

    public EmployeeQueryService(DapperQueryService query, DapperCommandService command) 
    { 
        _query=query;
        _command=command;
    }

    public async Task<TransactionCollectionResponse<TResult>> GetByEmployeeId<TResult>(string employeeId, Func<EmployeeDTO, TResult>? map = null) => await executor.Get<EmployeeDTO, TResult, EmployeeFilters>(GetEmployeedsById, new EmployeeFilters { EmployeeId = employeeId }, map);

    public Task<OperationResult> CreateEmployee(Employee employee)=> _command.InsertAsync(employee);
}
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
