using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace CMMI.Business
{
    public class IdentityBase
    {
        public Guid CurrentUserGuid
        {
            get
            {
                if (System.Threading.Thread.CurrentPrincipal == null)
                {
                    return Guid.Empty;
                }

                var userIdString = System.Threading.Thread.CurrentPrincipal.Identity.GetUserId();

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
