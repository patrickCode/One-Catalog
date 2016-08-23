using System;

namespace Microsoft.Catalog.Common.Exceptions
{
    public class AppException: Exception
    {
        public string ExceptionName { get; set; }
        public string CorrelationId { get; set; }
        public string UserId { get; set; }
        public int ExceptionCode { get; set; }
        public AppException(string message, Exception exception): base(message, exception) { }
        public AppException(string message, int exceptionCode): base(message)
        {
            ExceptionCode = exceptionCode;
            CorrelationId = Guid.NewGuid().ToString();
        }
        public AppException(string correlationId, string userId, int exceptionCode)
        {
            CorrelationId = correlationId;
            UserId = userId;
            ExceptionCode = exceptionCode;
        }
        public AppException(string message, Exception exception, string correlationId, string userId, int exceptionCode): base(message, exception)
        {
            CorrelationId = correlationId;
            UserId = userId;
            ExceptionCode = exceptionCode;
        }
    }
}