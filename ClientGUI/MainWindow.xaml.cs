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
        // User's log-in token
        public static int userToken = 0;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void LoginBtn_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Content = new LoginPage();
        }

        private void RegisterBtn_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Content = new RegisterPage();
        }

        private void ShowServicesBtn_Click(object sender, RoutedEventArgs e)
        {
            ShowServicesPage showServicesPage = new ShowServicesPage();
            MainFrame.Content = showServicesPage;
            showServicesPage.Start();
        }
    }
}
