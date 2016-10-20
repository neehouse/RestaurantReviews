using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
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
            var exceptionType = context.Exception.GetType();

            // Return 404 - Not Found
            if (exceptionType == typeof(NotFoundException))
            {
                context.Result = new BaseHttpActionResult(
                    context.Exception.Message,
                    context.Exception,
                    context.Request,
                    HttpStatusCode.NotFound);
                return;
            }

            // Return 403 - Forbidden
            if (exceptionType == typeof(ForbiddenException))
            {
                context.Result = new BaseHttpActionResult(
                    context.Exception.Message,
                    context.Exception,
                    context.Request,
                    HttpStatusCode.Forbidden);
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
    }
}
