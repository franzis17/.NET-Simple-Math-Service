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

namespace ClientGUI
{
    /// <summary>
    /// Interaction logic for LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Page
    {
        private IAuth auth;

        public LoginPage()
        {
            InitializeComponent();
            
            GUI_Utility.HideStatusLabel(LoginStatusLabel);

            auth = AuthenticatorSingleton.GetInstance();
        }

        private void LoginUserBtn_Click(object sender, RoutedEventArgs e)
        {
            GUI_Utility.HideStatusLabel(LoginStatusLabel);
            try
            {
                int token = auth.Login(LoginUserTxtBox.Text, LoginPassTxtBox.Text);
                if (token != 0)
                {
                    GUI_Utility.ShowStatusLabel(LoginStatusLabel, "Successfully Logged In!");
                }
                else
                {
                    GUI_Utility.ShowStatusLabel(LoginStatusLabel, "User not found");
                }

                // TO DO: token must be saved in program memory (Singleton Pseudo-Database ?)
                
            }
            catch (System.ServiceModel.EndpointNotFoundException exc)
            {
                Console.WriteLine("Error: Authenticator Server might not be online.\n\t" + exc.Message);
            }
        }
    }
}
