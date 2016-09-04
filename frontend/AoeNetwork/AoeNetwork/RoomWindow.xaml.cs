using RestSharp.Extensions.MonoHttp;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
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
using TheArtOfDev.HtmlRenderer.WPF;

namespace AoeNetwork
{
    /// <summary>
    /// Interaction logic for RoomWindow.xaml
    /// </summary>
    public partial class RoomWindow : Window
    {
        public RoomWindow()
        {
            InitializeComponent();

            InitTabView();
            InitUserList();
            DisplayLoadingGame(false);
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

        private ChatWindow chatView;
        private SelectRunWindow selectRunWindow;
        private RoomController roomController;
        private RoomListItem userList;
        public void SetRoomController(RoomController roomController)
        {
            this.roomController = roomController;
        }
        public void SetChatView(ChatWindow chatView)
        {
            this.chatView = chatView;
        }

        int last_view_id = 0;
        public int LastViewId
        {
            get { return this.last_view_id; }
            set { this.last_view_id = value; }
        }


        #region private function, init function...

        private void InitTabView() 
        {
            tabHome_Click(null, null);
        }
        
        private void InitUserList()
        {
            this.userList = new RoomListItem(this, this.userListControl);
        }

        private void DisplayLoadingGame(bool loading)
        {
            if (loading)
            {
                this.tabGameContent.RowDefinitions[1].Height = new GridLength(30);
                this.tabGameContent.RowDefinitions[2].Height = new GridLength(40);
            }
            else
            {
                this.tabGameContent.RowDefinitions[1].Height = new GridLength(0);
                this.tabGameContent.RowDefinitions[2].Height = new GridLength(0);
            }
        }
        #endregion

        #region controller callback
        public void AddPrivateMessage(APIMessage message)
        {
            Dispatcher.Invoke(new Action(() => {
                string extend = "";
                //DateTime mesDate = new DateTime();
                //extend += mesDate.ToString("[H:mm:ss]");
                extend += ("<span style='color:green'>[" + message.user_name + "]: </span>" + "<span style='color:white'>" + HttpUtility.HtmlEncode(message.message) + "</span>");
                htmlChatContent.Text += YahooIcon.translateText("<div>" + extend + "</div>");

            }));
        }

        public void RenderMessage(IList messages)
        {
            Dispatcher.Invoke(new Action(() => {
                for (int i = messages.Count - 1; i >= 0; i--)
                {
                    APIMessage message = (APIMessage)messages[i];
                    string extend = "";
                    //DateTime mesDate = new DateTime();
                    //extend += mesDate.ToString("[H:mm:ss]");
                    if (message.notify == 0)
                    {
                        if (message.user_id == StaticValue.user_id)
                        {
                            extend = ("<span style='color:green'>[" + message.user_name + "]: </span>" + "<span style='color:white'>" + HttpUtility.HtmlEncode(message.message) + "</span>");
                        }
                        else
                        {
                            extend = ("<span style='color:blue'>[" + message.user_name + "]: </span>" + "<span style='color:white'>" + HttpUtility.HtmlEncode(message.message) + "</span>");
                        }

                    }
                    else if (message.notify == 1)
                    {
                        extend = ("<span style='color:red'>[Thông báo]:</span>" + "<span style='color:white'>" + HttpUtility.HtmlEncode(message.message) + "</span>");
                    }
                    if (extend != "")
                    {
                        htmlChatContent.Text += YahooIcon.translateText("<div>" + extend + "</div>");
                    }
                    if (last_view_id == 0 || last_view_id > message.message_id)
                    {
                        last_view_id = message.message_id;
                    }
                }
            }));
        }

        public void RenderOldMessage(IList messages)
        {
            Dispatcher.Invoke(new Action(() => {
                for (int i = messages.Count - 1; i >= 0; i--)
                {
                    APIMessage message = (APIMessage)messages[i];
                    string extend = "";
                    //DateTime mesDate = new DateTime();
                    //extend += mesDate.ToString("[H:mm:ss]");
                    if (message.notify == 0)
                    {
                        if (message.user_id == StaticValue.user_id)
                        {
                            extend = ("<span style='color:green'>[" + message.user_name + "]: </span>" + "<span style='color:white'>" + HttpUtility.HtmlEncode(message.message) + "</span>");
                        }
                        else
                        {
                            extend = ("<span style='color:blue'>[" + message.user_name + "]: </span>" + "<span style='color:white'>" + HttpUtility.HtmlEncode(message.message) + "</span>");
                        }

                    }
                    if (extend != "")
                    {
                        htmlChatContent.Text = YahooIcon.translateText("<div>" + extend + "</div>") + htmlChatContent.Text;
                    }
                    if (last_view_id == 0 || last_view_id > message.message_id)
                    {
                        last_view_id = message.message_id;
                    }
                }
            }));
        }

        public void SetAdsInfo(string url, string image, int type)
        {
            Dispatcher.Invoke(new Action(() => {
                if (type == 0) // banner
                {
                    this.bannerAds.Source = SystemUtils.getImageUrl(image);

                }
                else if (type == 1)
                {
                    this.adsPic1.Source = SystemUtils.getImageUrl(image);
                }
                else if (type == 2)
                {
                    this.adsPic2.Source = SystemUtils.getImageUrl(image);
                }
            }));
        }

        public void SetTreeRoom(DataTable dataTable)
        {
            Dispatcher.Invoke(new Action(() => {
                treeAddNodes(null, dataTable);
            }));
        }

        private void treeAddNodes(TreeViewItem parent, DataTable dataTable)
        {
            foreach (DataRow row in dataTable.Rows)
            {
                Room room = new Room();
                room.room_id = int.Parse(row["room_id"].ToString());
                room.room_name = row["name"].ToString();
                room.server_id = row["server_id"].ToString();
                room.parent_id = int.Parse(row["parent_id"].ToString());
                room.host = row["host"].ToString();
                room.port = row["port"].ToString();
                room.hub = row["hub"].ToString();

                TreeViewItem node = new TreeViewItem();
                node.IsExpanded = true;
                node.Header = room.room_name;
                node.Tag = room;
                node.MouseDoubleClick += this.GameTreeItemMouseDoubleClick;
                if (parent != null)
                {
                    parent.Items.Add(node);
                }
                else
                {
                    this.GameTreeView.Items.Add(node);
                }

                if (row.Table.Columns.Contains("child") && row["child"] is DataTable)
                {
                    DataTable child = (DataTable)row["child"];

                    treeAddNodes(node, child);
                }
            }
        }

        private bool isRoomSet = false;
        public void SuccessJoinRoom(Room room)
        {            
            Dispatcher.Invoke(new Action(() => {
                isRoomSet = true;
                roomNameLbl.Content = room.room_name;
                tabRoom_Click(null, null);
                DisplayLoadingGame(false);

                roomController.LoadUsers();
            }));
        }

        public void SetUserList(IList dataSource)
        {
            Dispatcher.Invoke(new Action(() => {

                IList base_items = new ArrayList();
                foreach (UserCache user in dataSource)
                {
                    this.userList.addUser(user);
                }
            }));

        }
        #endregion

        #region Events handle
        private void buttonStart_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.Reload();
            var url = Properties.Settings.Default["RUN_URL"].ToString();
            if (url == "")
            {
                buttonSettting_Click(null, null);
            }
            else
            {
                SystemUtils.OpenGame(url);
            }
        }

        private void buttonEnter_Click(object sender, RoutedEventArgs e)
        {
            if (!isRoomSet)
            {
                return;
            }
            if (this.messageBox.Text.Trim() != "")
            {
                roomController.SendMessage(this.messageBox.Text.Trim());
                this.messageBox.Text = "";
            }
        }

        private void buttonSettting_Click(object sender, RoutedEventArgs e)
        {
            selectRunWindow = new SelectRunWindow();
            selectRunWindow.ShowDialog();
        }

        private void tabHome_Click(object sender, RoutedEventArgs e)
        {
            this.windowAllContentGrid.ColumnDefinitions[1].Width = new GridLength(0);//init 240
            this.tabGameContent.Visibility = Visibility.Hidden;
            this.tabHomeContent.Visibility = Visibility.Visible;
            this.tabRoomContent.Visibility = Visibility.Hidden;
        }

        private void tabGame_Click(object sender, RoutedEventArgs e)
        {
            this.tabGameContent.Visibility = Visibility.Visible;
            this.tabHomeContent.Visibility = Visibility.Hidden;
            this.tabRoomContent.Visibility = Visibility.Hidden;
            this.windowAllContentGrid.ColumnDefinitions[1].Width = new GridLength(0);//init 240
        }

        private void tabRoom_Click(object sender, RoutedEventArgs e)
        {
            if (!isRoomSet)
            {
                MessageBox.Show("Bạn chưa tham gia phòng nào!");
                return;
            }
            this.tabGameContent.Visibility = Visibility.Hidden;
            this.tabHomeContent.Visibility = Visibility.Hidden;
            this.tabRoomContent.Visibility = Visibility.Visible;
            this.windowAllContentGrid.ColumnDefinitions[1].Width = new GridLength(240);//init 240
        }

        private void messageBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                e.Handled = true;
                buttonEnter_Click(null, null);
            }
        }

        private void searchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            Dispatcher.Invoke(new Action(() =>
            {
                if (this.userList == null) return;
                this.userList.ResetSearch();
                if (!string.IsNullOrEmpty(this.searchBox.Text))
                {
                    this.userList.SearchByString(this.searchBox.Text);
                }
            }));
        }

        private void GameTreeItemMouseDoubleClick(object sender, MouseButtonEventArgs args)
        {
            if (sender is TreeViewItem)
            {
                if (!((TreeViewItem)sender).IsSelected)
                {
                    return;
                }
            }
            DisplayLoadingGame(true);
            //
            // Get the selected node.
            //
            TreeViewItem node = (TreeViewItem)sender;
            Room room = (Room)node.Tag;
            if (room.server_id != "0")
            {
                // Join room and Access VPN here
                string requestError = SystemUtils.CallVPNConnection(room);
                if (requestError == null)
                {
                    roomController.JoinRoom(room);
                }
                else
                {
                    DisplayLoadingGame(false);
                    MessageBox.Show("Kết nối VPN không thành công.");
                }
            }
            else
            {
                DisplayLoadingGame(false);
            }

        }
        #endregion
    }
}
