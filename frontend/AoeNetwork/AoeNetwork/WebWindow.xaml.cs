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
using System.Windows.Shapes;

namespace AoeNetwork
{
    /// <summary>
    /// Interaction logic for WebWindow.xaml
    /// </summary>
    public partial class WebWindow : Window
    {
        public WebWindow()
        {
            InitializeComponent();
        }

        public void OpenLink(string link)
        {
            if (link != "")
            {
                this.browser.Navigate(link);
            }
        }

        private void browser_Navigating(object sender, System.Windows.Navigation.NavigatingCancelEventArgs e)
        {
            this.Title = "Loading... Please wait...";
        }

        private void browser_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            this.Title = "GameTV";
        }
    }
}
