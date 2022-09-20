using ServiceProvider.Models;
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
        //Gets singleton of the Authenticator
        private Authenticator.IAuth authenticator = Authenticator.AuthenticatorSingleton.GetInstance();

        //Helper function to Authenticate a token with the Authenticator service
        private bool Authenticate(int token)
        {
            string result = authenticator.Validate(token);
            return result.Equals("validated");
        }

        /**
         * Type: GET
         * Description: Adds two numbers together
         * Params: token (int), numOne (int), numTwo (int)
         * Returns: the result of the two numbers added together
         * **/
        [Route("add/{token}/{numOne}/{numTwo}")]
        [Route("add")]
        [HttpGet]
        public IHttpActionResult addTwoNumbers(int token, int numOne, int numTwo)
        {
            //validate
            if (Authenticate(token))
            {
                return Ok(numOne + numTwo);
            }
            else
            {
                return Ok(new InvalidUserModel() { Status = "Denied", Reason = "Authentication Error" });
            }
        }

        /**
         * Type: GET
         * Description: Adds three numbers together
         * Params: token (int), numOne (int), numTwo (int), numThree (int)
         * Returns: the result of the three numbers added together
         * **/
        [Route("add/{token}/{numOne}/{numTwo}/{numThree}")]
        [Route("add")]
        [HttpGet]
        public IHttpActionResult addThreeNumbers(int token, int numOne, int numTwo, int numThree)
        {

            if (Authenticate(token))
            {
                return Ok(numOne + numTwo + numThree);
            }
            else
            {
                return Ok(new InvalidUserModel() { Status = "Denied", Reason = "Authentication Error" });
            }
        }

        /**
         * Type: GET
         * Description: Multiply two numbers together
         * Params: token (int), numOne (int), numTwo (int)
         * Returns: the result of the two numbers mulitplied together
         * **/
        [Route("mul/{token}/{numOne}/{numTwo}")]
        [Route("mul")]
        [HttpGet]
        public IHttpActionResult mulTwoNumbers(int token, int numOne, int numTwo)
        {
            //validate

            if (Authenticate(token))
            {
                return Ok(numOne * numTwo);
            }
            else
            {
                return Ok(new InvalidUserModel() { Status = "Denied", Reason = "Authentication Error" });
            }
        }

        /**
         * Type: GET
         * Description: Multiply three numbers together
         * Params: token (int), numOne (int), numTwo (int), numThree (int)
         * Returns: the result of the three numbers mulitplied together
         * **/
        [Route("mul/{token}/{numOne}/{numTwo}/{numThree}")]
        [Route("mul")]
        [HttpGet]
        public IHttpActionResult mulThreeNumbers(int token, int numOne, int numTwo, int numThree)
        {

            if (Authenticate(token))
            {
                return Ok(numOne * numTwo * numThree);
            }
            else
            {
                return Ok(new InvalidUserModel() { Status = "Denied", Reason = "Authentication Error" });
            }
        }
    }
}
