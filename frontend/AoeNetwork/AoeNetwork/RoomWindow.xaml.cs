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
        private ChannelListItem channelList;
        private RoomInChannelList roomInChannelList;
        private string htmlChatBody = ""; // hold all in <body> tag of html chat

        public void SetRoomController(RoomController roomController)
        {
            this.roomController = roomController;
        }
        public void SetChatView(ChatWindow chatView)
        {
            this.chatView = chatView;
        }

        int min_view_id = 0;
        int max_view_id = 0;

        #region private function, init function...

        private void InitTabView() 
        {
            tabHome_Click(null, null);
        }
        
        private void InitUserList()
        {
            this.userList = new RoomListItem(this, this.userListControl);
            this.channelList = new ChannelListItem(this, this.channelListControl);
            this.roomInChannelList = new RoomInChannelList(this, this.roomInChannelListControl);
        }

        private void DisplayLoadingGame(bool loading)
        {
            if (loading)
            {
                this.tabGameGrid.RowDefinitions[1].Height = new GridLength(30);
                this.tabGameGrid.RowDefinitions[2].Height = new GridLength(40);
            }
            else
            {
                this.tabGameGrid.RowDefinitions[1].Height = new GridLength(0);
                this.tabGameGrid.RowDefinitions[2].Height = new GridLength(0);
            }
        }
        #endregion

        #region controller callback
        public void AddPrivateMessage(APIMessage message)
        {
            Dispatcher.Invoke(new Action(() => {
                
                if (max_view_id == 0 || max_view_id < message.message_id)
                {
                    string times = DateTime.Now.ToString("HH:mm:ss");
                    string extend = "";
                    extend += ("<span style='color:green'>[" +times+"][" + message.user_name + "]: </span>" + "<span style='color:white'>" + HttpUtility.HtmlEncode(message.message) + "</span>");
                    htmlChatBody += YahooIcon.translateText("<div>" + extend + "</div>");
                    this.reGenerateHtml();
                
                    max_view_id = message.message_id;
                }
            }));
        }

        public void RenderMessage(IList messages)
        {
            Dispatcher.Invoke(new Action(() => {
                for (int i = messages.Count - 1; i >= 0; i--)
                {
                    APIMessage message = (APIMessage)messages[i];
                    if ((max_view_id == 0 || max_view_id < message.message_id))
                    {
                        string extend = "";
                        string times = SystemUtils.UnixTimeStampToDateTime(message.create_time);
                        if (message.notify == 0)
                        {
                            if (message.user_id == StaticValue.user_id)
                            {
                                extend = ("<span style='color:green'>[" + times + "][" + message.user_name + "]: </span>" + "<span style='color:white'>" + HttpUtility.HtmlEncode(message.message) + "</span>");
                            }
                            else
                            {
                                extend = ("<span style='color:blue'>[" + times + "][" + message.user_name + "]: </span>" + "<span style='color:white'>" + HttpUtility.HtmlEncode(message.message) + "</span>");
                            }

                        }
                        else if (message.notify == 2)
                        {
                            extend += ("<span style='color:red'>[" + times + "][Tin nhắn từ hệ thống]:</span>" + "<span style='color:white'>Chào mừng đến với " + message.message + "</span>");
                        }
                        if (extend != "")
                        {
                            htmlChatBody += YahooIcon.translateText("<div>" + extend + "</div>");
                            this.reGenerateHtml();
                        }
                    
                        max_view_id = message.message_id;
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
                    if ((min_view_id == 0 || min_view_id > message.message_id))
                    {
                        string extend = "";
                        string times = SystemUtils.UnixTimeStampToDateTime(message.create_time);
                        if (message.notify == 0)
                        {
                            if (message.user_id == StaticValue.user_id)
                            {
                                extend = ("<span style='color:green'>[" + times + "][" + message.user_name + "]: </span>" + "<span style='color:white'>" + HttpUtility.HtmlEncode(message.message) + "</span>");
                            }
                            else
                            {
                                extend = ("<span style='color:blue'>[" + times + "][" + message.user_name + "]: </span>" + "<span style='color:white'>" + HttpUtility.HtmlEncode(message.message) + "</span>");
                            }

                        }
                        else if (message.notify == 2)
                        {
                            extend += ("<span style='color:red'>[" + times + "][Tin nhắn từ hệ thống]:</span>" + "<span style='color:white'>Chào mừng đến với " + message.message + "</span>");
                        }
                        if (extend != "")
                        {
                            htmlChatBody = YahooIcon.translateText("<div>" + extend + "</div>") + htmlChatBody;
                            this.reGenerateHtml();
                        }
                    
                        min_view_id = message.message_id;
                    }
                }
            }));
        }

        private void reGenerateHtml()
        {
            this.htmlChatContent.Text = "<html>"
+ "<head>"
+ "<meta charset=\"utf-8\">"
+ "<meta http-equiv=\"X-UA-Compatible\" content=\"IE=edge\">"
+ "<style>"
+ "body {"
    + "font-family: Helvetica, Arial, Meiryo, sans-serif;"
    + "font-size: 13px;"
    + "line-height: 1.5;"
+ "}"
+ "</style>"
+ "</head>" 
+ "<body>"
+ this.htmlChatBody
+ "</body>"
+ "</html>";
        }

        public void SetAdsInfo(string url, string image, int type)
        {
            Dispatcher.Invoke(new Action(() => {
                if (type == 0) // banner
                {
                    //this.bannerAds.Source = SystemUtils.getImageUrl(image);

                }
                else if (type == 1)
                {
                    //this.adsPic1.Source = SystemUtils.getImageUrl(image);
                }
                else if (type == 2)
                {
                    //this.adsPic2.Source = SystemUtils.getImageUrl(image);
                }
            }));
        }

        public void SetChannel(DataTable dataTable)
        {
            Dispatcher.Invoke(new Action(() =>
            {
                foreach (DataRow row in dataTable.Rows)
                {
                    Channel channel = new Channel();
                    channel.id = int.Parse(row["room_id"].ToString());
                    channel.name = row["name"].ToString();
                    channel.image = row["image"].ToString();

                    this.channelList.addItem(channel);
                }
            }));
        }

        public void LoadRoomInChannel(Channel channel)
        {
            roomController.LoadChannel(channel.id.ToString());
            this.tabGame_Click(null, null);
        }

        public void SetRoomOfChannel(DataTable dataTable)
        {
            Dispatcher.Invoke(new Action(() =>
            {
                this.roomInChannelList.clearData();
                foreach (DataRow row in dataTable.Rows)
                {
                    Room room = new Room();
                    room.room_id = int.Parse(row["room_id"].ToString());
                    room.room_name = row["name"].ToString();
                    room.members = row["members"].ToString();
                    room.maximum = row["maximum"].ToString();
                    room.server_id = row["server_id"].ToString();
                    room.parent_id = int.Parse(row["parent_id"].ToString());
                    room.host = row["host"].ToString();
                    room.port = row["port"].ToString();
                    room.hub = row["hub"].ToString();
                    room.level = row["level"].ToString();

                    this.roomInChannelList.addItem(room);
                }
            }));
        }

        #region Tree room no use now
        /*
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
        */
        #endregion

        private bool isRoomSet = false;
        public void SuccessJoinRoom(Room room)
        {            
            Dispatcher.Invoke(new Action(() => {
                isRoomSet = true;
                roomNameLbl.Content = room.room_name;
                htmlChatContent.Text = "";
                htmlChatBody = "";
                min_view_id = 0;
                max_view_id = 0;

                tabRoom_Click(null, null);
                DisplayLoadingGame(false);

                roomController.ScheduleWork();
                roomController.HistoryMessage(0);
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
            this.tabRoom.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#535859"));
            this.tabGame.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#535859"));
            this.tabHome.Foreground = Brushes.White;

            this.tabHomeContent.Visibility = Visibility.Visible;
            this.tabHomeContent.IsHitTestVisible = true;

            this.channelListContainer.Visibility = Visibility.Hidden;
            this.channelListContainer.IsHitTestVisible = false;
            this.tabGameContent.Visibility = Visibility.Hidden;
            this.tabGameContent.IsHitTestVisible = false;
            this.tabRoomContent.Visibility = Visibility.Hidden;
            this.tabRoomContent.IsHitTestVisible = false;
            this.tabRoomContent_extend.Visibility = Visibility.Hidden;
            this.tabRoomContent_extend.IsHitTestVisible = false;
            this.tabGameContent_extend.Visibility = Visibility.Hidden;
            this.tabGameContent_extend.IsHitTestVisible = false;
        }

        private void tabGame_Click(object sender, RoutedEventArgs e)
        {
            this.tabRoom.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#535859"));
            this.tabHome.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#535859"));
            this.tabGame.Foreground = Brushes.White;

            this.tabGameContent.Visibility = Visibility.Visible;
            this.tabGameContent.IsHitTestVisible = true;
            this.tabGameContent_extend.Visibility = Visibility.Visible;
            this.tabGameContent_extend.IsHitTestVisible = true;
            this.channelListContainer.Visibility = Visibility.Visible;
            this.channelListContainer.IsHitTestVisible = true;

            this.tabHomeContent.Visibility = Visibility.Hidden;
            this.tabHomeContent.IsHitTestVisible = false;
            this.tabRoomContent.Visibility = Visibility.Hidden;
            this.tabRoomContent.IsHitTestVisible = false;
            this.tabRoomContent_extend.Visibility = Visibility.Hidden;
            this.tabRoomContent_extend.IsHitTestVisible = false;
        }

        private void tabRoom_Click(object sender, RoutedEventArgs e)
        {
            if (!isRoomSet)
            {
                MessageBox.Show("Bạn chưa tham gia phòng nào!");
                return;
            }

            this.tabHome.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#535859"));
            this.tabGame.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#535859"));
            this.tabRoom.Foreground = Brushes.White;

            this.channelListContainer.Visibility = Visibility.Visible;
            this.channelListContainer.IsHitTestVisible = true;
            this.tabRoomContent.Visibility = Visibility.Visible;
            this.tabRoomContent.IsHitTestVisible = true;
            this.tabRoomContent_extend.Visibility = Visibility.Visible;
            this.tabRoomContent_extend.IsHitTestVisible = true;

            this.tabGameContent.Visibility = Visibility.Hidden;
            this.tabGameContent.IsHitTestVisible = false;
            this.tabHomeContent.Visibility = Visibility.Hidden;
            this.tabHomeContent.IsHitTestVisible = false;
            this.tabGameContent_extend.Visibility = Visibility.Hidden;
            this.tabGameContent_extend.IsHitTestVisible = false;
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

        public void JoinARoom(Room room)
        {
            
            DisplayLoadingGame(true);
            
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

        private void userStatus_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                e.Handled = true;
                //roomController.UpdateStatus(1, this.userStatus.Text);
                StaticValue.status = this.userStatus.Text;
                Keyboard.ClearFocus();
            }
        }

        private void coinBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //open context
            ContextMenu cm = this.FindResource("contextCoin") as ContextMenu;
            cm.PlacementTarget = sender as WrapPanel;
            cm.IsOpen = true;
        }

        private void homePageBrowser_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            this.homePageBrowserLabel.Visibility = Visibility.Hidden;
            this.homePageBrowser.Visibility = Visibility.Visible;
        }

        private void Coin_Menu_Click(object sender, RoutedEventArgs e)
        {
            chatView.Coin_Menu_Click(sender, e);
        }

        private void State_Menu_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ContextMenu cm = this.FindResource("cmStateChanging") as ContextMenu;
            cm.PlacementTarget = sender as WrapPanel;
            cm.IsOpen = true;
        }

        private void State_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            chatView.State_MenuItem_Click(sender, e);
        }

        private void logoutStripMenuItem_Click(object sender, RoutedEventArgs e)
        {
            chatView.logoutStripMenuItem_Click(null, null);
        }

        private void exitStripMenuItem_Click(object sender, RoutedEventArgs e)
        {
            chatView.exitStripMenuItem_Click(null, null);
        }

        public void SetUserInfo(string username, string status, string avatar, string level, string diamond, int state)
        {
            Dispatcher.Invoke(new Action(() => {
                this.userStatus.Text = (status == null || status.Trim() == "") ? "status..." : status;
                this.userLevel.Content = "Level " + level;
                this.userDiamond.Content = "" + diamond;
                this.userName.Content = username;
                if (state == 2)
                {
                    userState.Source = SystemUtils.getResource("login_busy");
                }
                else if (state == 0)
                {
                    userState.Source = SystemUtils.getResource("login_offline");
                }
                else
                {
                    userState.Source = SystemUtils.getResource("login_avail");
                }
            }));
        }

        private void channelTabBrowserTop_LoadCompleted(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            string script = "document.body.style.overflow ='hidden'";
            WebBrowser wb = (WebBrowser)sender;
            wb.InvokeScript("execScript", new Object[] { script, "JavaScript" });
        }

        private void channelTabBrowserBottom_LoadCompleted(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            string script = "document.body.style.overflow ='hidden'";
            WebBrowser wb = (WebBrowser)sender;
            wb.InvokeScript("execScript", new Object[] { script, "JavaScript" });
        }

        private void homePageBrowser_LoadCompleted(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            string script = "document.body.style.overflow ='hidden'";
            WebBrowser wb = (WebBrowser)sender;
            wb.InvokeScript("execScript", new Object[] { script, "JavaScript" });
        }
    }
}
