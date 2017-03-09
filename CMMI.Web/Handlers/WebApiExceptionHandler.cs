using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.ExceptionHandling;
using CMMI.Business.Exceptions;
using CMMI.Web.Results;

namespace CMMI.Web.Handlers
{
    /// <summary>
    /// Custom Exception Handler for WebApi to handle standard cases
    /// </summary>
    internal class WebApiExceptionHandler : ExceptionHandler
    {
        public override void Handle(ExceptionHandlerContext context)
        {
            // Return 404 - Not Found
            if (context.Exception is NotFoundException)
            {
                context.Result = new BaseHttpActionResult(
                    context.Exception.Message,
                    context.Exception,
                    context.Request,
                    HttpStatusCode.NotFound);
                return;
            }

            // Return 403 - Forbidden
            if (context.Exception is ForbiddenException)
            {
                context.Result = new BaseHttpActionResult(
                    context.Exception.Message,
                    context.Exception,
                    context.Request,
                    HttpStatusCode.Forbidden);
                return;
            }

            // Return 501 - Not Implemented.
            if (context.Exception is NotImplementedException)
            {
                context.Result = new BaseHttpActionResult(
                    context.Exception.Message,
                    context.Exception,
                    context.Request,
                    HttpStatusCode.NotImplemented);
                return;
            }

            // add addition exception types here.






            // Return 500 - Server Error (default)
            context.Result = new BaseHttpActionResult(
                    context.Exception.Message,
                    context.Exception,
                    context.Request,
                    HttpStatusCode.InternalServerError); ;
        }

        //public Task HandleAsync(ExceptionHandlerContext context, CancellationToken cancellationToken)
        //{
        //    return Task.FromResult<>(Handle(context));
        //}
    }
}
