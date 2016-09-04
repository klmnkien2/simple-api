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
    /// Interaction logic for AddFriendWindow.xaml
    /// </summary>
    public partial class AddFriendWindow : Window
    {
        public AddFriendWindow()
        {
            InitializeComponent();
        }

        ChatController controller;
        public void SetController(ChatController controller)
        {
            this.controller = controller;
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

        private void formObject_KeyDown(KeyEventArgs e)
        {
            if (this.username.Text.Trim() == "")
            {
                MessageBox.Show("User name must be entered.");
            }
            if (e.Key == Key.Enter)
            {
                controller.AddFriend(this.username.Text, this.message.Text);
            }
        }

        private void submit_Click(object sender, RoutedEventArgs e)
        {
            if (this.username.Text.Trim() == "")
            {
                MessageBox.Show("User name must be entered.");
            }
            controller.AddFriend(this.username.Text, this.message.Text);

        }

        private void message_KeyDown(object sender, KeyEventArgs e)
        {
            formObject_KeyDown(e);
        }

        private void username_KeyDown(object sender, KeyEventArgs e)
        {
            formObject_KeyDown(e);
        }
    }
}
