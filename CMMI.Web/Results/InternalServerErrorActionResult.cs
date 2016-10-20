using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;

namespace CMMI.Web.Results
{
    public class InternalServerErrorActionResult : BaseHttpActionResult
    {
        private readonly string _source;
        public Encoding Encoding { get; private set; }

        /// <summary>   Constructor. </summary>
        /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are null. </exception>
        /// <param name="content">  The content message of the response. </param>
        /// <param name="source">   Source for the. </param>
        /// <param name="encoding"> The encoding. </param>
        /// <param name="request">  The request. </param>
        /// <param name="ex">       The exception. </param>
        public InternalServerErrorActionResult(string content, string source, Encoding encoding,
                                HttpRequestMessage request, Exception ex) : base(content, ex)
        {
            if (encoding == null)
            {
                throw new ArgumentNullException("encoding");
            }

            if (request == null)
            {
                throw new ArgumentNullException("request");
            }

            _source = source;
            Encoding = encoding;
        }

        protected override HttpResponseMessage Execute()
        {
            // Content is the customizable generic error coming from Generic Exception Handler
            var err = new HttpError(Content)
            {
                { "Source", _source },
                { "ExceptionInfo", OriginalException },
                { "RequestUri", Request.RequestUri}

            };
            return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, err);
        }
    }
}