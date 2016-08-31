using System;
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

            //privateController = new PrivateController(view);
            //privateController.LoadView();

            initFriendLists();
        }

        #region action for extend MainWindow (title bar, border, ...)
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
        //PrivateController privateController = null;
        //RoomController roomController = null;

        RoomWindow roomView = null;
        AddFriendWindow addFriendView;
        LoginWindow loginView;

        Dictionary<int, PrivateWindow> privateViews = new Dictionary<int, PrivateWindow>();
        Dictionary<int, Friend> mapFriend = new Dictionary<int, Friend>();


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
            this.userStatus.Text = status;
            this.userLevel.Content = "Level " + level;
            this.userDiamond.Content = "" + level;
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
                userState.Source = (ImageSource)Resources["state_invi"];
            }
            else
            {
                userState.Source = (ImageSource)Resources["state_avail"];
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
                //roomView.SetChatView(this);
                roomView.Show();
                roomView.Focus();
                //roomController = new RoomController(roomView);
                //roomController.LoadView();
            }));
        }
    }
}
