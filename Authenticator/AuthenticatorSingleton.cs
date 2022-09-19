using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Authenticator
{
    public class AuthenticatorSingleton
    {
        /** Singleton instance */
        private static IAuth authServerInstance  = null;

        public static IAuth GetInstance()
        {
            if (authServerInstance == null)
            {
                // Set URL and create the connection for the BankBusinessServer to communicate with the BankServer
                NetTcpBinding tcp = new NetTcpBinding();
                string URL = "net.tcp://localhost:8100/AuthenticatorService";
                ChannelFactory<IAuth> authFactory = new ChannelFactory<IAuth>(tcp, URL);
                authServerInstance = authFactory.CreateChannel();
            }
            return authServerInstance;
        }
    }
}
