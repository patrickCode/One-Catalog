using System;

namespace Microsoft.Catalog.Common.Exceptions
{
    public class AzureSearchException: AppException
    {
        public AzureSearchException(string message, Exception exception): base(message, exception)
        {
            ExceptionName = Constants.ExceptionNames.AzureSearchException;
        }
        public AzureSearchException(string message, int exceptionCode): base(message, exceptionCode)
        {
            ExceptionName = Constants.ExceptionNames.AzureSearchException;
        }
        public AzureSearchException(string correlationId, string userId, int exceptionCode): base(correlationId, userId, exceptionCode)
        {
            ExceptionName = Constants.ExceptionNames.AzureSearchException;
        }
        public AzureSearchException(string message, Exception exception, string correlationId, int exceptionCode, string userId = null) : base(message, exception, correlationId, userId, exceptionCode)
        {
            ExceptionName = Constants.ExceptionNames.AzureSearchException;
        }
    }
}