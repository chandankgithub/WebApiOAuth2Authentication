using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Security.Claims;
using System.Security.Principal;

namespace CK.TokenAuthentication.Controllers
{
    public class TestController : ApiController
    {
        [Authorize]
        public IHttpActionResult Get()
        {
            var principal = Request.GetRequestContext().Principal;
            var claims = Request.GetOwinContext().Authentication.User.Claims;
            
            foreach (var claim in claims)
            {
                
            }
            
            return Ok();
        }
    }
}
