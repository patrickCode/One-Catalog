using System;

namespace Microsoft.Catalog.Common.Exceptions
{
    public class DomainException: AppException
    {
        public DomainException(string message, Exception exception): base(message, exception)
        {
            ExceptionName = Constants.ExceptionNames.DomainException;
        }
        public DomainException(string correlationId, string userId, int exceptionCode): base(correlationId, userId, exceptionCode)
        {
            ExceptionName = Constants.ExceptionNames.DomainException;
        }
        public DomainException(string message, Exception exception, string correlationId, string userId, int exceptionCode): base(message, exception, correlationId, userId, exceptionCode)
        {
            ExceptionName = Constants.ExceptionNames.DomainException;
        }
    }
}