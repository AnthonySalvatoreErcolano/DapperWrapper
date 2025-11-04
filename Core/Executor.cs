using Dapper;
using DapperWrapper.Builders;
using DapperWrapper.Models;
using System.Data;

namespace DapperWrapper.Core
{
    public class Executor
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public Executor(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        // Generic simple query
        public Task<OperationCollectionResult<T>> ExecuteQueryAsync<T>(string sql, DynamicParameters? parameters = null)
        {
            return ExecuteQueryInternalAsync(connection => connection.QueryAsync<T>(sql, parameters));
        }

        // 2-table mapping
        public Task<OperationCollectionResult<TResult>> ExecuteQueryAsync<T1, T2, TResult>(string sql,DynamicParameters parameters,Func<T1, T2, TResult> map,string splitOn)
        {
            return ExecuteQueryInternalAsync(connection =>connection.QueryAsync(sql, map, parameters, splitOn: splitOn));
        }

        // 3-table mapping
        public Task<OperationCollectionResult<TResult>> ExecuteQueryAsync<T1, T2, T3, TResult>(string sql,DynamicParameters parameters,Func<T1, T2, T3, TResult> map,string splitOn)
        {
            return ExecuteQueryInternalAsync(connection => connection.QueryAsync(sql, map, parameters, splitOn: splitOn));
        }

        // 4-table mapping
        public Task<OperationCollectionResult<TResult>> ExecuteQueryAsync<T1, T2, T3, T4, TResult>(string sql,DynamicParameters parameters,Func<T1, T2, T3, T4, TResult> map,string splitOn)
        {
            return ExecuteQueryInternalAsync(connection => connection.QueryAsync(sql, map, parameters, splitOn: splitOn));
        }


        public async Task<OperationResult> InsertAsync<T>(T model)
        {
            try
            {
                var (sql, parameters) = SqlGenerator.GenerateInsert(model);
                return await ExecuteNonQuery(sql, parameters);
            }
            catch (Exception ex)
            {
                return OperationResult.Failed(ex.Message);
            }
        }

        public async Task<OperationResult> UpdateAsync<T>(T model)
        {
            try
            {
                var (sql, parameters) = SqlGenerator.GenerateUpdate(model);
                return await ExecuteNonQuery(sql, parameters);
            }
            catch (Exception ex)
            {
                return OperationResult.Failed(ex.Message);
            }
        }

        public async Task<OperationResult> DeleteAsync<T>(T model)
        {
            try
            {
                var (sql, parameters) = SqlGenerator.GenerateDelete(model);
                return await ExecuteNonQuery(sql, parameters);
            }
            catch (Exception ex)
            {
                return OperationResult.Failed(ex.Message);
            }
        }



        // Non-query (insert/update/delete)
        public async Task<OperationResult> ExecuteNonQuery(string sql, DynamicParameters? parameters = null)
        {
            try
            {
                using var connection = _connectionFactory.CreateConnection();
                var rows = await connection.ExecuteAsync(sql, parameters);

                if (rows > 0)
                    return OperationResult.Success($"{rows} record(s) affected.");

                return OperationResult.NotFound("No rows affected.");
            }
            catch (Exception ex)
            {
                return OperationResult.Failed(ex.Message);
            }
        }

        // Internal shared logic for SELECT queries
        private async Task<OperationCollectionResult<TResult>> ExecuteQueryInternalAsync<TResult>(
            Func<IDbConnection, Task<IEnumerable<TResult>>> queryFunc)
        {
            try
            {
                using var connection = _connectionFactory.CreateConnection();
                var list = (await queryFunc(connection)).ToList();

                if (!list.Any())
                    return OperationCollectionResult<TResult>.NotFound("No records found.");

                return OperationCollectionResult<TResult>.Success(list);
            }
            catch (Exception ex)
            {
                return OperationCollectionResult<TResult>.Failed(ex.Message);
            }
        }
    }
}
