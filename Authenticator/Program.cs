using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

using MathAppClassLibrary;
using System.Windows.Threading;
using System.Runtime.CompilerServices;
using System.IO;

namespace Authenticator
{
    /**
     * A .NET WCF/Remoting Server that aims to authenticate a user whenever the user invokes a service of ServiceProvider
     */
    internal class Program
    {
        static void Main(string[] args)
        {
            bool continueLoop = false;
            do
            {
                try
                {
                    Console.Write("\nEnter minute interval for clearing tokens: ");
                    int clear_token_interval = Int32.Parse(Console.ReadLine());
                    ImplAuth.ClearTokens(clear_token_interval);
                    continueLoop = false;
                }
                catch (FormatException)
                {
                    Console.WriteLine("\n>>> Error: Please enter a number");
                    continueLoop = true;
                }
            } while (continueLoop);

            Console.WriteLine("Initiating Authenticator Server...");

            NetTcpBinding tcp = new NetTcpBinding();
            ServiceHost host = new ServiceHost(typeof(ImplAuth));

            host.AddServiceEndpoint(typeof(IAuth), tcp, AuthenticatorSingleton.auth_URL);
            host.Open();
            Console.WriteLine("Server is online");

            Console.WriteLine("Press enter to stop authentication server");
            Console.ReadLine();
            host.Close();
        }
    }
}
