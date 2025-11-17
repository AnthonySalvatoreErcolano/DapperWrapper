namespace DapperWrapper.Models
{
    public class OperationCollectionResult<T>
    {
        public string ResponseText { get; set; } = string.Empty;
        public ResponseValue Value { get; set; }
        public IEnumerable<T>? Data { get; set; }

        private static OperationCollectionResult<T> Create(ResponseValue value, string msg, IEnumerable<T>? data = null) => new()
        {
            Value = value,
            ResponseText = msg,
            Data = data
        };

        public static OperationCollectionResult<T> Success(IEnumerable<T> data, string msg = "Success")
            => Create(ResponseValue.Success, msg, data);

        public static OperationCollectionResult<T> Success(string msg = "Success")
            => Create(ResponseValue.Success, msg);

        public static OperationCollectionResult<T> Failed(string msg = "Operation failed")
            => Create(ResponseValue.Failed, msg);

        public static OperationCollectionResult<T> NotFound(string msg = "No records found")
            => Create(ResponseValue.NotFound, msg);

        public static OperationCollectionResult<T> Invalid(string msg = "Invalid request")
            => Create(ResponseValue.Invalid, msg);

        public static OperationCollectionResult<T> Unauthorized(string msg = "Unauthorized")
            => Create(ResponseValue.Unauthorized, msg);
    }

    public class OperationCollectionResult<T1, T2>
    {
        public string ResponseText { get; set; } = string.Empty;
        public ResponseValue Value { get; set; }
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
        public static OperationCollectionResult<T1, T2> Invalid(string msg = "Invalid request") => new()
        {
            Value = ResponseValue.Invalid,
            ResponseText = msg,
            FirstResult = null,
            SecondResult = null
        };

        public static OperationCollectionResult<T1, T2> Unauthorized(string msg = "Unauthorized") => new()
        {
            Value = ResponseValue.Unauthorized,
            ResponseText = msg,
            FirstResult = null,
            SecondResult = null
        };
    }

    public class OperationCollectionResult<T1, T2, T3>
    {
        public string ResponseText { get; set; } = string.Empty;
        public ResponseValue Value { get; set; }
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
