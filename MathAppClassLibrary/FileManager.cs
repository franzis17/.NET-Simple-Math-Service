using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathAppClassLibrary
{
    /**
     * Save/Load/Delete user info in a file used by Authenticator
     * Files are located in -> DC_MathServicesApp\{Client}\bin\Debug
     *   - Client = console app that invoked FileManager functions (e.g. Authenticator)
     */
    public class FileManager
    {
        private string userFilePath;
        private string tokenFilePath;

        public FileManager()
        {
            userFilePath = "users.txt";
            tokenFilePath = "tokens.txt";
        }

        /** ---- User Methods ---- **/

        /**
         * User info are saved in a text file with the format "username+password"
         * File is appended when a new user info has to be saved
         * Note: Doesn't account for duplicated user info
         *   - RETURN true if the file is successfully saved
         */
        public bool SaveUserInfo(string username, string password)
        {
            bool isFileSaved = false;
            
            try
            {
                StreamWriter streamWriter = null;

                // Create new file if file doesn't exist yet else get existing one
                if (!File.Exists(userFilePath))
                {
                    streamWriter = File.CreateText(userFilePath);
                }
                else
                {
                    streamWriter = File.AppendText(userFilePath);
                }

                if (streamWriter != null)
                {
                    streamWriter.WriteLine(username + "+" + password);
                    streamWriter.Close();
                    isFileSaved = true;
                }
            }
            catch (IOException io_e)
            {
                Console.WriteLine("Error: IOException occured with message -> " + io_e);
            }

            return isFileSaved;
        }

        /**
         *   - RETURN List of Users saved in the local file
         */
        public List<User> LoadUserInfo()
        {
            List<User> userList = new List<User>();

            try
            {
                if (!File.Exists(userFilePath))
                {
                    Console.WriteLine("Error: Cannot read file because " + userFilePath + " doesn't exist.");
                }
                else
                {
                    StreamReader streamReader = File.OpenText(userFilePath);
                    string fileStr = "";

                    while ((fileStr = streamReader.ReadLine()) != null)
                    {
                        // Separate username and password for each line in the file
                        char delimiter = '+';
                        string[] line = fileStr.Split(delimiter);

                        // Add and Create new user with the read username and password
                        string username = line[0];
                        string password = line[1];
                        userList.Add(new User(username, password));
                    }

                    streamReader.Close();
                }
            }
            catch (IOException io_exc)
            {
                Console.WriteLine("Error: IOException occured with message -> " + io_exc);
            }

            return userList;
        }

        public void DeleteUserFile()
        {
            File.Delete(userFilePath);
        }


        /** ---- Token Methods ---- **/


        /**
         * Note: Doesn't account for duplicated tokens in the file
         */
        public void SaveToken(int token)
        {
            try
            {
                StreamWriter streamWriter = null;

                // Create new file if file doesn't exist yet else get existing one
                if (!File.Exists(tokenFilePath))
                {
                    streamWriter = File.CreateText(tokenFilePath);
                }
                else
                {
                    streamWriter = File.AppendText(tokenFilePath);
                }

                if (streamWriter != null)
                {
                    streamWriter.WriteLine(token.ToString());
                    streamWriter.Close();
                }
            }
            catch (IOException io_e)
            {
                Console.WriteLine("Error: IOException occured with message -> " + io_e);
            }
        }

        public List<Token> LoadTokenList()
        {
            List<Token> tokenList = new List<Token>();

            try
            {
                if (!File.Exists(tokenFilePath))
                {
                    Console.WriteLine("Error: Cannot read file because " + tokenFilePath + " doesn't exist.");
                }
                else
                {
                    StreamReader streamReader = File.OpenText(tokenFilePath);
                    string fileStr = "";

                    while ((fileStr = streamReader.ReadLine()) != null)
                    {
                        try
                        {
                            tokenList.Add(new Token(int.Parse(fileStr)));
                        }
                        catch (FormatException format_exc)
                        {
                            Console.WriteLine("Error: Expected an integer from token file. Message -> " + format_exc);
                        }
                    }

                    streamReader.Close();
                }
            }
            catch (IOException io_exc)
            {
                Console.WriteLine("Error: IOException occured with message -> " + io_exc);
            }

            return tokenList;
        }

        public void DeleteTokenFile()
        {
            File.Delete(tokenFilePath);
        }
    }
}
