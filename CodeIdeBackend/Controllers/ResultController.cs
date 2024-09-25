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

            request.Code = @"
namespace LearningCSharpConcept
{
    public class FizzBuzz
    {
        /// <summary>
        /// Returns:
        /// ""Fizz"" if the number is divisible by 3,
        /// ""Buzz"" if the number is divisible by 5,
        /// ""FizzBuzz"" if the number is divisible by both 3 and 5,
        /// the number if it's not divisible by either 3 or 5
        /// </summary>
        /// <param name=""number"">the integer to get output for</param>
        /// <returns>a string with the proper output as described in the summary</returns>
        public static string GetOutput(int number)
        {
            string output;

            if ((number % 3 == 0) && (number % 5 == 0))
            {
                output = ""FizzBuzz"";
            }
            else if (number % 3 == 0)
            {
                output = ""Fizz"";
            }
            else if (number % 5 == 0)
            {
                output = ""Buzz"";
            }
            else
            {
                output = number.ToString();
            }

            return output;
        }
    }
}
";

            var handler = new Handler();
            var response = handler.Handle(request);

            return Ok(response);
        }
    }
}
