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
    /// Exception returned to WebAPI to return a HttpStatusCode.NotFound
    /// </summary>
    public class NotFoundException : Exception
    {
        public NotFoundException() : base() { }
        public NotFoundException(string message) : base(message) { }
        public NotFoundException(string message, Exception exception) : base(message, exception) { }
        public NotFoundException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
