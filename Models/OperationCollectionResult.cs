using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DapperWrapper.Models
{
    public class OperationCollectionResult<T> : OperationResult
    {
        public IEnumerable<T>? Data { get; set; }

        public static OperationCollectionResult<T> Success(IEnumerable<T> data, string msg = "Success") => new()
        {
            Value = ResponseValue.Success,
            ResponseText = msg,
            Data = data
        };

        public static OperationCollectionResult<T> Failed(string msg = "Operation failed") => new()
        {
            Value = ResponseValue.Failed,
            ResponseText = msg,
            Data = null
        };

        public static OperationCollectionResult<T> NotFound(string msg = "No records found") => new()
        {
            Value = ResponseValue.NotFound,
            ResponseText = msg,
            Data = Enumerable.Empty<T>()
        };

        public static OperationCollectionResult<T> Invalid(string msg = "Invalid request") => new()
        {
            Value = ResponseValue.Invalid,
            ResponseText = msg,
            Data = null
        };

        public static OperationCollectionResult<T> Unauthorized(string msg = "Unauthorized") => new()
        {
            Value = ResponseValue.Unauthorized,
            ResponseText = msg,
            Data = null
        };
    }
}
