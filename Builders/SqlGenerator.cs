using Dapper;
using DapperWrapper.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DapperWrapper.Builders
{
    public static class SqlGenerator
    {
        public static (string Sql, DynamicParameters Parameters) GenerateInsert<T>(T model)
        {
            var type = typeof(T);
            var tableAttr = type.GetCustomAttribute<TableAttribute>();
            if (tableAttr == null) throw new InvalidOperationException($"{type.Name} is missing TableAttribute");

            var tableName = tableAttr.Name;
            var columns = new List<string>();
            var values = new List<string>();
            var parameters = new DynamicParameters();

            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var prop in properties)
            {
                var columnAttr = prop.GetCustomAttribute<ColumnAttribute>();
                if (columnAttr == null) continue;

                // Handle primary key generation
                if (columnAttr.IsPrimaryKey)
                {
                    switch (columnAttr.KeyGeneration)
                    {
                        case KeyGeneration.AutoIncrement:
                            // Skip auto-increment columns
                            continue;

                        case KeyGeneration.GenerateGuid:
                            var val = prop.GetValue(model);
                            if (val == null || (val is Guid g && g == Guid.Empty))
                            {
                                var newGuid = Guid.NewGuid();
                                prop.SetValue(model, newGuid);
                                parameters.Add(prop.Name, newGuid);
                            }
                            else
                            {
                                parameters.Add(prop.Name, val);
                            }
                            break;

                        case KeyGeneration.External:
                            parameters.Add(prop.Name, prop.GetValue(model));
                            break;
                    }
                }
                else
                {
                    parameters.Add(prop.Name, prop.GetValue(model));
                }

                columns.Add(columnAttr.Name ?? prop.Name);
                values.Add("@" + prop.Name);
            }

            if (!columns.Any()) throw new InvalidOperationException("No columns found to insert");

            var sql = $"INSERT INTO {tableName} ({string.Join(", ", columns)}) VALUES ({string.Join(", ", values)})";
            return (sql, parameters);
        }

        public static (string Sql, DynamicParameters Parameters) GenerateUpdate<T>(T model)
        {
            var type = typeof(T);
            var tableAttr = type.GetCustomAttribute<TableAttribute>();
            if (tableAttr == null) throw new InvalidOperationException($"{type.Name} is missing TableAttribute");

            var tableName = tableAttr.Name;
            var setClauses = new List<string>();
            var whereClauses = new List<string>();
            var parameters = new DynamicParameters();

            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var prop in properties)
            {
                var columnAttr = prop.GetCustomAttribute<ColumnAttribute>();
                if (columnAttr == null) continue;

                var colName = columnAttr.Name ?? prop.Name;
                var val = prop.GetValue(model);

                if (columnAttr.IsPrimaryKey)
                {
                    whereClauses.Add($"{colName} = @{prop.Name}");
                    parameters.Add(prop.Name, val);
                }
                else
                {
                    setClauses.Add($"{colName} = @{prop.Name}");
                    parameters.Add(prop.Name, val);
                }
            }

            if (!setClauses.Any()) throw new InvalidOperationException("No columns found to update");
            if (!whereClauses.Any()) throw new InvalidOperationException("No primary key defined for update");

            var sql = $"UPDATE {tableName} SET {string.Join(", ", setClauses)} WHERE {string.Join(" AND ", whereClauses)}";
            return (sql, parameters);
        }

        public static (string Sql, DynamicParameters Parameters) GenerateDelete<T>(T model)
        {
            var type = typeof(T);
            var tableAttr = type.GetCustomAttribute<TableAttribute>();
            if (tableAttr == null) throw new InvalidOperationException($"{type.Name} is missing TableAttribute");

            var tableName = tableAttr.Name;
            var whereClauses = new List<string>();
            var parameters = new DynamicParameters();

            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var prop in properties)
            {
                var columnAttr = prop.GetCustomAttribute<ColumnAttribute>();
                if (columnAttr == null) continue;
                if (!columnAttr.IsPrimaryKey) continue;

                var colName = columnAttr.Name ?? prop.Name;
                whereClauses.Add($"{colName} = @{prop.Name}");
                parameters.Add(prop.Name, prop.GetValue(model));
            }

            if (!whereClauses.Any()) throw new InvalidOperationException("No primary key defined for delete");

            var sql = $"DELETE FROM {tableName} WHERE {string.Join(" AND ", whereClauses)}";
            return (sql, parameters);
        }
    }
}

