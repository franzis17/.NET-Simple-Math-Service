using Registry.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Registry.Controllers
{
    [RoutePrefix("api/registry")]
    public class RegistryController : ApiController
    {
        private Authenticator.IAuth authenticator;

        public RegistryController()
        {
            
        }

        private IEnumerable<Service> GetServices()
        {
            return ServiceList.AllServices();
        }

        [Route("services/{token}/{searchTerm}")]
        [Route("services")]
        [HttpGet]
        public IHttpActionResult GetServiceBySearch(AuthenticationToken token, string searchTerm)
        {
            //validate user
            //if user OK
                //open text file
                //parse text file for services with descriptions matcing search term
                //return results and status OK
            //else
                //return denied

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

        [Route("services/{token}")]
        [HttpGet]
        public IHttpActionResult GetAllServices(AuthenticationToken token)
        {
            //validate user
            //if user OK
            //open text file
            //return results and status OK
            //else
            //return denied

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

        [Route("publish/{token}/{service}")]
        [Route("publish")]
        [HttpPost]
        public IHttpActionResult PostNewService(AuthenticationToken token, Service service)
        {
            //validate user
            //if user OK
                //check body matches Service structure
                //if body is valid JSON
                    //append service to the service.json file
                    //return ok
                //else body is not valid JSON
                    //return BadRequest("Invalid Data")
            //else user is not OK
                //return BadRequest(Bad request JSON)

            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid Data");
            }
            ServiceList.PublishService(service);
            return Ok(new InvalidUserModel() { Status = "Denied", Reason = "Authentication Error" });
        }

        [Route("unpublish/{token}/{service}")]
        [Route("unpublish")]
        [HttpPost]
        public IHttpActionResult PostRemoveService(AuthenticationToken token, Service service)
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
    }
}
