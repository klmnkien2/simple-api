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
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
            _controller = new AuthenController(this);
            _controller.LoadView();
        }

        #region action for extend MainWindow (title bar, border, ...)
        private void coolform_titletext_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }
        private void coolform_close_btn_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.Hide();
            this.Close();
            Application.Current.Shutdown();
        }

        private void coolform_mini_btn_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.WindowState = System.Windows.WindowState.Minimized;
        }
        #endregion

        #region setup controller and share fields
        private int state = 1;
        AuthenController _controller;

        public void ClearForm()
        {
            this.usernameTextbox.Clear();
            this.passwordTextbox.Clear();
        }

        public void ResetForm()
        {
            this.Window_Loaded(null, null);
        }

        public string UserName
        {
            get { return this.usernameTextbox.Text; }
            set { this.usernameTextbox.Text = value; }
        }

        public string Password
        {
            get { return this.passwordTextbox.Password; }
            set { this.passwordTextbox.Password = value; }
        }

        public int State
        {
            get { return this.state; }
            set { this.state = value; }
        }
        #endregion

        #region callback function after controller
        public void NeedPay()
        {
            Dispatcher.Invoke(new Action(() => {
                PaymentWindow pay = new PaymentWindow();
                pay.SetController(this._controller);
                pay.setNotifyLabel("Tài khoản đã hết hạn, vui lòng gia hạn !");
                pay.ShowDialog();
            }));
        }

        public void LoginEnable(bool enable)
        {
            Dispatcher.Invoke(new Action(() => {
                this.loginButton.IsEnabled = enable;
            }));
        }

        public void GoChat()
        {
            Dispatcher.Invoke(new Action(() => {
                // close the form on the forms thread
                this.Hide();

                ChatWindow view = new ChatWindow();
                view.setLoginWindow(this);
                view.ShowDialog();
            }));

        }
        #endregion

        #region Events handle

        private void loginButton_Click(object sender, RoutedEventArgs e)
        {
            this._controller.Login();

            //check rememberpass 
            if (this.rememberPassChk.IsChecked == true)
            {
                Properties.Settings.Default["SAVEPWD"] = "1";
                Properties.Settings.Default["USERNAME"] = this.usernameTextbox.Text;
                Properties.Settings.Default["PASSWORD"] = this.passwordTextbox.Password;
                Properties.Settings.Default.Save();
            }
            else
            {
                Properties.Settings.Default["SAVEPWD"] = "";
                Properties.Settings.Default["USERNAME"] = "";
                Properties.Settings.Default["PASSWORD"] = "";
                Properties.Settings.Default.Save();
            }
        }

        private void registerButton_Click(object sender, RoutedEventArgs e)
        {
            WebWindow browser = new WebWindow();
            browser.OpenLink("http://trading.gametv.vn/api_app/app_register");
        }

        private void stateButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (this.state == 1)
            {
                this.state = 0;
                this.stateButton.Source = SystemUtils.getResource("invisible");
            }
            else
            {
                this.state = 1;
                this.stateButton.Source = SystemUtils.getResource("available");
            }
        }

        private void forgotPassLink_MouseDown(object sender, MouseButtonEventArgs e)
        {
            WebWindow browser = new WebWindow();
            browser.OpenLink("http://trading.gametv.vn/api_app/app_register");
        }

        private void usernameTextbox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                loginButton_Click(sender, e);
            }
        }

        private void passwordTextbox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                loginButton_Click(sender, e);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.Reload();
            string savePwd = Properties.Settings.Default["SAVEPWD"].ToString();
            if (savePwd == "1")
            {
                this.usernameTextbox.Text = Properties.Settings.Default["USERNAME"].ToString();
                this.passwordTextbox.Password = Properties.Settings.Default["PASSWORD"].ToString();
                this.rememberPassChk.IsChecked = true;
            }
            else
            {
                this.usernameTextbox.Text = "";
                this.passwordTextbox.Password = "";
                this.rememberPassChk.IsChecked = false;
            }
        }

        #endregion

        private void paymentLbl_MouseUp(object sender, MouseButtonEventArgs e)
        {
            PaymentWindow pay = new PaymentWindow();
            pay.SetController(this._controller);
            pay.ShowDialog();
        }

    }
}
