using TaskManagementSystem.Shared.Enums;

namespace TaskManagementSystem.Shared.Responses
{
    public class ApiResponse<T>
    {
        public ErrorCode ErrorCode { get; set; }
        public string? Message { get; set; }
        public T? Data { get; set; }

        public static ApiResponse<T> SuccessResponse(T data, string? message = null) =>
            new ApiResponse<T> { ErrorCode = ErrorCode.None, Data = data, Message = message };

        public static ApiResponse<T> FailResponse(string message, ErrorCode errorCode = ErrorCode.GeneralApiError) =>
            new ApiResponse<T> { ErrorCode = errorCode, Message = message };
    }
}
