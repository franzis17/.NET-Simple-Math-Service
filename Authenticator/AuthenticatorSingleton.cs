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

        public static string auth_URL = "net.tcp://localhost:8100/AuthenticatorService";

        public static IAuth GetInstance()
        {
            if (authServerInstance == null)
            {
                // Set URL and create the connection to the Authentication Server
                NetTcpBinding tcp = new NetTcpBinding();
                ChannelFactory<IAuth> authFactory = new ChannelFactory<IAuth>(tcp, auth_URL);
                authServerInstance = authFactory.CreateChannel();
            }
            return authServerInstance;
        }
    }
}
