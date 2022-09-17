using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathAppClassLibrary
{
    public class FileManagerTestHarness
    {
        private FileManager fileManager;

        public FileManagerTestHarness()
        {
            fileManager = new FileManager();
        }

        public static void Test()
        {
            FileManagerTestHarness fileManagerTester = new FileManagerTestHarness();
            fileManagerTester.TestSaveUserInfo();
            fileManagerTester.TestLoadUserInfo();
            fileManagerTester.TestDeleteUserFile();
        }

        public void TestSaveUserInfo()
        {
            Console.WriteLine("Enter a username: ");
            string username = Console.ReadLine();

            Console.WriteLine("Enter a password: ");
            string password = Console.ReadLine();

            fileManager.SaveUserInfo(username, password);
        }

        public void TestLoadUserInfo()
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

        public void TestDeleteUserFile()
        {
            fileManager.DeleteUserFile();
        }
    }
}
