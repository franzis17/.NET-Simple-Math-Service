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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private LoginPage loginPage;
        private RegisterPage registerPage;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void LoginBtn_Click(object sender, RoutedEventArgs e)
        {
            if (loginPage == null)
            {
                loginPage = new LoginPage();
            }
            MainFrame.Content = loginPage;
        }

        private void RegisterBtn_Click(object sender, RoutedEventArgs e)
        {
            if (registerPage == null)
            {
                registerPage = new RegisterPage();
            }
            MainFrame.Content = registerPage;
        }
    }
}
