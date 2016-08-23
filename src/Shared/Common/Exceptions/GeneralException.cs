using System;

namespace Microsoft.Catalog.Common.Exceptions
{
    public class GeneralException: AppException
    {
        public GeneralException(string message, Exception exception): base(message, exception)
        {
            ExceptionName = Constants.ExceptionNames.GeneralException;
        }
        public GeneralException(string correlationId, string userId, int exceptionCode): base(correlationId, userId, exceptionCode)
        {
            ExceptionName = Constants.ExceptionNames.GeneralException;
        }
        public GeneralException(string message, Exception exception, string correlationId, string userId, int exceptionCode): base(message, exception, correlationId, userId, exceptionCode)
        {
            ExceptionName = Constants.ExceptionNames.GeneralException;
        }
        public GeneralException(string message, int exceptionCode): base(message, exceptionCode)
        {
            ExceptionName = Constants.ExceptionNames.GeneralException;
        }
    }
}