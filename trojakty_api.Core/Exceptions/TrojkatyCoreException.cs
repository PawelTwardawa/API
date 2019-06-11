using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace trojakty_api.Core.Exceptions
{
    public class TrojkatyCoreException : Exception
    {
        public TrojkatyCoreException()
        {
        }

        public TrojkatyCoreException(string message) : base(message)
        {
        }

        public TrojkatyCoreException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected TrojkatyCoreException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
