using System;

namespace Microsoft.Catalog.Common.Exceptions
{
    public class DatabaseException: AppException
    {
        public DatabaseException(string message, Exception exception): base(message, exception)
        {
            ExceptionName = Constants.ExceptionNames.DatabaseException;
        }
        public DatabaseException(string correlationId, string userId, int exceptionCode): base(correlationId, userId, exceptionCode)
        {
            ExceptionName = Constants.ExceptionNames.DatabaseException;
        }
        public DatabaseException(string message, Exception exception, string correlationId, string userId, int exceptionCode): base(message, exception, correlationId, userId, exceptionCode)
        {
            ExceptionName = Constants.ExceptionNames.DatabaseException;
        }
    }
}