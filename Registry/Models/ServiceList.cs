using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace Registry.Models
{
    public class ServiceList
    {
        private static readonly JsonSerializerSettings _options = new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore };

        public static List<Service> AllServices()
        {
            List<Service> services = new List<Service>();

            //Get the working dir of the instance
            string path = System.Web.HttpContext.Current.Request.MapPath("~\\App_Data\\services.json");
            //locate the file in the App_Data dir, inside file services.json

            //open the file and read the contents
            using (StreamReader r = new StreamReader(path))
            {
                //Parse the json
                string json = r.ReadToEnd();
                //Deserialize the json using NewtonSoft into a list of Service objects
                services = JsonConvert.DeserializeObject<List<Service>>(json);
                r.Close();
            }
            return services;
        }

        public static void PublishService(Service newService)
        {
            //get current services
            var services = AllServices();

            //add service to the list of services
            services.Add(newService);
            //get path to file
            string path = System.Web.HttpContext.Current.Request.MapPath("~\\App_Data\\services.json");

            //serialize the data
            var jsonString = JsonConvert.SerializeObject(services, Formatting.Indented, _options);
            File.WriteAllText(path, jsonString);
        }

        public static bool DeleteService(Service service)
        {
            var services = AllServices();
            var temp = services[0];
            bool found = false;

            foreach (Service ii in services)
            {
                if (ii.Name.Equals(service.Name))
                {
                    temp = ii;
                    found = true;
                    break;
                }
            }

            if (found)
            {
                if (services.Remove(temp))
                {
                    string path = System.Web.HttpContext.Current.Request.MapPath("~\\App_Data\\services.json");

                    //create json from list of services
                    var jsonString = JsonConvert.SerializeObject(services, Formatting.Indented, _options);
                    //write to file
                    File.WriteAllText(path, jsonString);

                    return true;
                }
            }
            return false;
        }
    }
}