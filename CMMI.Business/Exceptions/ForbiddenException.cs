using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace CMMI.Business.Exceptions
{
    /// <summary>
    /// Exception returned to WebAPI to return a HttpStatusCode.Forbidden
    /// </summary>
    public class ForbiddenException : Exception
    {
        public ForbiddenException() : base() { }
        public ForbiddenException(string message) : base(message) { }
        public ForbiddenException(string message, Exception exception) : base(message, exception) { }
        public ForbiddenException(SerializationInfo info, StreamingContext context) : base(info, context) { }

    }
}
