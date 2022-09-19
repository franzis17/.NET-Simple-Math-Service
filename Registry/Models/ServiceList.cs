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
    }
}