using CodeIdeBackend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace CodeIdeBackend.Controllers
{
    [RoutePrefix("api/v1/result")]
    public class ResultController : ApiController
    {
        [HttpPost]
        [Route("")]
        public async Task<IHttpActionResult> RunCode([FromBody] TestRequest request)
        {
            request = request ?? new TestRequest();

            var handler = new Handler();
            var response = handler.Handle(request);

            return Ok(response);
        }
    }
}
