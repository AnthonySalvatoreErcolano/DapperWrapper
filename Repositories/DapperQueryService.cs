using DapperWrapper.Core;
using Dapper;
using DapperWrapper.Models;

namespace DapperWrapper.Repositories
{
    public delegate (string, DynamicParameters) QueryBuilder<TFilter>(TFilter filter);

    public class DapperQueryService
    {
        private readonly Executor _executor;

        // Delegate that users will implement
        public delegate (string Sql, DynamicParameters Params) QueryBuilder<TFilter>(TFilter filters);
        public delegate (string Sql, DynamicParameters Params) MultiQueryBuilder<TFilter>(TFilter filters);
        public DapperQueryService(Executor executor)
        {
            _executor = executor;
        }

        public async Task<OperationCollectionResult<TResult>> Get<T, TResult, TFilter>(QueryBuilder<TFilter> builder,TFilter filters,Func<T, TResult>? map = null)
        {
            var (sql, parameters) = builder(filters);

            if (string.IsNullOrWhiteSpace(sql))
                return OperationCollectionResult<TResult>.Invalid("SQL cannot be empty.");
            try
            {
                if (map == null)
                {
                    var data = (await _executor.ExecuteQueryAsync<TResult>(sql, parameters));

                    if (data == null || data.Data == null) 
                        return OperationCollectionResult<TResult>.Invalid("Invalid search");
                    if(data.IsSuccess==false) 
                        return OperationCollectionResult<TResult>.Failed(data.ResponseText);

                    return data.Data.Any()? OperationCollectionResult<TResult>.Success(data.Data): OperationCollectionResult<TResult>.NotFound("No records found.");
                }
                else
                {
                    var baseData = (await _executor.ExecuteQueryAsync<T>(sql, parameters));
                    if (baseData==null || baseData.Data==null) 
                        return OperationCollectionResult<TResult>.Invalid("Invalid search");
                    if(baseData.Data.Any() == false) 
                        return OperationCollectionResult<TResult>.NotFound("No records found.");

                    var mapped = baseData.Data.Select(map);
                    return OperationCollectionResult<TResult>.Success(mapped);
                }
            }
            catch (Exception ex)
            {
                return OperationCollectionResult<TResult>.Failed(ex.Message);
            }
        }

        public async Task<OperationCollectionResult<TResult>> Get<T, TResult>((string Sql, DynamicParameters Params) builder, Func<T, TResult>? map = null) => await Get<T, TResult, (string Sql, DynamicParameters Params)>(_ => builder, builder, map);

        public async Task<OperationCollectionResult<T>> Get<T>((string Sql, DynamicParameters Params) builder) => await Get<T, T>(builder);
        public async Task<OperationCollectionResult<T>> Get<T, TFilter>(QueryBuilder<TFilter> builder, TFilter filters)
           => await Get<T, T, TFilter>(builder, filters);
        public async Task<OperationCollectionResult<T1, T2>> GetMulti<T1, T2, TFilter>(QueryBuilder<TFilter> builder, TFilter filters)
        {
            var (sql, parameters) = builder(filters);

            if (string.IsNullOrWhiteSpace(sql))
                return OperationCollectionResult<T1, T2>.Invalid("SQL cannot be empty.");
            return await _executor.ExecuteQueryMultipleAsync<T1, T2>(sql, parameters);
            try
            {
                 var multi = await _executor.ExecuteQueryMultipleAsync<T1,T2>(sql, parameters);


                return OperationCollectionResult<T1, T2>.Success(multi.FirstResult, multi.SecondResult);
            }
            catch (Exception ex)
            {
                return OperationCollectionResult<T1, T2>.Failed(ex.Message);
            }
        }

        public async Task<OperationCollectionResult<TResult>> GetByJoin<T1,T2,TResult>((string Sql, DynamicParameters Params) builder, Func<T1,T2,TResult> tableMap, string splitOn)
        {
            var (sql, parameters) = builder;

            if (string.IsNullOrWhiteSpace(sql))
                return OperationCollectionResult<TResult>.Invalid("SQL cannot be empty.");
            if (tableMap == null) 
                return OperationCollectionResult<TResult>.Invalid("Table mapper cannot be null.");
            
            try
            {
                var data = (await _executor.ExecuteQueryAsync<T1,T2,TResult>(sql, parameters, tableMap,splitOn));
                if (data == null || data.Data == null)
                    return OperationCollectionResult<TResult>.Invalid("Invalid search");
                if (data.Data.Any() == false)
                    return OperationCollectionResult<TResult>.NotFound("No records found.");

                return OperationCollectionResult<TResult>.Success(data.Data);
                
            }
            catch (Exception ex)
            {
                return OperationCollectionResult<TResult>.Failed(ex.Message);
            }

        }
        public async Task<OperationCollectionResult<TResult>> GetByJoin<T1, T2, T3,TResult>((string Sql, DynamicParameters Params) builder, Func<T1, T2,T3, TResult> tableMap, string splitOn)
        {
            var (sql, parameters) = builder;

            if (string.IsNullOrWhiteSpace(sql))
                return OperationCollectionResult<TResult>.Invalid("SQL cannot be empty.");
            if (tableMap == null)
                return OperationCollectionResult<TResult>.Invalid("Table mapper cannot be null.");

            try
            {
                var data = (await _executor.ExecuteQueryAsync<T1, T2,T3, TResult>(sql, parameters, tableMap, splitOn));
                if (data == null || data.Data == null)
                    return OperationCollectionResult<TResult>.Invalid("Invalid search");
                if (data.Data.Any() == false)
                    return OperationCollectionResult<TResult>.NotFound("No records found.");

                return OperationCollectionResult<TResult>.Success(data.Data);

            }
            catch (Exception ex)
            {
                return OperationCollectionResult<TResult>.Failed(ex.Message);
            }

        }
        public async Task<OperationCollectionResult<TResult>> GetByJoin<T1, T2, T3,T4, TResult>((string Sql, DynamicParameters Params) builder, Func<T1, T2, T3,T4, TResult> tableMap, string splitOn)
        {
            var (sql, parameters) = builder;

            if (string.IsNullOrWhiteSpace(sql))
                return OperationCollectionResult<TResult>.Invalid("SQL cannot be empty.");
            if (tableMap == null)
                return OperationCollectionResult<TResult>.Invalid("Table mapper cannot be null.");

            try
            {
                var data = (await _executor.ExecuteQueryAsync<T1, T2, T3,T4, TResult>(sql, parameters, tableMap, splitOn));
                if (data == null || data.Data == null)
                    return OperationCollectionResult<TResult>.Invalid("Invalid search");
                if (data.Data.Any() == false)
                    return OperationCollectionResult<TResult>.NotFound("No records found.");

                return OperationCollectionResult<TResult>.Success(data.Data);

            }
            catch (Exception ex)
            {
                return OperationCollectionResult<TResult>.Failed(ex.Message);
            }

        }

    }
}
