﻿using System;
using System.Collections;
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
    /// Interaction logic for ChatWindow.xaml
    /// </summary>
    public partial class ChatWindow : Window
    {
        public ChatWindow()
        {
            InitializeComponent();

            // Init controller
            chatController = new ChatController(this);
            chatController.LoadView();

            privateController = new PrivateController(this);
            privateController.LoadView();

            initFriendLists();
            tabfriendText_MouseDown(null, null);//select tab friend;
        }

        #region action for extend MainWindow (title bar, border, ...)
        private void coolform_titletext_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }
        private void coolform_close_btn_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Logout();
        }

        private void coolform_mini_btn_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.WindowState = System.Windows.WindowState.Minimized;
        }
        #endregion

        #region controller and their callback
        ChatController chatController;
        PrivateController privateController = null;
        RoomController roomController = null;

        RoomWindow roomView = null;
        AddFriendWindow addFriendView;
        LoginWindow loginView;

        Dictionary<int, PrivateWindow> privateViews = new Dictionary<int, PrivateWindow>();
        Dictionary<int, Friend> mapFriend = new Dictionary<int, Friend>();

        public PrivateController getPrivateController()
        {
            return this.privateController;
        }

        public void setLoginWindow(LoginWindow loginView)
        {
            this.loginView = loginView;
        }

        public void SetFriendList(IList dataSource)
        {
            Dispatcher.Invoke(new Action(() => {
                lock (this.mapFriend)
                {
                    this.mapFriend.Clear();
                    ArrayList friendIDs = new ArrayList(this.friendList.IDs);//all current friend id
                    ArrayList ignoreIDs = new ArrayList(this.ignoreList.IDs);

                    foreach (Friend friend in dataSource)
                    {
                        Friend existFriend = null;
                        if (mapFriend.TryGetValue(friend.user_id, out existFriend))
                        {
                            existFriend.state = friend.state;
                            existFriend.type = friend.type;
                            existFriend.status = friend.status;
                        }
                        else
                        {
                            this.mapFriend.Add(friend.user_id, friend);
                        }

                        if (friend.type != -1)
                        {
                            this.friendList.addUser(friend);
                            friendIDs.Remove(friend.user_id);//remove to get list of deleted id
                        }
                        else
                        {
                            this.ignoreList.addUser(friend);
                            ignoreIDs.Remove(friend.user_id);
                        }
                    }

                    // Delete in GUI list
                    foreach (int id in friendIDs)
                    {
                        friendList.deleteUser(id);
                    }

                    foreach (int id in ignoreIDs)
                    {
                        ignoreList.deleteUser(id);
                    }
                }
            }));

        }

        public void SetUserInfo(string username, string status, string avatar, string level, string diamond, int state)
        {
            this.userStatus.Text = (status == null || status.Trim() == "") ? "status..." : status;
            this.userLevel.Content = "Level " + level;
            this.userDiamond.Content = "" + diamond;
            if (avatar != "")
            {
                //load image avatar
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(avatar, UriKind.RelativeOrAbsolute);
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.EndInit();
                userAvatar.Source = bitmap;
            }
            this.userName.Content = username;
            if (state == 0)
            {
                userState.Source = SystemUtils.getResource("state_invi");
            }
            else
            {
                userState.Source = SystemUtils.getResource("state_avail");
            }
        }

        public void AfterAddFriend(Friend friend, string error)
        {
            Dispatcher.Invoke(new Action(() => {
                if (addFriendView != null)
                {
                    addFriendView.Hide();
                    this.Focus();
                    MessageBox.Show("Đã gửi yêu cầu kết bạn thành công!");
                }
                if (error != null)
                {
                    MessageBox.Show(error);
                }
            }));
        }

        public void Logout()
        {
            StaticValue.Clear();

            // First close all open
            var windows = Application.Current.Windows;
            foreach (var item in windows)
            {
                if ((item as Window).Title.ToLower() == "loginwindow") continue;
                (item as Window).Close();
            }

            // Then create new login
            loginView.Show();
            loginView.LoginEnable(true);
            loginView.ResetForm();
        }

        public void OpenPrivate(Friend friendInfo, bool focus = false)
        {
            Dispatcher.Invoke(new Action(() => {
                PrivateWindow privateView;
                bool needNew = false;
                if (!privateViews.TryGetValue(friendInfo.user_id, out privateView))
                {
                    needNew = true;
                }

                if (needNew)
                {
                    privateView = new PrivateWindow();
                    privateView.SetController(privateController);
                    privateView.SetChatView(this);

                    privateView.SetFriendInfo(friendInfo);
                    privateViews.Add(friendInfo.user_id, privateView);
                }

                privateView.Show();

                if (focus)
                {
                    privateView.Topmost = true;
                    privateView.Focus();
                }
            }));

        }

        public void AddPrivateMessage(APIMessage message)
        {
            Dispatcher.Invoke(new Action(() => {
                PrivateWindow privateView;
                int user_id;
                string user_name;
                user_id = message.receive_id;
                user_name = message.receive_name;
                if (privateViews.TryGetValue(user_id, out privateView))
                {
                    privateView.Show();
                    privateView.RenderMessage(message);
                }

            }));

        }

        public void AfterUpdateFriend()
        {
            chatController.LoadFriends();
        }
        public void RenderPrivateMessage(bool first_time, IList messages)
        {
            Dispatcher.Invoke(new Action(() => {
                lock (this.mapFriend)
                {
                    for (int i = messages.Count - 1; i >= 0; i--)
                    {
                        APIMessage message = (APIMessage)messages[i];
                        PrivateWindow privateView;
                        int user_id;
                        string user_name;
                        if (message.receive_id == StaticValue.user_id)
                        {
                            user_id = message.user_id;
                            user_name = message.user_name;
                        }
                        else
                        {
                            user_id = message.receive_id;
                            user_name = message.receive_name;
                        }
                        if (!privateViews.TryGetValue(user_id, out privateView))
                        {
                            privateView = new PrivateWindow();
                            privateView.SetController(this.privateController);
                            privateView.SetChatView(this);

                            Friend friend = null;
                            if (!mapFriend.TryGetValue(user_id, out friend))
                            {
                                friend = new Friend();
                                friend.user_id = user_id;
                                friend.user_name = user_name;
                            }
                            privateView.SetFriendInfo(friend);
                            if (first_time)
                            {
                                // add to recent list
                                this.recentList.addUser(friend);
                            }
                            privateViews.Add(user_id, privateView);
                        }
                        privateView.RenderMessage(message);
                        if (first_time)
                        {
                            // add to recent list
                        }
                        else
                        {
                            privateView.Show();
                        }
                    }
                }
            }));

        }

        public void RenderOldPrivate(int receive_id, IList messages)
        {
            Dispatcher.Invoke(new Action(() => {
                PrivateWindow privateView;
                if (privateViews.TryGetValue(receive_id, out privateView))
                {

                    for (int i = 0; i <= messages.Count - 1; i++)
                    {
                        APIMessage message = (APIMessage)messages[i];
                        int user_id;
                        string user_name;
                        if (message.receive_id == StaticValue.user_id)
                        {
                            user_id = message.user_id;
                            user_name = message.user_name;
                        }
                        else
                        {
                            user_id = message.receive_id;
                            user_name = message.receive_name;
                        }
                        privateView.Show();
                        privateView.RenderOldMessage(message);
                    }
                }
            }));

        }

        #endregion

        #region Customlist
        private ChatListItem recentList;
        private ChatListItem friendList;
        private ChatListItem ignoreList;
        private void initFriendLists()
        {
            this.recentList = new ChatListItem(this, this.recentListControl);
            this.friendList = new ChatListItem(this, this.friendListControl);
            this.ignoreList = new ChatListItem(this, this.ignoreListControl);
        }
        #endregion

        #region handle events
        private void userSearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            Dispatcher.Invoke(new Action(() => {
                if (this.friendList == null || this.ignoreList == null) return;
                this.friendList.ResetSearch();
                this.ignoreList.ResetSearch();
                if (!string.IsNullOrEmpty(this.userSearchBox.Text))
                {
                    this.friendList.SearchByString(this.userSearchBox.Text);
                    this.ignoreList.SearchByString(this.userSearchBox.Text);
                }
            }));
        }

        private void aoeGameButton_Click(object sender, RoutedEventArgs e)
        {
            Dispatcher.Invoke(new Action(() => {
                if (roomView != null)
                {
                    roomView.Hide();
                }

                roomView = new RoomWindow();
                roomView.SetChatView(this);
                roomView.Show();
                //roomView.WindowState = System.Windows.WindowState.Normal;
                roomView.Focus();
                roomController = new RoomController(roomView);
                roomController.LoadView();

                this.WindowState = System.Windows.WindowState.Minimized;
            }));
        }
        #endregion

        private void tabrecentText_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.gridListContainer.RowDefinitions[1].Height = new GridLength(100, GridUnitType.Star);
            this.gridListContainer.RowDefinitions[3].Height = new GridLength(0);
            this.gridListContainer.RowDefinitions[5].Height = new GridLength(0);
        }

        private void tabfriendText_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.gridListContainer.RowDefinitions[3].Height = new GridLength(100, GridUnitType.Star);
            this.gridListContainer.RowDefinitions[1].Height = new GridLength(0);
            this.gridListContainer.RowDefinitions[5].Height = new GridLength(0);
        }

        private void tabIgnoretext_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.gridListContainer.RowDefinitions[5].Height = new GridLength(100, GridUnitType.Star);
            this.gridListContainer.RowDefinitions[3].Height = new GridLength(0);
            this.gridListContainer.RowDefinitions[1].Height = new GridLength(0);
        }


        #region CHange state of user menu
        private void WrapPanel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ContextMenu cm = this.FindResource("cmStateChanging") as ContextMenu;
            cm.PlacementTarget = sender as WrapPanel;
            cm.IsOpen = true;
        }

        private void availStripMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (StaticValue.state == 1) return;
            StaticValue.state = 1;
            this.userState.Source = SystemUtils.getResource("state_avail");
            chatController.UpdateStatus(1, null);
        }

        private void inviStripMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (StaticValue.state == 0) return;
            StaticValue.state = 0;
            this.userState.Source = SystemUtils.getResource("state_invi");
            chatController.UpdateStatus(0, null);
        }

        private void logoutStripMenuItem_Click(object sender, RoutedEventArgs e)
        {
            this.Logout();
        }

        private void exitStripMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (System.Windows.Forms.Application.MessageLoop)
            {
                // WinForms app
                System.Windows.Forms.Application.Exit();
            }
            else
            {
                // Console app
                System.Environment.Exit(1);
            }
        }
        #endregion

        private void userStatus_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                e.Handled = true;
                chatController.UpdateStatus(1, this.userStatus.Text);
                StaticValue.status = this.userStatus.Text;
                Keyboard.ClearFocus();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            addFriendView = new AddFriendWindow();
            addFriendView.SetController(chatController);

            addFriendView.ShowDialog();
        }

    }
}