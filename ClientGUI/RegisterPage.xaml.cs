using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading;
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

using Authenticator;

namespace ClientGUI
{
    /// <summary>
    /// Interaction logic for RegisterPage.xaml
    /// </summary>
    public partial class RegisterPage : Page
    {
        private IAuth auth;

        private string username;
        private string password;

        public RegisterPage()
        {
            InitializeComponent();
            GUI_Utility.HideControls(new Control[] { RegStatusLabel, Reg_ProgBar });

            auth = AuthenticatorSingleton.GetInstance();
        }

        private async void RegUserBtn_Click(object sender, RoutedEventArgs e)
        {
            GUI_Utility.HideStatusLabel(RegStatusLabel);
            GUI_Utility.ShowProgressBar(Reg_ProgBar);
            
            username = RegUserTxtBox.Text;
            password = RegPassTxtBox.Text;

            Task<string> registerTask = new Task<String>(Register);
            registerTask.Start();

            try
            {
                string regStatus = await registerTask;
                GUI_Utility.ShowStatusLabel(RegStatusLabel, regStatus);
                GUI_Utility.HideProgressBar(Reg_ProgBar);
            }
            catch (System.ServiceModel.EndpointNotFoundException)
            {
                GUI_Utility.ShowMessageBox("Error: Authenticator Server might not be online");
            }
        }

        private string Register()
        {
            Thread.Sleep(1000);
            return auth.Register(username, password);
        }
    }
}
