using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ClientGUI
{
    internal class GUI_Utility
    {
        public static void ShowStatusLabel(Label label, string msg)
        {
            label.Content = msg;
            label.Visibility = Visibility.Visible;
        }
        
        public static void HideStatusLabel(Label label)
        {
            if (label.Visibility == Visibility.Visible)
            {
                label.Visibility = Visibility.Hidden;
            }
        }
    }
}
