using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace CMMI.Web.Results
{
    public class BaseHttpActionResult : IHttpActionResult
    {
        public HttpStatusCode? StatusCode { get; set; }
        public HttpRequestMessage Request { get; private set; }
        public Exception OriginalException { get; private set; }
        public string Content { get; private set; }


        public BaseHttpActionResult(string content, Exception ex, HttpRequestMessage request = null)
        {
            if (content == null)
            {
                throw new ArgumentNullException("content");
            }

            if (ex == null)
            {
                throw new ArgumentNullException("ex");
            }

            Content = content;
            OriginalException = ex;
            // if request is null try grabbing it from the current context otherwise take what was passed in
            Request = request ?? ((HttpContext.Current != null) ? (HttpRequestMessage)HttpContext.Current.Items["MS_HttpRequestMessage"] : null);
        }

        public BaseHttpActionResult(string content, Exception ex, HttpRequestMessage request, HttpStatusCode? statusCode = null) : this(content, ex, request)
        {
            if (statusCode != null)
            {
                StatusCode = statusCode;
            }
        }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(Execute());
        }

        protected virtual HttpResponseMessage Execute()
        {
            // Content is the customizable generic error coming from Generic Exception Handler
            var err = new HttpError(Content)
            {
                { "ExceptionInfo", OriginalException },
                { "RequestUri", Request.RequestUri}
            };
            return Request.CreateErrorResponse(StatusCode ?? HttpStatusCode.NoContent, err);
        }
    }
}