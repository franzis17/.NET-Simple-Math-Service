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
using System.Threading;

namespace ClientGUI
{
    /// <summary>
    /// Interaction logic for LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Page
    {
        private IAuth auth;

        private string username;
        private string password;

        public LoginPage()
        {
            InitializeComponent();
            Control[] control_arr = new Control[] { LoginStatusLabel, Login_ProgBar };
            GUI_Utility.HideControls(new Control[] { LoginStatusLabel, Login_ProgBar });

            auth = AuthenticatorSingleton.GetInstance();
        }

        private async void LoginUserBtn_Click(object sender, RoutedEventArgs e)
        {
            GUI_Utility.HideStatusLabel(LoginStatusLabel);
            GUI_Utility.ShowProgressBar(Login_ProgBar);

            username = LoginUserTxtBox.Text;
            password = LoginPassTxtBox.Text;

            Task<int> loginTask = new Task<int>(Login);
            loginTask.Start();

            try
            {
                MainWindow.userToken = await loginTask;
                if (User.TokenNotGenerated(MainWindow.userToken))
                {
                    GUI_Utility.ShowMessageBox("Error: User not found");
                }
                else
                {
                    GUI_Utility.ShowStatusLabel(LoginStatusLabel, "Successfully Logged In!");
                }
                GUI_Utility.HideProgressBar(Login_ProgBar);
            }
            catch (System.ServiceModel.EndpointNotFoundException)
            {
                GUI_Utility.ShowMessageBox("Error: Authenticator Server might not be online");
            }
        }

        private int Login()
        {
            Thread.Sleep(1000);
            return auth.Login(username, password);
        }
    }
}
