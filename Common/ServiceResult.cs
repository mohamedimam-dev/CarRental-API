namespace CarRental.API.Common
{
    public class ServiceResult<T>
    {
        public ServiceResultStatus Status { get; }

        public string? Message { get; }

        public T? Data { get; }

        public bool IsSuccess => Status == ServiceResultStatus.Success;

        private ServiceResult(ServiceResultStatus status, T? data, string? message)
        {
            Status = status;
            Data = data;
            Message = message;
        }

        public static ServiceResult<T> Success(T data)
            => new(ServiceResultStatus.Success, data, null);

        public static ServiceResult<T> NotFound(string message)
            => new(ServiceResultStatus.NotFound, default, message);

        public static ServiceResult<T> BadRequest(string message)
            => new(ServiceResultStatus.BadRequest, default, message);

        public static ServiceResult<T> Conflict(string message)
            => new(ServiceResultStatus.Conflict, default, message);
    }
}
