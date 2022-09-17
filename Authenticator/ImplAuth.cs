using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Authenticator
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, UseSynchronizationContext = false)]

    internal class ImplAuth : IAuth
    {
        public string Register(string username, string password)
        {
            // save username and password in a local text file

            // return "Successfully registered" if successful
            return "Sucessfully Registered";
        }

        public int Login(string username, string password)
        {
            int rand_int = 123;

            // Load the user info from the local text file
            
            // If a match is found, create a token(random integer) then save it to another local text file
            

            // return token to the actor who called the function
            return rand_int;
        }

        public string Validate(string username, string password)
        {
            // If token is already generated, return “validate” else “not validated”
            return "Not Validated";
        }
    }
}
