using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using CMMI.Business.Attributes;
using CMMI.Web.Handlers;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json.Serialization;

namespace CMMI.Web
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Use camel case for JSON data.  Removed as I already have code written when I added this line.. need to update JS/html
//            config.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            // Configure Web API to use only bearer token authentication.
            config.SuppressDefaultHostAuthentication();
            config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));

            // Automatic ModelState Validation
            config.Filters.Add(new ModelStateValidationAttribute());

            // Restful Exception Handler
//            config.Services.Replace(typeof(IExceptionHandler), new WebApiExceptionHandler());

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
