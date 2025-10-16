using DapperWrapper.Core;
using DapperWrapper.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DapperWrapper.Repositories
{
    public class DapperCommandService
    {
        private readonly Executor _executor;

        public DapperCommandService(Executor executor)
        {
            _executor = executor;
        }

        public Task<OperationResult> InsertAsync<T>(T model) => _executor.InsertAsync(model);

        public Task<OperationResult> UpdateAsync<T>(T model) => _executor.UpdateAsync(model);

        public Task<OperationResult> DeleteAsync<T>(T model) => _executor.DeleteAsync(model);
    }

}
