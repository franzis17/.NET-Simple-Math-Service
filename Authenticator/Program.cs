using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Authenticator
{
    /**
     * A .NET WCF/Remoting Server that aims to authenticate a user whenever the user invokes a service of ServiceProvider
     */
    internal class Program
    {
        private IAuth authServer;

        static void Main(string[] args)
        {
            Console.WriteLine("Initiating Authenticator Server...");

            NetTcpBinding tcp = new NetTcpBinding();
            ServiceHost host = new ServiceHost(typeof(ImplAuth));

            host.AddServiceEndpoint(typeof(IAuth), tcp, "net.tcp://0.0.0.0:8100/AuthenticationService");
            host.Open();

            Console.WriteLine("Server is online");
            Console.ReadLine();

            host.Close();
        }
    }
}
