using System.ServiceModel;
using System.Windows;

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
