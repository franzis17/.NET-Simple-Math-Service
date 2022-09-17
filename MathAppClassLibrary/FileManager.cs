using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathAppClassLibrary
{
    /**
     * For saving/loading user info in a file
     */
    public class FileManager
    {
        private string accountFilePath;
        private string tokenFilePath;

        public FileManager()
        {
            accountFilePath = "accounts.txt";
            tokenFilePath = "tokens.txt";
        }

        public void SaveUserInfo(string username, string password)
        {
            try
            {
                StreamWriter streamWriter = null;

                // Create new file if file doesn't exist yet else get existing one
                if (!File.Exists(accountFilePath))
                {
                    streamWriter = File.CreateText(accountFilePath);
                }
                else
                {
                    streamWriter = File.AppendText(accountFilePath);
                }

                if (streamWriter != null)
                {
                    streamWriter.WriteLine(username + "+" + password);
                    streamWriter.Close();
                }
            }
            catch (IOException io_e)
            {
                Console.WriteLine("IOException occured with message: " + io_e);
            }
        }

        public List<User> LoadUserInfo()
        {
            List<User> listUser = new List<User>();

            Console.WriteLine("Reading file: " + accountFilePath + "\n");

            try
            {
                if (!File.Exists(accountFilePath))
                {
                    Console.WriteLine("Error: Cannot read file because " + accountFilePath + " doesn't exist.");
                }
                else
                {
                    string fileStr = "";
                    StreamReader streamReader = File.OpenText(accountFilePath);

                    while ((fileStr = streamReader.ReadLine()) != null)
                    {
                        // Separate username and password for each line in the file
                        char delimiter = '+';
                        string[] line = fileStr.Split(delimiter);

                        // Add and Create new user with the read username and password
                        string username = line[0];
                        string password = line[1];
                        listUser.Add(new User(username, password));
                    }
                }
            }
            catch (IOException io_exc)
            {
                Console.WriteLine("IOException occured with message: " + io_exc);
            }

            return listUser;
        }



        /** ---- Test Functions ---- */

        public static void TestWriteToFile(FileManager fileManager)
        {
            Console.WriteLine("Enter a username: ");
            string username = Console.ReadLine();

            Console.WriteLine("Enter a password: ");
            string password = Console.ReadLine();

            fileManager.SaveUserInfo(username, password);
        }

        public static void TestReadFile(FileManager fileManager)
        {
            List<User> listUser = fileManager.LoadUserInfo();

            Console.WriteLine("File contains:\n");
            for (int i = 0; i < listUser.Count; i++)
            {
                Console.WriteLine("User " + (i + 1)
                    + "\n\tUsername: " + listUser[i].username
                    + " | Password: " + listUser[i].password
                );
            }
        }
    }
}
