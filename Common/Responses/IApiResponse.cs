namespace ecommerse_api.Common.Responses;


    public static class ErrorCodes
    {
        public const string NoRequestProvided = "NO_REQUEST_PROVIDED";
        public const string ValidationError = "VALIDATION_ERROR";
        public const string NotFound = "NOT_FOUND";
        public const string EmptyRecord = "EMPTY_RECORD";
        public const string ServerError = "SERVER_ERROR";
    }

    public static class ResponseCodes
    {
        // Success Codes
        public const string Ok = "200_OK";
        public const string Created = "201_CREATED";
        public const string Accepted = "202_ACCEPTED";
        public const string NoContent = "204_NO_CONTENT";

        // Client Error Codes
        public const string BadRequest = "400_BAD_REQUEST";
        public const string Unauthorized = "401_UNAUTHORIZED";
        public const string Forbidden = "403_FORBIDDEN";
        public const string NotFound = "404_NOT_FOUND";
        public const string MethodNotAllowed = "405_METHOD_NOT_ALLOWED";
        public const string Conflict = "409_CONFLICT";

        // Server Error Codes
        public const string InternalServerError = "500_INTERNAL_SERVER_ERROR";
        public const string NotImplemented = "501_NOT_IMPLEMENTED";
        public const string BadGateway = "502_BAD_GATEWAY";
        public const string ServiceUnavailable = "503_SERVICE_UNAVAILABLE";
        public const string GatewayTimeout = "504_GATEWAY_TIMEOUT";
    }

    public interface IApiResponse
    {
        bool Success { get; }
        string ResponseCode { get; }
        string Message { get; }
        string ErrorCode { get; }
        string CorrelationId { get; }
        List<string> Details { get; } 
    }

    public class ApiResponse<T> : IApiResponse
    {
        public bool Success => true;
        public string ResponseCode { get; set; } = string.Empty;
        public T? Data { get; set; }
        public string Message { get; set; } = string.Empty; 
        public string ErrorCode { get; set; } = string.Empty;
        string IApiResponse.CorrelationId { get; } = string.Empty;
        public List<string> Details { get; set; } = new List<string>(); 
    }

    public class ApiErrorResponse : IApiResponse
    {
        public bool Success => false;
        public string ResponseCode { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public string ErrorCode { get; set; } = string.Empty;
        public string CorrelationId { get; set; } = string.Empty;
        public List<string> Details { get; set; } = new List<string>(); 
    }