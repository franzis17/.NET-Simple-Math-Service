using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

using MathAppClassLibrary;
using System.ServiceModel.Security.Tokens;

namespace Authenticator
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, UseSynchronizationContext = false)]

    /**
     * Implementation of Authenticator Interface
     */
    internal class ImplAuth : IAuth
    {
        private FileManager fileManager;

        public ImplAuth()
        {
            fileManager = new FileManager();
        }

        public string Register(string username, string password)
        {
            // save username and password in a local text file
            bool userInfoSaved = fileManager.SaveUserInfo(username, password);
            if (userInfoSaved)
            {
                return "Sucessfully Registered";
            }
            return "Error: Failed to save user info";
        }

        public int Login(string username, string password)
        {
            int token = 0;

            // Load the user info from the local text file
            List<User> userList = fileManager.LoadUserInfo();

            if (userList.Count != 0)
            {
                // If a match is found, create a token(random integer) then save it to tokens txt file
                foreach (User user in userList)
                {
                    if (user.Matches(username, password))
                    {
                        token = Token.CreateRandomInt();
                        fileManager.SaveToken(token);
                    }
                }
            }

            return token;
        }

        public string Validate(int token)
        {
            // If token is already generated, return “validated” else “not validated”
            return "validated";
        }

        /**
         * TBD: Internal function that deletes saved tokens every 'x' minutes
         * Console should ask the number of minutes for periodical clean-up
         *   - MUST use multithreading
         */
    }
}
