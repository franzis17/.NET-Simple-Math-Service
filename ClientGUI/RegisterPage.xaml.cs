using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Remoting.Channels;
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

using Authenticator;

namespace ClientGUI
{
    /// <summary>
    /// Interaction logic for RegisterPage.xaml
    /// </summary>
    public partial class RegisterPage : Page
    {
        private IAuth auth;

        public RegisterPage()
        {
            InitializeComponent();

            GUI_Utility.HideStatusLabel(RegStatusLabel);

            auth = AuthenticatorSingleton.GetInstance();
        }

        private void RegUserBtn_Click(object sender, RoutedEventArgs e)
        {
            GUI_Utility.HideStatusLabel(RegStatusLabel);
            try
            {
                string regStatus = auth.Register(RegUserTxtBox.Text, RegPassTxtBox.Text);
                GUI_Utility.ShowStatusLabel(RegStatusLabel, regStatus);
            }
            catch (System.ServiceModel.EndpointNotFoundException exc)
            {
                Console.WriteLine("Error: Authenticator Server might not be online.\n\t" + exc.Message);
            }
        }

        private void ShowStatusLabel(Label label, string msg)
        {
            label.Content = msg;
            label.Visibility = Visibility.Visible;
        }

        private void HideStatusLabel(Label label)
        {
            if (label.Visibility == Visibility.Visible)
            {
                label.Visibility = Visibility.Hidden;
            }
        }
    }
}
