using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ClientGUI
{
    /// <summary>
    /// Uses async to handle GUI elements
    /// </summary>
    internal class GUI_Utility
    {
        private static readonly SolidColorBrush redColor = new SolidColorBrush(Colors.Red);
        private static readonly SolidColorBrush greenColor = new SolidColorBrush(Colors.Green);

        private static string msgBox_msg = "";

        /** ----- Show Controls ----- **/

        public static async void ShowMessageBox(string msg)
        {
            msgBox_msg = msg;
            Task showTextBoxTask = new Task(ShowMessageBoxTask);
            showTextBoxTask.Start();
            await showTextBoxTask;
        }

        private static void ShowMessageBoxTask()
        {
            MessageBox.Show(msgBox_msg);
        }

        public static void ShowErrorStatusLabel(Label label, string msg)
        {
            label.Dispatcher.Invoke(new Action(() => label.Content = msg));
            label.Dispatcher.Invoke(new Action(() => label.Foreground = redColor));
            label.Dispatcher.Invoke(new Action(() => label.Visibility = Visibility.Visible));
        }

        public static void ShowStatusLabel(Label label, string msg)
        {
            label.Dispatcher.Invoke(new Action(() => label.Content = msg));
            label.Dispatcher.Invoke(new Action(() => label.Foreground = greenColor));
            label.Dispatcher.Invoke(new Action(() => label.Visibility = Visibility.Visible));
        }

        public static void ShowButton(Button button)
        {
            if (button.Visibility == Visibility.Hidden)
            {
                button.Dispatcher.Invoke(new Action(() => button.Visibility = Visibility.Visible));
            }
        }

        /** ----- Hide Controls ----- **/

        public static void HideControls(Control[] control)
        {
            foreach (Control controlInArray in control)
            {
                if (controlInArray.Visibility == Visibility.Visible)
                {
                    controlInArray.Dispatcher.Invoke(new Action(() => controlInArray.Visibility = Visibility.Hidden));
                }
            }
        }

        public static void HideStatusLabel(Label label)
        {
            if (label.Visibility == Visibility.Visible)
            {
                label.Dispatcher.Invoke(new Action(() => label.Visibility = Visibility.Hidden));
            }
        }

        public static void HideButton(Button button)
        {
            if (button.Visibility == Visibility.Visible)
            {
                button.Dispatcher.Invoke(new Action(() => button.Visibility = Visibility.Hidden));
            }
        }

        /** ----- Change Controls ----- **/
        public static void ChangeButtonContent(Button button, string text)
        {
            button.Dispatcher.Invoke(new Action(() => button.Content = text));
        }
    }
}
