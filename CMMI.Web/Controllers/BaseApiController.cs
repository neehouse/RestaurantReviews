using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Microsoft.AspNet.Identity;

namespace CMMI.Web.Controllers
{
    public abstract class BaseApiController : ApiController
    {
        public Guid CurrentUserGuid
        {
            get
            {
                if (HttpContext.Current == null)
                {
                    return Guid.Empty;
                }
                var userIdString = HttpContext.Current.User.Identity.GetUserId();
                Guid userId;
                if (Guid.TryParse(userIdString, out userId))
                {
                    return userId;
                }
                return Guid.Empty;
            }
        }


    }
}
