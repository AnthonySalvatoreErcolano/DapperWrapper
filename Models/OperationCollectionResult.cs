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

    public class OperationCollectionResult<T1,T2> : OperationResult
    {
        public IEnumerable<T1>? FirstResult { get; set; }
        public IEnumerable<T2>? SecondResult { get; set; }
        public static OperationCollectionResult<T1, T2> Success(IEnumerable<T1> data1, IEnumerable<T2> data2, string msg = "Success") => new()
        {
            Value = ResponseValue.Success,
            ResponseText = msg,
            FirstResult = data1,
            SecondResult = data2
        };

        public static OperationCollectionResult<T1, T2> Failed(string msg = "Operation failed") => new()
        {
            Value = ResponseValue.Failed,
            ResponseText = msg,
            FirstResult = null,
            SecondResult = null
        };

        public static OperationCollectionResult<T1, T2> NotFound(string msg = "No records found") => new()
        {
            Value = ResponseValue.NotFound,
            ResponseText = msg,
            FirstResult = Enumerable.Empty<T1>(),
            SecondResult = Enumerable.Empty<T2>()
        };
        public static OperationCollectionResult<T1,T2> Invalid(string msg = "Invalid request") => new()
        {
            Value = ResponseValue.Invalid,
            ResponseText = msg,
            FirstResult = null,
            SecondResult= null
        };

        public static OperationCollectionResult<T1,T2> Unauthorized(string msg = "Unauthorized") => new()
        {
            Value = ResponseValue.Unauthorized,
            ResponseText = msg,
            FirstResult = null,
            SecondResult = null
        };
    }

    public class OperationCollectionResult<T1, T2, T3> : OperationResult
    {
        public IEnumerable<T1>? FirstResult { get; set; }
        public IEnumerable<T2>? SecondResult { get; set; }
        public IEnumerable<T3>? ThirdResult { get; set; }

        public static OperationCollectionResult<T1, T2, T3> Success(
            IEnumerable<T1> data1,
            IEnumerable<T2> data2,
            IEnumerable<T3> data3,
            string msg = "Success") => new()
            {
                Value = ResponseValue.Success,
                ResponseText = msg,
                FirstResult = data1,
                SecondResult = data2,
                ThirdResult = data3
            };

        public static OperationCollectionResult<T1, T2, T3> Failed(string msg = "Operation failed") => new()
        {
            Value = ResponseValue.Failed,
            ResponseText = msg,
            FirstResult = null,
            SecondResult = null,
            ThirdResult = null
        };

        public static OperationCollectionResult<T1, T2, T3> NotFound(string msg = "No records found") => new()
        {
            Value = ResponseValue.NotFound,
            ResponseText = msg,
            FirstResult = Enumerable.Empty<T1>(),
            SecondResult = Enumerable.Empty<T2>(),
            ThirdResult = Enumerable.Empty<T3>()
        };

        public static OperationCollectionResult<T1, T2, T3> Invalid(string msg = "Invalid request") => new()
        {
            Value = ResponseValue.Invalid,
            ResponseText = msg,
            FirstResult = null,
            SecondResult = null,
            ThirdResult = null
        };

        public static OperationCollectionResult<T1, T2, T3> Unauthorized(string msg = "Unauthorized") => new()
        {
            Value = ResponseValue.Unauthorized,
            ResponseText = msg,
            FirstResult = null,
            SecondResult = null,
            ThirdResult = null
        };
    }


}
