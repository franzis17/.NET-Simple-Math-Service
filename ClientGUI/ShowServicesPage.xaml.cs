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

using Newtonsoft.Json;
using Registry.Models;
using RestSharp;

namespace ClientGUI
{
    /// <summary>
    /// Interaction logic for ShowServicesPage.xaml
    /// </summary>
    public partial class ShowServicesPage : Page
    {
        private RestClient registryRestClient;
        private RestClient serviceProviderRestClient;

        private TextBox[] mathOp_TxtBoxes;

        private string searchTerm;
        private string serviceEndpoint;  // Endpoint of selected service in the list

        public ShowServicesPage()
        {
            InitializeComponent();
            registryRestClient = new RestClient(ClientURL.REGISTRY_URL);
            serviceProviderRestClient = new RestClient(ClientURL.SERVICEPROVIDER_URL);
        }

        public async void ShowAllServices()
        {
            Control[] controls = new Control[] { SvcStatusLabel, Calc_Btn };
            GUI_Utility.HideControls(controls);

            Task<List<Service>> taskGetAllServices = new Task<List<Service>>(GetAllServices);
            taskGetAllServices.Start();

            List<Service> services = await taskGetAllServices;
            if (services == null)
            {
                // TO DO: Update display error in the GUI
                Console.WriteLine("Service List is null");
            }
            else
            {
                ServicesListView.ItemsSource = services;
            }
        }

        /** Async Task for getting List of Services from Registry */
        private List<Service> GetAllServices()
        {
            List<Service> services = new List<Service>();

            RestRequest restRequest = new RestRequest("api/registry/services/{token}", Method.Get);
            restRequest.AddUrlSegment("token", MainWindow.userToken);
            RestResponse restResponse = registryRestClient.Execute(restRequest);

            if (restResponse.Content.Contains("Status"))
            {
                // Show user invalid response
                InvalidUserModel response = JsonConvert.DeserializeObject<InvalidUserModel>(restResponse.Content);
                string statusMsg = String.Format("Error: Failed to get services, Reason = {1}", response.Status, response.Reason);
                GUI_Utility.ShowMessageBox(statusMsg);
            }
            else if (!restResponse.IsSuccessful)
            {
                string errorMsg = "Error details: " + restResponse.StatusCode + " --> " + restResponse.Content;
                GUI_Utility.ShowMessageBox(errorMsg);
            }
            else
            {
                services = JsonConvert.DeserializeObject<List<Service>>(restResponse.Content);
            }

            return services;
        }

        private async void SearchServiceBtn_Click(object sender, RoutedEventArgs e)
        {
            GUI_Utility.HideStatusLabel(SvcStatusLabel);

            searchTerm = SearchTxtBox.Text;

            Task<List<Service>> searchServiceTask = new Task<List<Service>>(SearchService);
            searchServiceTask.Start();

            List<Service> services = await searchServiceTask;
            if (services == null)
            {
                // Display error
            }
            else
            {
                ServicesListView.ItemsSource = services;
            }
        }

        /** Async Task for Searching a service */
        private List<Service> SearchService()
        {
            RestRequest restRequest = new RestRequest("api/registry/services/{token}/{searchTerm}", Method.Get);
            restRequest.AddUrlSegment("token", MainWindow.userToken);
            restRequest.AddUrlSegment("searchTerm", searchTerm);
            RestResponse restResponse = registryRestClient.Execute(restRequest);

            if (restResponse.Content.Contains("Status"))
            {
                // Show User Invalid Response
                InvalidUserModel response = JsonConvert.DeserializeObject<InvalidUserModel>(restResponse.Content);
                string statusMsg = String.Format("Error: Failed to get services, Reason = {1}", response.Status, response.Reason);
                GUI_Utility.ShowMessageBox(statusMsg);
            }
            else if (!restResponse.IsSuccessful)
            {
                string errorMsg = "Error details: " + restResponse.StatusCode + " --> " + restResponse.Content;
                GUI_Utility.ShowMessageBox(errorMsg);
            }
            else
            {
                return JsonConvert.DeserializeObject<List<Service>>(restResponse.Content);
            }

            return null;
        }

        /**
         * Create dynamic amount of TextBox depending on the Service's number of operands that's selected in the list
         */
        private void ListViewItem_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            MathOp_WrapPanel.Children.Clear();  // Clear existing textboxes instantiated before

            var item = sender as ListViewItem;
            if (item != null)
            {
                // Grab the selected service and downcast it from 'Object' to 'Service' datatype
                Service service_item = item.Content as Service;
                serviceEndpoint = service_item.Endpoint;

                // Create the input boxes dynamically
                mathOp_TxtBoxes = new TextBox[service_item.Operands];
                for (int i = 0; i < service_item.Operands; i++)
                {
                    TextBox mathOp_TxtBox = new TextBox();
                    mathOp_TxtBox.Text = "Math " + (i+1);
                    mathOp_TxtBox.Margin = new Thickness(10,0,0,0);
                    MathOp_WrapPanel.Children.Add(mathOp_TxtBox);

                    // Add to Array of TextBoxes to obtain inputted numbers later
                    mathOp_TxtBoxes[i] = mathOp_TxtBox;
                }

                GUI_Utility.ShowButton(Calc_Btn);
            }
        }

        /** Sends the inputted numbers to ServiceProvider to do the calculation */
        private void Calc_Btn_Click(object sender, RoutedEventArgs e)
        {
            int[] math_inputs = GetNumInputsFromTxtBox();

            // Send inputs to the appropriate service
            if (math_inputs.Length != 0)
            {
                RestRequest restRequest = new RestRequest("api/calculator");
            }
        }

        private int[] GetNumInputsFromTxtBox()
        {
            int[] math_inputs = new int[mathOp_TxtBoxes.Length];

            try
            {
                for (int i = 0; i < mathOp_TxtBoxes.Length; i++)
                {
                    math_inputs[i] = Int32.Parse(mathOp_TxtBoxes[i].Text);
                }
            }
            catch (FormatException)
            {
                GUI_Utility.ShowMessageBox("Error: Please input numbers to calculate");
            }

            return math_inputs;
        }
    }
}
