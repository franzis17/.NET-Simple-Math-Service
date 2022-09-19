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

        /** 
         * Save username and password in a local text file
         *   - RETURN "Successfully Registered" if registering user is successful
         */
        public string Register(string username, string password)
        {
            bool userInfoSaved = fileManager.SaveUserInfo(username, password);
            if (userInfoSaved)
            {
                return "Sucessfully Registered";
            }
            return "Error: Failed to save user info";
        }

        /**
         * Checks txt file if username & password exists
         *   - RETURN token if a user exists
         */
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

        /**
         * Validate if param{token} is already generated in the list
         *   - RETURN “validated” if token has been generated already else “not validated”
         */
        public string Validate(int token)
        {
            List<Token> tokenList = fileManager.LoadTokenList();
            foreach (Token listToken in tokenList)
            {
                if (token == listToken.random_int)
                {
                    return "validated";
                }
            }
            return "not validated";
        }

        /**
         * TBD: Internal function that deletes saved tokens every 'x' minutes
         * Console should ask the number of minutes for periodical clean-up
         *   - MUST use multithreading
         */
    }
}
