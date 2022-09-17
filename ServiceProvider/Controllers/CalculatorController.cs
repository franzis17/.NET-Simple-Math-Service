using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ServiceProvider.Controllers
{
    [RoutePrefix("api/calculator")]
    public class CalculatorController : ApiController
    {

        [Route("add/{numOne}/{numTwo}")]
        [Route("add")]
        [HttpGet]
        public IHttpActionResult AddTwoNumbers(int numOne, int numTwo)
        {
            //validate

            return Ok(numOne + numTwo);
        }

        [Route("add/{numOne}/{numTwo}/{numThree}")]
        [Route("add")]
        [HttpGet]
        public IHttpActionResult addThreeNumbers(int numOne, int numTwo, int numThree)
        {
            //validate
            return Ok(numOne + numTwo + numThree);
        }

        [Route("mul/{numOne}/{numTwo}")]
        [Route("mul")]
        [HttpGet]
        public IHttpActionResult mullTwoNumbers(int numOne, int numTwo)
        {
            //validate
            return Ok(numOne * numTwo);
        }

        [Route("mul/{numOne}/{numTwo}/{numThree}")]
        [Route("mul")]
        [HttpGet]
        public IHttpActionResult mullThreeNumbers(int numOne, int numTwo, int numThree)
        {
            //validate
            return Ok(numOne * numTwo * numThree);
        }

    }
}
