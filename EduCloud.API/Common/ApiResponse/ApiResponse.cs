namespace EduCloud.API.Common.ApiResponse
{
    public class ApiResponse<T>
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public T? Data { get; set; }

        public ApiResponse(int statusCode, string message, T? data = default)
        {
            StatusCode = statusCode;
            Message = message;
            Data = data;
        }

        public static ApiResponse<T> Success(T data, string message = "Success", int statusCode = 200)
        {
            return new ApiResponse<T>(statusCode, message, data);
        }

        public static ApiResponse<T> Fail(string message, int statusCode)
        {
            return new ApiResponse<T>(statusCode, message);
        }
    }
}
