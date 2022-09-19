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
    }
}
