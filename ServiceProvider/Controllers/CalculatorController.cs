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
        [Route("AddTwoNumbers")]
        [HttpGet]
        public IHttpActionResult AddTwoNumbers(int token, int num1, int num2)
        {
            //validate
            if (Authenticate(token))
            {
                return Ok(num1 + num2);
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
        [Route("AddThreeNumbers")]
        [HttpGet]
        public IHttpActionResult AddThreeNumbers(int token, int num1, int num2, int num3)
        {
            if (Authenticate(token))
            {
                return Ok(num1 + num2 + num3);
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
        [Route("MulTwoNumbers")]
        [HttpGet]
        public IHttpActionResult MulTwoNumbers(int token, int num1, int num2)
        {
            if (Authenticate(token))
            {
                return Ok(num1 * num2);
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
        [Route("MulThreeNumbers")]
        [HttpGet]
        public IHttpActionResult MulThreeNumbers(int token, int num1, int num2, int num3)
        {
            if (Authenticate(token))
            {
                return Ok(num1 * num2 * num3);
            }
            else
            {
                return Ok(new InvalidUserModel() { Status = "Denied", Reason = "Authentication Error" });
            }
        }
    }
}
