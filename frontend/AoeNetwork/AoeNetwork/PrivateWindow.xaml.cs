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
    /// Interaction logic for PrivateWindow.xaml
    /// </summary>
    public partial class PrivateWindow : Window
    {
        public PrivateWindow()
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
            this.Hide();
        }

        private void coolform_mini_btn_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.WindowState = System.Windows.WindowState.Minimized;
        }
        #endregion

        #region IntefaceView implementation
        PrivateController privateController;
        Friend friend;
        int last_view_id = 0;
        ChatWindow chatView;

        public void SetController(PrivateController controller)
        {
            privateController = controller;
        }

        public void SetChatView(ChatWindow chatView)
        {
            this.chatView = chatView;
        }

        public void SetFriendInfo(Friend friend)
        {
            this.friend = friend;
            this.coolform_titletext.Content = friend.user_name;

            if (friend.state == 1)
            {
                this.friendState.Source = SystemUtils.getResource("state_avail");
                if (friend.status == null || friend.status == "")
                {
                    friend.status = "Available";
                }
            }
            else
            {
                if (friend.status == null || friend.status == "")
                {
                    friend.status = "Offline";
                }
                if (friend.state == 0)
                {
                    this.friendState.Source = SystemUtils.getResource("state_invi");
                }
            }
            this.friendStatus.Content = friend.status;

            // CHeck button showing
            if (friend.type == 1)
            {
                this.btnAcceptFriend.Width = 0;
                this.btnDenyFriend.Width = 0;
                this.btnIgnore.Width = 90;
                this.btnRemoveIgnore.Width = 0;
            }
            else if (friend.type == 0)
            {
                this.btnAcceptFriend.Width = 90;
                this.btnDenyFriend.Width = 90;
                this.btnIgnore.Width = 90;
                this.btnRemoveIgnore.Width = 0;
            }
            else if (friend.type == -1)
            {
                this.btnAcceptFriend.Width = 0;
                this.btnDenyFriend.Width = 0;
                this.btnIgnore.Width = 0;
                this.btnRemoveIgnore.Width = 90;
            }
        }

        public int GetLastViewId()
        {
            return this.last_view_id;
        }

        public void SetLastViewId(int last_view_id)
        {
            this.last_view_id = last_view_id;
        }

        public void RenderMessage(APIMessage message)
        {
            string extend = "";
            //DateTime mesDate = new DateTime();
            //extend += mesDate.ToString("[H:mm:ss]");
            if (message.notify == 0)
            {
                if (message.user_id == StaticValue.user_id)
                {
                    extend += ("<span style='color:green'>[" + message.user_name + "]: </span>" + "<span style='color:white'>" + message.message + "</span>");
                }
                else
                {
                    extend += ("<span style='color:blue'>[" + message.user_name + "]: </span>" + "<span style='color:white'>" + message.message + "</span>");
                }

            }
            else if (message.notify == 1)
            {
                extend += ("<span style='color:red'>[Thông báo]:</span>" + "<span style='color:white'>" + message.message + "</span>");
            }
            htmlChatContent.Text += YahooIcon.translateText("<div>" + extend + "</div>");
            if (last_view_id == 0 || last_view_id > message.message_id)
            {
                last_view_id = message.message_id;
            }
        }

        public void RenderOldMessage(APIMessage message)
        {
            string extend = "";
            //DateTime mesDate = new DateTime();
            //extend += mesDate.ToString("[H:mm:ss]");
            if (message.notify == 0)
            {
                if (message.user_id == StaticValue.user_id)
                {
                    extend += ("<span style='color:green'>[" + message.user_name + "]: </span>" + "<span style='color:white'>" + message.message + "</span>");
                }
                else
                {
                    extend += ("<span style='color:blue'>[" + message.user_name + "]: </span>" + "<span style='color:white'>" + message.message + "</span>");
                }

            }
            htmlChatContent.Text = YahooIcon.translateText("<div>" + extend + "</div>") + htmlChatContent.Text;
            if (last_view_id == 0 || last_view_id > message.message_id)
            {
                last_view_id = message.message_id;
            }
        }
        #endregion

        #region handle events
        private void buttonEnter_Click(object sender, RoutedEventArgs e)
        {
            if (this.messageBox.Text.Trim() != "")
            {
                privateController.SendMessage(this.friend, this.messageBox.Text.Trim());
                this.messageBox.Text = "";
            }
        }

        private void messageBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                e.Handled = true;
                buttonEnter_Click(null, null);
            }
        }

        private void loadMoreLbl_MouseDown(object sender, MouseButtonEventArgs e)
        {
            privateController.HistoryMessage(this.friend.user_id, this.last_view_id);
        }

        private void btnAcceptFriend_Click(object sender, RoutedEventArgs e)
        {
            privateController.UpdateFriendStatus(this.friend, 1, 0);
        }

        private void btnDenyFriend_Click(object sender, RoutedEventArgs e)
        {
            privateController.UpdateFriendStatus(this.friend, -2, 0);
        }

        private void btnIgnore_Click(object sender, RoutedEventArgs e)
        {
            privateController.UpdateFriendStatus(this.friend, 0, -2);//ignore type can xem xet this.user
        }

        private void btnRemoveIgnore_Click(object sender, RoutedEventArgs e)
        {
            privateController.UpdateFriendStatus(this.friend, 0, -2);//ignore type can xem xet this.user
        }

        #endregion
    }
}
