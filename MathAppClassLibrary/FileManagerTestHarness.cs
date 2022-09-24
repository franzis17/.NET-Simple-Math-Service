using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathAppClassLibrary
{
    public class FileManagerTestHarness
    {

        public FileManagerTestHarness()
        {
        }

        /** Test any user methods of FileManager */
        public static void TestUserInfo()
        {
            FileManagerTestHarness fileManagerTester = new FileManagerTestHarness();
            fileManagerTester.TestSaveUserInfo();
            fileManagerTester.TestLoadUserInfo();
            fileManagerTester.TestDeleteUserFile();
            fileManagerTester.TestLoadUserInfo();
        }

        /** Test any token methods of FileManager */
        public static void TestTokens()
        {
            FileManagerTestHarness fileManagerTester = new FileManagerTestHarness();
            fileManagerTester.TestSaveToken();
            fileManagerTester.TestLoadTokenList();
        }

        /** ---- Test User Methods ---- **/

        public void TestSaveUserInfo()
        {
            Console.WriteLine("> Saving User Info to file");
            Console.WriteLine("Enter a username: ");
            string username = Console.ReadLine();

            Console.WriteLine("Enter a password: ");
            string password = Console.ReadLine();

            FileManager.SaveUserInfo(username, password);
        }

        public void TestLoadUserInfo()
        {
            Console.WriteLine("> Loading User Info from file");
            List<User> userList = FileManager.LoadUserInfo();

            if (userList.Count != 0)
            {
                // Show file contains
                Console.WriteLine("File contains:\n");
                for (int i = 0; i < userList.Count; i++)
                {
                    Console.WriteLine("User " + (i+1)
                        + "\n\tUsername: " + userList[i].username
                        + " | Password: " + userList[i].password + "\n"
                    );
                }
            }
        }

        public void TestDeleteUserFile()
        {
            Console.WriteLine("> Deleting User File");
            FileManager.DeleteUserFile();
        }


        /** ---- Test Token Methods ---- **/


        public void TestSaveToken()
        {
            Console.WriteLine("> Saving Token to file");
            FileManager.SaveToken(Token.GenerateRandomToken());
        }

        public void TestLoadTokenList()
        {
            Console.WriteLine("> Loading Tokens from file"
                + "\nToken File contains:\n"
            );

            List<Token> tokenList = FileManager.LoadTokenList();

            // Display tokens in the file
            for (int i = 0; i < tokenList.Count; i++)
            {
                Console.WriteLine("\tToken " + (i+1) + " = " + tokenList[i].random_int.ToString());
            }
        }

        public void TestDeleteTokenFile()
        {
            Console.WriteLine("> Deleting Tokens from file");
            FileManager.DeleteTokenFile();
        }
    }
}
