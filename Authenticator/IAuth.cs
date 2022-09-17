using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Authenticator
{
    [ServiceContract]
    public interface IAuth
    {
        [OperationContract]
        string Register(string username, string password);

        [OperationContract]
        int Login(string username, string password);

        [OperationContract]
        string Validate(string username, string password);
    }
}
