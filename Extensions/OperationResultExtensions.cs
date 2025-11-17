using DapperWrapper.Models;

namespace DapperWrapper.Extensions
{
    public static class OperationResultExtensions
    {
        extension(OperationResult operationResult)
        {
            public bool IsSuccess => operationResult.Value == ResponseValue.Success || operationResult.Value == ResponseValue.Warning;
        }

        extension<T>(OperationResult<T> operationResult)
        {
            public bool IsSuccess => operationResult.Value == ResponseValue.Success || operationResult.Value == ResponseValue.Warning;
            public bool HasData => operationResult.Data is not null;
        }

        extension<T>(OperationCollectionResult<T> operationResult)
        {
            public bool IsSuccess => operationResult.Value == ResponseValue.Success || operationResult.Value == ResponseValue.Warning;

            public bool HasAny => operationResult.Data?.Any() == true;

            public bool IsNullOrEmpty => operationResult.Data == null || !operationResult.Data.Any();
        }
        extension<T1, T2>(OperationCollectionResult<T1, T2> operationResult)
        {
            public bool IsSuccess => operationResult.Value == ResponseValue.Success || operationResult.Value == ResponseValue.Warning;

            public bool HasEither =>
                (operationResult.FirstResult?.Any() == true)
                || (operationResult.SecondResult?.Any() == true);

            public bool HasBoth =>
                (operationResult.FirstResult?.Any() == true)
                && (operationResult.SecondResult?.Any() == true);

            public bool HasFirst =>
                operationResult.FirstResult?.Any() == true;

            public bool HasSecond =>
                operationResult.SecondResult?.Any() == true;

            public bool IsNullOrEmpty =>
                (operationResult.FirstResult == null || !operationResult.FirstResult.Any())
                && (operationResult.SecondResult == null || !operationResult.SecondResult.Any());
        }
        extension<T1, T2, T3>(OperationCollectionResult<T1, T2, T3> operationResult)
        {
            public bool IsSuccess =>
                operationResult.Value == ResponseValue.Success ||
                operationResult.Value == ResponseValue.Warning;

            public bool HasAnyFirst =>
                operationResult.FirstResult?.Any() == true;

            public bool IsNullOrEmptyFirst =>
                operationResult.FirstResult == null ||
                !operationResult.FirstResult.Any();

            public bool HasAnySecond =>
                operationResult.SecondResult?.Any() == true;

            public bool IsNullOrEmptySecond =>
                operationResult.SecondResult == null ||
                !operationResult.SecondResult.Any();

            public bool HasAnyThird =>
                operationResult.ThirdResult?.Any() == true;

            public bool IsNullOrEmptyThird =>
                operationResult.ThirdResult == null ||
                !operationResult.ThirdResult.Any();
        }
    }
}
