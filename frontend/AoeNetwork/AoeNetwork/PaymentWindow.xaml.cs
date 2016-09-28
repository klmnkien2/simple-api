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
    /// Interaction logic for PaymentWindow.xaml
    /// </summary>
    public partial class PaymentWindow : Window
    {
        public PaymentWindow()
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

        public void setNotifyLabel(string text)
        {
            this.notifyLabel.Content = text;
        }

        public void AfterPay(string message)
        {
            Dispatcher.Invoke(new Action(() =>
            {
                setNotifyLabel(message);
            }));
        }

        AuthenController controller;
        public void SetController(AuthenController controller)
        {
            this.controller = controller;
        }

        private int user_id;
        public int getUserId()
        {
            return this.user_id;
        }

        public void setUserId(int user_id)
        {
            this.user_id = user_id;
        }

        public string getCardSeri()
        {
            return this.cardSeri.Text;
        }

        public string getCardCode()
        {
            return this.cardCode.Text;
        }

        public string getCardType()
        {
            ComboBoxItem item = this.cardType.SelectedItem as ComboBoxItem;
            return item.Uid;
        }

        private void submit_Click(object sender, RoutedEventArgs e)
        {
            this.controller.Payment(this);
        }

    }
}
