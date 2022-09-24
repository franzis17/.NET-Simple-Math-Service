using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using Authenticator;
using Newtonsoft.Json;
using MathAppClassLibrary;
using RestSharp;
using System.ServiceModel;
using static System.Net.WebRequestMethods;

namespace ServicePublisher
{
    internal class Program
    {
        private const int EXIT_OPTION = 5;

        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to the Math Service Publisher Console App!");

            Publisher publisher = new Publisher();
            ShowOptions(publisher);
        }

        private static void ShowOptions(Publisher publisher)
        {
            int choice = 0;

            // Loop til user enters exit option (5)
            do
            {
                try
                {
                    Console.WriteLine("\nPlease choose a number from the following options:\n"
                        + "  1. Register\n"
                        + "  2. Login\n"
                        + "  3. Publish a Service\n"
                        + "  4. Unpublish a Service\n"
                        + "  5. Exit\n"
                    );

                    choice = int.Parse(Console.ReadLine());
                    switch (choice)
                    {
                        case 1:
                            publisher.Register();
                            break;
                        case 2:
                            publisher.Login();
                            break;
                        case 3:
                            publisher.Publish();
                            break;
                        case 4:
                            publisher.Unpublish();
                            break;
                        default:
                            Console.WriteLine("\n>>> Error: {0} is not a part of the option\n", choice);
                            break;
                    }
                }
                catch (FormatException)
                {
                    Console.WriteLine("\n>>> Error: Must enter a number");
                }
                catch (EndpointNotFoundException)
                {
                    Console.WriteLine("\n>>> Error: Authenticator might not be online");
                }
            } while (choice != EXIT_OPTION);
        }

    }
}
