using Registry.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Registry.Controllers
{
    [RoutePrefix("api/registry")]
    public class RegistryController : ApiController
    {
        //Gets singleton of the Authenticator
        private Authenticator.IAuth authenticator = Authenticator.AuthenticatorSingleton.GetInstance();

        //Helper function to grab a list of services from the text file
        private IEnumerable<Service> GetServices()
        {
            return ServiceList.AllServices();
        }

        //Helper function to Authenticate a token with the Authenticator service
        private bool Authenticate(int token)
        {
            string result = authenticator.Validate(token);
            return result.Equals("validated");
        }

        /**
         * Type: GET
         * Description: Given a search term string, search the registered services matching the search term
         * Params: token (int), searchTerm (string)
         * Returns: list of JSON objects (service descriptions)
         * **/
        [Route("services/{token}/{searchTerm}")]
        [Route("services")]
        [HttpGet]
        public IHttpActionResult GetServiceBySearch(int token, string searchTerm)
        {
            if (Authenticate(token))
            {
                var searchResults = new List<Service>();

                foreach (Service service in GetServices())
                {
                    if (service.Description.ToLower().Contains(searchTerm.ToLower()) || service.Name.ToLower().Contains(searchTerm.ToLower()))
                    {
                        searchResults.Add(service);
                    }
                }
                if (searchResults.Count() > 0)
                {
                    return Ok(searchResults);
                }
                else
                {
                    return NotFound();
                }
            }
            else
            {
                return Ok(new InvalidUserModel() { Status = "Denied", Reason = "Authentication Error" });
            }
        }


        /**
         * Type: GET
         * Description: Returns all registered services
         * Params: token (int)
         * Returns: list of JSON objects (service descriptions)
         * **/
        [Route("services/{token}")]
        [Route("services")]
        [HttpGet]
        public IHttpActionResult GetAllServices(int token)
        {
            if (Authenticate(token))
            {
                var searchResults = new List<Service>();

                foreach (Service service in GetServices())
                {
                    searchResults.Add(service);
                }

                if (searchResults.Count() > 0)
                {

                    return Ok(searchResults);
                }
                else
                {
                    return NotFound();
                }
            }
            else
            {
                return Ok(new InvalidUserModel() { Status = "Denied", Reason = "Authentication Error" });
            }
        }

        /**
         * Type: POST
         * Description: Publish a service to the registry
         * Params: token (int)
         * Body: JSON object matching the Service model (found in /Models)
         * Returns: Nothing
         * **/
        [Route("publish/{token}/{service}")]
        [Route("publish")]
        [HttpPost]
        public IHttpActionResult PostNewService(int token, Service service)
        {
            if (Authenticate(token))
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest("Invalid Data");
                }
                ServiceList.PublishService(service);
                return Ok();
            }
            else
            {
                return Ok(new InvalidUserModel() { Status = "Denied", Reason = "Authentication Error" });
            }
        }


        /**
         * Type: POST
         * Description: Unpublish a service to the registry
         * Params: token (int)
         * Body: JSON object matching the Service model (found in /Models)
         * Returns: Nothing
         * **/
        [Route("unpublish/{token}/{service}")]
        [Route("unpublish")]
        [HttpPost]
        public IHttpActionResult PostRemoveService(int token, Service service)
        {

            if (Authenticate(token))
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest("Invalid Data");
                }

                if (ServiceList.DeleteService(service))
                {
                    //found service and deleted
                    return Ok();
                }
                else
                {
                    //did not find service
                    return BadRequest();
                }
            }
            else
            {
                return Ok(new InvalidUserModel() { Status = "Denied", Reason = "Authentication Error" });
            }
        }
    }
}
