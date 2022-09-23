using Authenticator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using MathAppClassLibrary;

namespace ClientGUI
{
    /// <summary>
    /// Interaction logic for LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Page
    {
        private IAuth auth;

        // TextBox values
        private string username;
        private string password;

        public LoginPage()
        {
            InitializeComponent();
            
            GUI_Utility.HideStatusLabel(LoginStatusLabel);
            
            auth = AuthenticatorSingleton.GetInstance();
        }

        private async void LoginUserBtn_Click(object sender, RoutedEventArgs e)
        {
            GUI_Utility.HideStatusLabel(LoginStatusLabel);

            username = LoginUserTxtBox.Text;
            password = LoginPassTxtBox.Text;

            Task<int> loginTask = new Task<int>(Login);
            loginTask.Start();

            try
            {
                MainWindow.userToken = await loginTask;
                if (User.TokenNotGenerated(MainWindow.userToken))
                {
                    GUI_Utility.ShowErrorStatusLabel(LoginStatusLabel, "Error: User not found");
                }
                else
                {
                    GUI_Utility.ShowStatusLabel(LoginStatusLabel, "Successfully Logged In!");
                }
            }
            catch (System.ServiceModel.EndpointNotFoundException)
            {
                string errorMsg = "Error: Authenticator Server might not be online.\n\t";
                GUI_Utility.ShowErrorStatusLabel(LoginStatusLabel, errorMsg);
            }
        }

        private int Login()
        {
            return auth.Login(username, password);
        }
    }
}
