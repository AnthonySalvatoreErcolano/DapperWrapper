using System;
using System.ClientModel.Primitives;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DapperWrapper.Models
{
    public class OperationResult
    {
        public string ResponseText { get; set; } = string.Empty;
        public ResponseValue Value { get; set; }

        public bool IsSuccess => Value == ResponseValue.Success || Value == ResponseValue.Warning;

        public static OperationResult Success(string msg = "Success") => new()
        {
            Value = ResponseValue.Success,
            ResponseText = msg
        };

        public static OperationResult Failed(string msg = "Operation failed") => new()
        {
            Value = ResponseValue.Failed,
            ResponseText = msg
        };

        public static OperationResult NotFound(string msg = "No records found") => new()
        {
            Value = ResponseValue.NotFound,
            ResponseText = msg
        };

        public static OperationResult Invalid(string msg = "Invalid request") => new()
        {
            Value = ResponseValue.Invalid,
            ResponseText = msg
        };

        public static OperationResult Unauthorized(string msg = "Unauthorized") => new()
        {
            Value = ResponseValue.Unauthorized,
            ResponseText = msg
        };

    }
}
