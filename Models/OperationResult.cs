namespace DapperWrapper.Models
{
    public class OperationResult
    {
        public string ResponseText { get; set; } = string.Empty;
        public ResponseValue Value { get; set; }

        // ----- Shared factory -----
        protected static TRes Create<TRes>(ResponseValue value, string msg)
            where TRes : OperationResult, new()
            => new()
            {
                Value = value,
                ResponseText = msg
            };

        public static OperationResult Success(string msg = "Success")
            => Create<OperationResult>(ResponseValue.Success, msg);

        public static OperationResult Failed(string msg = "Operation failed")
            => Create<OperationResult>(ResponseValue.Failed, msg);

        public static OperationResult NotFound(string msg = "No records found")
            => Create<OperationResult>(ResponseValue.NotFound, msg);

        public static OperationResult Invalid(string msg = "Invalid request")
            => Create<OperationResult>(ResponseValue.Invalid, msg);

        public static OperationResult Unauthorized(string msg = "Unauthorized")
            => Create<OperationResult>(ResponseValue.Unauthorized, msg);
    }


    public class OperationResult<T> : OperationResult
    {
        public T? Data { get; set; }

        // ----- Generic versions using shared base factory -----
        private static OperationResult<T> Create(ResponseValue value, string msg, T? data = default)
            => new()
            {
                Value = value,
                ResponseText = msg,
                Data = data
            };

        public static OperationResult<T> Success(T data, string msg = "Success")
            => Create(ResponseValue.Success, msg, data);

        public static OperationResult<T> Success(string msg = "Success")
            => Create(ResponseValue.Success, msg);

        public static OperationResult<T> Failed(string msg = "Operation failed")
            => Create(ResponseValue.Failed, msg);

        public static OperationResult<T> NotFound(string msg = "No records found")
            => Create(ResponseValue.NotFound, msg);

        public static OperationResult<T> Invalid(string msg = "Invalid request")
            => Create(ResponseValue.Invalid, msg);

        public static OperationResult<T> Unauthorized(string msg = "Unauthorized")
            => Create(ResponseValue.Unauthorized, msg);
    }
}
