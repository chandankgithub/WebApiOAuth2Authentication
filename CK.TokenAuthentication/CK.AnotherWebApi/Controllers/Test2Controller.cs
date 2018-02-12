using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace CK.AnotherWebApi.Controllers
{
    public class Test2Controller : ApiController
    {
        [Authorize]
        public async Task<IHttpActionResult> Get()
        {
            return Ok();
        }
    }
}
