﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Authenticator;
using ClientGUI;
using MathAppClassLibrary;
using Newtonsoft.Json;
using Registry.Models;
using RestSharp;
using System.ServiceModel;

namespace ServicePublisher
{
    /**
     * Contains the operations to Publish/Unpublish a Service
     */
    public class Publisher
    {
        private RestClient registryRestClient;
        private IAuth auth;

        private int userToken;

        public Publisher()
        {
            registryRestClient = new RestClient(ClientURL.REGISTRY_URL);
            auth = AuthenticatorSingleton.GetInstance();
            userToken = 0;
        }

        public void Register()
        {
            Console.WriteLine("> Register");
            
            Console.Write("  Enter username: ");
            string username = Console.ReadLine();
            Console.Write("  Enter password: ");
            string password = Console.ReadLine();

            auth.Register(username, password);
        }

        public void Login()
        {
            Console.WriteLine("> Login");
            
            Console.Write("  Enter username: ");
            string username = Console.ReadLine();
            Console.Write("  Enter password: ");
            string password = Console.ReadLine();

            userToken = auth.Login(username, password);
            if (User.TokenNotGenerated(userToken))
            {
                Console.WriteLine("\n>>> Error: User can't be found");
            }
            else
            {
                Console.WriteLine("\nSuccessfully Logged In");
            }
        }

        public void Publish()
        {
            Console.WriteLine("> Publishing a Service\nEnter service's:");

            try
            {
                // User input service's name, description, api endpoint, num operands, operand type
                Console.Write("    Name: ");
                string name = Console.ReadLine();
                Console.Write("    Description: ");
                string desc = Console.ReadLine();
                Console.Write("    API Endpoint: api/calculator/");
                string api_endpoint = "api/calculator/" + Console.ReadLine();
                Console.Write("    Number of operands: ");
                int numOperands = int.Parse(Console.ReadLine());
                Console.Write("    Operand Type: ");
                string operand_type = Console.ReadLine();

                Service service = new Service() { 
                    Name = name, 
                    Description = desc,
                    Endpoint = api_endpoint,
                    Operands = numOperands,
                    OperandType = operand_type
                };

                // Send to Registry service to publish - request with RestSharp and Serialize with NewtonSoft.JSON
                RestRequest restRequest = new RestRequest("api/registry/publish/{token}/{service}", Method.Post);
                restRequest.AddUrlSegment("token", userToken);
                restRequest.AddJsonBody(JsonConvert.SerializeObject(service));

                RestResponse restResponse = registryRestClient.Execute(restRequest);
                if (SuccessfulResponse(restResponse))
                {
                    Console.WriteLine("\nSuccessfully published '{0}' Service", name);
                }
            }
            catch (FormatException)
            {
                Console.WriteLine("\n>>> Error: Must enter a number for number of operands");
            }
        }

        public void Unpublish()
        {
            Console.WriteLine("> Unpublishing a Service");

            Console.Write("  Enter Service's Name: ");
            string name = Console.ReadLine();

            // The unpublish POST method of Web API will only use service's name to search for a Service to delete
            // "not_used" is needed otherwise, Unpublish() returns BadRequest("Invalid Data") because of empty fields
            Service service = new Service() {
                Name = name,
                Description = "not_used",
                Endpoint = "not_used",
                Operands = 0,
                OperandType = "not_used"
            };

            // Create Request and Response, serialize service object
            RestRequest restRequest = new RestRequest("api/registry/unpublish/{token}/{service}", Method.Post);
            restRequest.AddUrlSegment("token", userToken);
            restRequest.AddJsonBody(JsonConvert.SerializeObject(service));

            RestResponse restResponse = registryRestClient.Execute(restRequest);
            if (SuccessfulResponse(restResponse))
            {
                Console.WriteLine("\nSuccessfully unpublished '{0}' Service", name);
            }
        }

        private bool SuccessfulResponse(RestResponse restResponse)
        {
            if (restResponse.Content.Contains("Denied"))
            {
                InvalidUserModel response = JsonConvert.DeserializeObject<InvalidUserModel>(restResponse.Content);
                Console.WriteLine("\n>>> Error: Unable to publish service for the reason:");
                Console.WriteLine("\tReason = {0}", response.Reason);
                return false;
            }
            else if (!restResponse.IsSuccessful)
            {
                Console.WriteLine(">>> Error: " + restResponse.StatusCode + " --> " + restResponse.Content);
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
