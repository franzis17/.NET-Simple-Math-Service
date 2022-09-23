using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathAppClassLibrary
{
    /**
     * Used for searching the local text file for a user
     */
    public class User
    {
        public string username;
        public string password;

        public User(string username, string password)
        {
            this.username = username;
            this.password = password;
        }

        public bool Matches(string username, string password)
        {
            return this.username.Equals(username) && this.password.Equals(password);
        }

        /** 
         * If token is 0, that means token was not generated due to the user being unable to login
         * likely because user wasn't found in the users.txt file
         */
        public static bool TokenNotGenerated(int token)
        {
            return token == 0;
        }
    }
}
