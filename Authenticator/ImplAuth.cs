using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using MathAppClassLibrary;
using System.Windows.Threading;
using System.Runtime.CompilerServices;
using System.Timers;

namespace Authenticator
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, UseSynchronizationContext = false)]

    /**
     * Implementation of Authenticator Interface
     */
    internal class ImplAuth : IAuth
    {
        private FileManager fileManager;

        private static int clear_token_interval = 0;

        public ImplAuth()
        {
            fileManager = new FileManager();
        }

        /**
         * Checks txt file if username and password exists
         *   - RETURN token (random int ranging from 1 to 50000) if a user exists. If user can't be found, it returns 0
         */
        public int Login(string username, string password)
        {
            int token = 0;

            // Load the user info from the local text file
            List<User> userList = FileManager.LoadUserInfo();

            if (userList.Count != 0)
            {
                // If a match is found, create a token(random integer) then save it to tokens.txt file
                foreach (User user in userList)
                {
                    if (user.Matches(username, password))
                    {
                        token = Token.GenerateRandomToken();
                        FileManager.SaveToken(token);
                    }
                }
            }

            return token;
        }

        /** 
         * Save username and password in a local text file
         *   - RETURN "Successfully Registered" if registering user is successful
         */
        public string Register(string username, string password)
        {
            bool userInfoSaved = FileManager.SaveUserInfo(username, password);
            if (userInfoSaved)
            {
                return "Sucessfully Registered";
            }
            return "Error: Failed to save user info";
        }

        /**
         * Validate if param{token} is already generated in the list
         *   - RETURN “validated” IF token has been generated already ELSE “not validated”
         */
        public string Validate(int token)
        {
            List<Token> tokenList = FileManager.LoadTokenList();
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
         * Internal function that deletes saved tokens every 'x' minutes, uses multithreading
         */
        public static void ClearTokens(int interval)
        {
            clear_token_interval = interval;
            Task clearTokensTask = new Task(ClearTokensTask);
            clearTokensTask.Start();
        }

        private static void ClearTokensTask()
        {
            Timer timer = new Timer();
            timer.Interval = TimeSpan.FromMinutes(clear_token_interval).TotalMilliseconds;
            timer.Elapsed += Timer_Tick;
            timer.AutoReset = true;
            timer.Enabled = true;
        }

        /** Clear tokens every tick */
        private static void Timer_Tick(object sender, EventArgs e)
        {
            Console.WriteLine("Clearing tokens...");
            FileManager.ClearTokens();
        }
    }
}
