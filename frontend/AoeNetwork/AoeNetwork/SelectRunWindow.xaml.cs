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
    /// Interaction logic for SelectRunWindow.xaml
    /// </summary>
    public partial class SelectRunWindow : Window
    {
        public SelectRunWindow()
        {
            InitializeComponent();
        }

        #region action for extend MainWindow (title bar, border, ...)
        private void coolform_titletext_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }
        private void coolform_close_btn_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void coolform_mini_btn_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.WindowState = System.Windows.WindowState.Minimized;
        }
        #endregion
        private void btnComplete_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnSelect_Click(object sender, RoutedEventArgs e)
        {
            // Create OpenFileDialog 
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            dlg.DefaultExt = ".exe";
            //dlg.Filter = "JPEG Files (*.jpeg)|*.jpeg|PNG Files (*.png)|*.png|JPG Files (*.jpg)|*.jpg|GIF Files (*.gif)|*.gif";

            // Display OpenFileDialog by calling ShowDialog method 
            Nullable<bool> result = dlg.ShowDialog();


            // Get the selected file name and display in a TextBox 
            if (result == true)
            {
                // Open document 
                string file = dlg.FileName;
                Properties.Settings.Default["RUN_URL"] = file;
                Properties.Settings.Default.Save();
                this.textboxLink.Text = file;
            }
        }

        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.Reload();
            this.textboxLink.Text = Properties.Settings.Default["RUN_URL"].ToString();
        }
    }
}
