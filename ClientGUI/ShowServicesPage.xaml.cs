using System;
using System.Collections.Generic;
using System.Linq;
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
        private int[] numInputsArr;

        private string searchTerm;
        private string serviceEndpoint;  // Endpoint of selected service in the list

        public ShowServicesPage()
        {
            InitializeComponent();
            registryRestClient = new RestClient(ClientURL.REGISTRY_URL);
            serviceProviderRestClient = new RestClient(ClientURL.SERVICEPROVIDER_URL);
        }

        public void Start()
        {
            Control[] control_arr = new Control[]
            {
                SvcStatusLabel, Calc_Btn, Answer_Label, Search_ProgBar, Calc_ProgBar
            };
            GUI_Utility.HideControls(control_arr);
            ShowAllServices();
        }

        public async void ShowAllServices()
        {
            GUI_Utility.HideControls(new Control[] { Answer_Label });
            GUI_Utility.ShowProgressBar(Search_ProgBar);

            Task<List<Service>> taskGetAllServices = new Task<List<Service>>(GetAllServices);
            taskGetAllServices.Start();

            List<Service> services = await taskGetAllServices;
            GUI_Utility.HideProgressBar(Search_ProgBar);
            if (services != null)
            {
                ServicesListView.ItemsSource = services;
            }
        }

        /** Async Task for getting List of Services from Registry */
        private List<Service> GetAllServices()
        {
            Thread.Sleep(1000);
            RestRequest restRequest = new RestRequest("api/registry/services/{token}", Method.Get);
            restRequest.AddUrlSegment("token", MainWindow.userToken);
            RestResponse restResponse = registryRestClient.Execute(restRequest);

            if (SuccessfulResponse(restResponse))
            {
                return JsonConvert.DeserializeObject<List<Service>>(restResponse.Content);
            }    
            return null;
        }

        private async void SearchServiceBtn_Click(object sender, RoutedEventArgs e)
        {
            MathOp_WrapPanel.Children.Clear();
            GUI_Utility.HideStatusLabel(SvcStatusLabel);
            GUI_Utility.ShowProgressBar(Search_ProgBar);

            searchTerm = SearchTxtBox.Text;
            if (searchTerm == "")
            {
                ShowAllServices();
            }
            else
            {
                Task<List<Service>> searchServiceTask = new Task<List<Service>>(SearchService);
                searchServiceTask.Start();

                List<Service> services = await searchServiceTask;
                GUI_Utility.HideProgressBar(Search_ProgBar);
                if (services != null)
                {
                    ServicesListView.ItemsSource = services;
                }
            }
        }

        /** Async Task for Searching a service */
        private List<Service> SearchService()
        {
            Thread.Sleep(1000);
            RestRequest restRequest = new RestRequest("api/registry/services/{token}/{searchTerm}", Method.Get);
            restRequest.AddUrlSegment("token", MainWindow.userToken);
            restRequest.AddUrlSegment("searchTerm", searchTerm);
            RestResponse restResponse = registryRestClient.Execute(restRequest);

            if (SuccessfulResponse(restResponse))
            {
                return JsonConvert.DeserializeObject<List<Service>>(restResponse.Content);
            }
            return null;
        }

        /** Create dynamic amount of TextBox depending on the Service's number of operands that's selected in the list */
        private void ListViewItem_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            MathOp_WrapPanel.Children.Clear();  // Clear existing textboxes instantiated before
            GUI_Utility.HideStatusLabel(Answer_Label);

            var item = sender as ListViewItem;
            if (item != null)
            {
                // Grab the selected service and typecast it from ListViewItem to 'Service' datatype
                Service service_item = item.Content as Service;
                serviceEndpoint = service_item.Endpoint;

                // Create the input boxes dynamically
                mathOp_TxtBoxes = new TextBox[service_item.Operands];
                for (int i = 0; i < service_item.Operands; i++)
                {
                    TextBox num_TxtBox = new TextBox();
                    num_TxtBox.Width = 50;
                    num_TxtBox.Margin = new Thickness(20,20,0,0);
                    MathOp_WrapPanel.Children.Add(num_TxtBox);

                    mathOp_TxtBoxes[i] = num_TxtBox;
                }

                GUI_Utility.ShowButton(Calc_Btn);
            }
        }

        /** Calculate and Show the answer of the operation obtained from Service Provider */
        private async void Calc_Btn_Click(object sender, RoutedEventArgs e)
        {
            GUI_Utility.ShowProgressBar(Calc_ProgBar);

            numInputsArr = GetNumInputsFromTxtBox();
            if (numInputsArr != null)
            {
                Task<int> calculateNumbersTask = new Task<int>(CalculateNumbers);
                calculateNumbersTask.Start();

                int answer = await calculateNumbersTask;

                Answer_Label.Content = "Answer = " + answer;
                Answer_Label.Visibility = Visibility.Visible;
                GUI_Utility.HideProgressBar(Calc_ProgBar);
            }

            GUI_Utility.HideProgressBar(Calc_ProgBar);
        }

        /// <summary>
        /// Async Task for calculating the inputted numbers
        /// </summary>
        /// <returns> (int)answer obtained from Service Provider </returns>
        private int CalculateNumbers()
        {
            Thread.Sleep(1000);
            RestRequest restRequest = new RestRequest(serviceEndpoint, Method.Get);
            restRequest.AddParameter("token", MainWindow.userToken);

            if (numInputsArr.Length != 0)
            {
                for (int i = 0; i < numInputsArr.Length; i++)
                {
                    restRequest.AddParameter(String.Format("num{0}", (i+1)), numInputsArr[i]);
                }

                RestResponse restResponse = serviceProviderRestClient.Execute(restRequest);
                if (SuccessfulResponse(restResponse))
                {
                    return JsonConvert.DeserializeObject<int>(restResponse.Content);
                }
            }
            return 0;
        }

        /** Returns an Array of int obtained from TextBoxes to pass as parameters to the math service */
        private int[] GetNumInputsFromTxtBox()
        {
            int[] numInputsArr = new int[mathOp_TxtBoxes.Length];

            try
            {
                for (int i = 0; i < mathOp_TxtBoxes.Length; i++)
                {
                    numInputsArr[i] = Int32.Parse(mathOp_TxtBoxes[i].Text);
                }
            }
            catch (FormatException)
            {
                GUI_Utility.ShowMessageBox("Error: Please input numbers to calculate");
                return null;
            }

            return numInputsArr;
        }

        /** Checks whether or not response is a failure, if it is, show the user error details */
        private bool SuccessfulResponse(RestResponse restResponse)
        {
            if (!restResponse.IsSuccessful)
            {
                string errorMsg = "Error: " + restResponse.StatusCode + " --> " + restResponse.Content;
                GUI_Utility.ShowMessageBox(errorMsg);
                return false;
            }
            else if (restResponse.Content.Contains("Status"))
            {
                // Show User Invalid Response
                InvalidUserModel response = JsonConvert.DeserializeObject<InvalidUserModel>(restResponse.Content);
                string statusMsg = String.Format("Error: Failed to get services, Reason = {1}", response.Status, response.Reason);
                GUI_Utility.ShowMessageBox(statusMsg);
                return false;
            }
            return true;
        }
    }
}
