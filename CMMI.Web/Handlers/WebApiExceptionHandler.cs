using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.ExceptionHandling;
using CMMI.Business.Exceptions;
using CMMI.Web.Results;

namespace CMMI.Web.Exceptions
{
    /// <summary>
    /// Custom Exception Handler for WebApi to handle standard cases
    /// </summary>
    internal class WebApiExceptionHandler : ExceptionHandler
    {
        public override void Handle(ExceptionHandlerContext context)
        {
            var exceptionType = context.Exception.GetType();

            if (exceptionType == typeof(NotFoundException))
            {
                context.Result = new BaseHttpActionResult(
                    context.Exception.Message,
                    context.Exception,
                    context.Request,
                    HttpStatusCode.NotFound);
                return;
            }

            if (exceptionType == typeof(ForbiddenException))
            {
                context.Result = new BaseHttpActionResult(
                    context.Exception.Message,
                    context.Exception,
                    context.Request,
                    HttpStatusCode.Forbidden);
                return;
            }

            if (exceptionType == typeof(ForbiddenException))
            {

            }

            context.Result = new InternalServerErrorActionResult(
                "An unhandled exception occurred; check the log for more information.",
                context.Exception.Source,
                Encoding.UTF8,
                context.Request,
                context.Exception);
        }
    }
}
