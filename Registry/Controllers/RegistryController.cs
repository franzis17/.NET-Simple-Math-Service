using Registry.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json;

namespace Registry.Controllers
{
    [RoutePrefix("api/registry")]
    public class RegistryController : ApiController
    {

        public IEnumerable<Service> GetServices()
        {
            return ServiceList.AllServices();
        }

        [Route("services/{searchTerm}")]
        [Route("services")]
        [HttpGet]
        public IHttpActionResult GetServiceBySearch(string searchTerm)
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

        [Route("services")]
        [HttpGet]
        public IHttpActionResult GetAllServices()
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

        [Route("publish/{service}")]
        [Route("publish")]
        [HttpPost]
        public IHttpActionResult PostNewService(Service service)
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
            return Ok();
        }

        [Route("unpublish/{service}")]
        [Route("unpublish")]
        [HttpPost]
        public IHttpActionResult PostRemoveService(Service service)
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
