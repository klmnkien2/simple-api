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
            InitChatHistory();
        }

        #region action for extend MainWindow (title bar, border, ...)
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
        private RoomController roomController;
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

        private HtmlPanel htmlChatContent;
        private void InitChatHistory()
        {
            htmlChatContent = new HtmlPanel();
            htmlChatContent.Text = "";
            //htmlChatContent.Dock = DockStyle.Fill;
            //htmlChatContent.BackColor = Color.Transparent;
            //htmlChatContent.BorderStyle = BorderStyle.None;
            this.chatHistoryContainer.Child = htmlChatContent;
        }

        private void InitTabView() 
        {
            tabHome_Click(null, null);
        }

        #region controller callback
        public void AddPrivateMessage(APIMessage message)
        {
            Dispatcher.Invoke(new Action(() => {
                string extend = "";
                //DateTime mesDate = new DateTime();
                //extend += mesDate.ToString("[H:mm:ss]");
                extend += ("<span style='color:green'>[" + message.user_name + "]: </span>" + "<span style='color:white'>" + message.message + "</span>");
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
                            extend = ("<span style='color:green'>[" + message.user_name + "]: </span>" + "<span style='color:white'>" + message.message + "</span>");
                        }
                        else
                        {
                            extend = ("<span style='color:blue'>[" + message.user_name + "]: </span>" + "<span style='color:white'>" + message.message + "</span>");
                        }

                    }
                    else if (message.notify == 1)
                    {
                        extend = ("<span style='color:red'>[Thông báo]:</span>" + "<span style='color:white'>" + message.message + "</span>");
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
                            extend = ("<span style='color:green'>[" + message.user_name + "]: </span>" + "<span style='color:white'>" + message.message + "</span>");
                        }
                        else
                        {
                            extend = ("<span style='color:blue'>[" + message.user_name + "]: </span>" + "<span style='color:white'>" + message.message + "</span>");
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
                //if (type == 0) // banner
                //{
                //    this.bannerAd.ImageLocation = image;

                //}
                //else if (type == 1)
                //{
                //    this.adImage1.ImageLocation = image;
                //}
                //else if (type == 2)
                //{
                //    this.adImage2.ImageLocation = image;
                //}
            }));
        }

        public void SetTreeRoom(DataTable dataTable)
        {
            Dispatcher.Invoke(new Action(() => {
                //treeAddNodes(null, dataTable);
                //this.gameTreeView.ExpandAll();
            }));
        }

        //private void treeAddNodes(TreeNode parent, DataTable dataTable)
        //{
        //    foreach (DataRow row in dataTable.Rows)
        //    {
        //        Room room = new Room();
        //        room.room_id = int.Parse(row["room_id"].ToString());
        //        room.room_name = row["name"].ToString();
        //        room.server_id = row["server_id"].ToString();
        //        room.parent_id = int.Parse(row["parent_id"].ToString());
        //        room.host = row["host"].ToString();
        //        room.port = row["port"].ToString();
        //        room.hub = row["hub"].ToString();

        //        TreeNode node = new TreeNode(room.room_name);
        //        node.Tag = room;
        //        if (parent != null)
        //        {
        //            parent.Nodes.Add(node);
        //        }
        //        else
        //        {
        //            this.gameTreeView.Nodes.Add(node);
        //        }

        //        if (row.Table.Columns.Contains("child") && row["child"] is DataTable)
        //        {
        //            DataTable child = (DataTable)row["child"];

        //            treeAddNodes(node, child);
        //        }
        //    }
        //}

        private bool isRoomSet = false;
        public void SuccessJoinRoom(Room room)
        {            
            Dispatcher.Invoke(new Action(() => {
                isRoomSet = true;
                roomNameLbl.Content = room.room_name;
                tabRoom_Click(null, null);
                //this.loadingselect.Height = 0;

                roomController.LoadUsers();
            }));
        }

        public void SetUserList(IList dataSource)
        {
            Dispatcher.Invoke(new Action(() => {

                IList base_items = new ArrayList();
                foreach (UserCache user in dataSource)
                {
                    //this.tableListUser.addUser(user);
                }
            }));

        }
        #endregion

        #region Events handle
        private void buttonStart_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Start");
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

        }

        private void tabHome_Click(object sender, RoutedEventArgs e)
        {
            this.windowAllContentGrid.ColumnDefinitions[1].Width = new GridLength(0);//init 240
        }

        private void tabGame_Click(object sender, RoutedEventArgs e)
        {
            this.windowAllContentGrid.ColumnDefinitions[1].Width = new GridLength(0);//init 240
        }

        private void tabRoom_Click(object sender, RoutedEventArgs e)
        {
            if (!isRoomSet)
            {
                MessageBox.Show("Bạn chưa tham gia phòng nào!");
                return;
            }
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

        }
        #endregion
    }
}
