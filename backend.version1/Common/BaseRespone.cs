namespace backend.Common
{
    public abstract class BaseResponse<T>
    {
        protected BaseResponse(T data, bool succeeded, string message, string[] errors, int statusCode)
        {
            Data = data;
            Succeeded = succeeded;
            Message = message;
            Errors = errors;
            StatusCode = statusCode;
        }

        public T Data { get; set; }
        public bool Succeeded { get; set; }
        public string[] Errors { get; set; }
        public string Message { get; set; }
        public int StatusCode { get; set; }
    }

    public class SuccessResponse<T> : BaseResponse<T>
    {
        public SuccessResponse(T data, string message)
            : base(data, true, message, Array.Empty<string>(), 200)
        {
        }
    }

    public class ErrorResponse<T> : BaseResponse<T>
    {
        public ErrorResponse(string[] errors, int statusCode = 400)
            : base(default(T), false, string.Empty, errors, statusCode)
        {
        }
    }
}