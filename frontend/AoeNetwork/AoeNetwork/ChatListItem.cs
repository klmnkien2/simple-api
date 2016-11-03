using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace AoeNetwork
{
    class ChatListItem
    {
        public ChatListItem(ChatWindow parentWindow, StackPanel parentList)
        {
            this.parentList = parentList;
            this.parentWindow = parentWindow;

            InitContextMenu();
        }

        class DataItem
        {
            public Image state;
            public Image avatar;
            public Label status;
            public Border container;
            public Friend user;
        }

        Dictionary<int, DataItem> dataItems = new Dictionary<int, DataItem>();
        ChatWindow parentWindow;
        StackPanel parentList;

        ArrayList _IDs = new ArrayList();
        public ArrayList IDs
        {
            get
            {
                return this._IDs;
            }
        }

        private ContextMenu contextMenu;
        MenuItem acceptFriendMenu;
        MenuItem denyFriendMenu;
        MenuItem removeFriendMenu;
        MenuItem ignoreFriendMenu;
        MenuItem noIgonreFriendMenu;
        private void InitContextMenu()
        {
            Style styleMenu = parentWindow.FindResource("customContextMenu") as Style;
            Style styleMenuItem = parentWindow.FindResource("customContextMenuItem") as Style;

            contextMenu = new ContextMenu();
            contextMenu.Style = styleMenu;

            acceptFriendMenu = new MenuItem();
            acceptFriendMenu.Style = styleMenuItem;
            acceptFriendMenu.Tag = "Images/arrow_right.png";
            acceptFriendMenu.Header = "Đồng ý kết bạn";
            acceptFriendMenu.Click += acceptFriendItem_Click;

            denyFriendMenu = new MenuItem();
            denyFriendMenu.Style = styleMenuItem;
            denyFriendMenu.Tag = "Images/arrow_right.png";
            denyFriendMenu.Header = "Từ chối kết bạn";
            denyFriendMenu.Click += denyFriendItem_Click;

            removeFriendMenu = new MenuItem();
            removeFriendMenu.Style = styleMenuItem;
            removeFriendMenu.Tag = "Images/arrow_right.png";
            removeFriendMenu.Header = "Hủy kết bạn";
            removeFriendMenu.Click += removeFriendItem_Click;

            ignoreFriendMenu = new MenuItem();
            ignoreFriendMenu.Style = styleMenuItem;
            ignoreFriendMenu.Tag = "Images/arrow_right.png";
            ignoreFriendMenu.Header = "Chặn người này";
            ignoreFriendMenu.Click += ignoreFriendItem_Click;

            noIgonreFriendMenu = new MenuItem();
            noIgonreFriendMenu.Style = styleMenuItem;
            noIgonreFriendMenu.Tag = "Images/arrow_right.png";
            noIgonreFriendMenu.Header = "Bỏ chặn";
            noIgonreFriendMenu.Click += removeIgnoreItem_Click;

            contextMenu.Items.Add(acceptFriendMenu);
            contextMenu.Items.Add(denyFriendMenu);
            contextMenu.Items.Add(removeFriendMenu);
            contextMenu.Items.Add(ignoreFriendMenu);
            contextMenu.Items.Add(noIgonreFriendMenu);
        }

        public void deleteUser(int user_id)
        {
            if (dataItems.ContainsKey(user_id))
            {
                DataItem existedItem = null;
                dataItems.TryGetValue(user_id, out existedItem);
                if (existedItem != null)
                {
                    this.parentList.Children.Remove(existedItem.container);
                    this.dataItems.Remove(user_id);
                    this.IDs.Remove(user_id);
                }
            }
        }

        public void addUser(Friend user)
        {
            if (dataItems.ContainsKey(user.user_id))
            {

                DataItem existedItem = null;
                dataItems.TryGetValue(user.user_id, out existedItem);
                if (existedItem != null)
                {
                    //Update object
                    Friend existedUser = existedItem.user;

                    //Update GUI

                    if (user.status != existedUser.status)
                    {
                        existedUser.status = user.status;
                        existedItem.status.Content = user.status;
                    }

                    if (user.avatar != existedUser.avatar)
                    {
                        existedUser.avatar = user.avatar;
                        existedItem.avatar.Source = SystemUtils.getImageUrl(user.avatar);
                    }

                    if (user.state != existedUser.state)
                    {
                        existedUser.state = user.state;
                        if (user.state == 1)
                        {
                            existedItem.state.Source = SystemUtils.getResource("login_avail");
                            existedItem.status.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#79942c"));
                        }
                        else if (user.state == 2)
                        {
                            existedItem.state.Source = SystemUtils.getResource("login_busy");
                            existedItem.status.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#858889"));
                        }
                        else
                        {
                            existedItem.state.Source = SystemUtils.getResource("login_offline");
                            existedItem.status.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#858889"));
                        }
                    }
                }

                return; // da ton tai thi next luon
            }

            Label saparateLine = new Label();
            saparateLine.HorizontalAlignment = HorizontalAlignment.Stretch;
            saparateLine.VerticalAlignment = VerticalAlignment.Stretch;
            saparateLine.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#122937"));
            Grid.SetRow(saparateLine, 2);
            Grid.SetColumnSpan(saparateLine, 3);

            Label name = new Label();
            name.Foreground = Brushes.White;
            name.VerticalContentAlignment = VerticalAlignment.Bottom;
            name.FontWeight = FontWeights.Bold;
            name.Tag = user.user_id;
            name.Content = user.user_name;
            Grid.SetRow(name, 0);
            Grid.SetColumn(name, 1);

            Label status = new Label();
            status.VerticalContentAlignment = VerticalAlignment.Top;
            status.Foreground = Brushes.White;
            status.FontSize = status.FontSize - 1;
            status.Tag = user.user_id;
            status.Content = user.status;
            Grid.SetRow(status, 1);
            Grid.SetColumn(status, 1);

            Image state = new Image();
            state.StretchDirection = StretchDirection.Both;
            state.Width = 12;
            state.Height = 12;
            Grid.SetRow(state, 0);
            Grid.SetColumn(state, 2);
            Grid.SetRowSpan(state, 2);
            if (user.state == 1)
            {
                state.Source = SystemUtils.getResource("login_avail");
                status.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#79942c"));
            }
            else if (user.state == 2)
            {
                state.Source = SystemUtils.getResource("login_busy");
                status.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#858889"));
            }
            else
            {
                state.Source = SystemUtils.getResource("login_offline");
                status.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#858889"));
            }

            Image avatar = new Image();
            avatar.Margin = new Thickness(6, 6, 6, 6);
            avatar.Source = SystemUtils.getResource("user");
            Grid.SetRow(avatar, 0);
            Grid.SetColumn(avatar, 0);
            Grid.SetRowSpan(avatar, 2);

            Grid itemPanel = new Grid();
            itemPanel.Children.Clear();

            // Adding Rows and Colums to Grid.
            RowDefinition[] rows = new RowDefinition[3];
            ColumnDefinition[] columns = new ColumnDefinition[3];
            // Draw Rows.

            rows[0] = new RowDefinition();
            rows[0].Height = new GridLength(30);
            itemPanel.RowDefinitions.Add(rows[0]);

            rows[1] = new RowDefinition();
            rows[1].Height = new GridLength(30);
            itemPanel.RowDefinitions.Add(rows[1]);

            rows[2] = new RowDefinition();
            rows[2].Height = new GridLength(1);
            itemPanel.RowDefinitions.Add(rows[2]);

            // Draw Columns.
            columns[0] = new ColumnDefinition();
            columns[0].Width = new GridLength(60);
            itemPanel.ColumnDefinitions.Add(columns[0]);

            columns[1] = new ColumnDefinition();
            columns[1].Width = new GridLength(100, GridUnitType.Star);
            itemPanel.ColumnDefinitions.Add(columns[1]);

            columns[2] = new ColumnDefinition();
            columns[2].Width = new GridLength(40);
            itemPanel.ColumnDefinitions.Add(columns[2]);

            itemPanel.Width = this.parentList.Width;
            itemPanel.Height = 61;
                       
            itemPanel.Children.Add(avatar);
            itemPanel.Children.Add(name);
            itemPanel.Children.Add(status);
            itemPanel.Children.Add(state);
            itemPanel.Children.Add(saparateLine);

            Border itemBorder = new Border();
            itemBorder.BorderThickness = new Thickness(1,1,1,0);
            itemBorder.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#0e1e28"));
            itemBorder.BorderBrush = (SolidColorBrush)(new BrushConverter().ConvertFrom("#010202"));
            itemBorder.Child = itemPanel;
            itemBorder.Tag = user;
            itemBorder.MouseLeftButtonDown += itemPanel_MouseLeftButtonDown;
            itemBorder.MouseRightButtonDown += itemPanel_MouseRightButtonDown;

            DataItem dataItem = new DataItem();
            dataItem.state = state;
            dataItem.avatar = avatar;
            dataItem.status = status;
            dataItem.user = user;
            dataItem.container = itemBorder;

            this.dataItems.Add(user.user_id, dataItem);
            this.IDs.Add(user.user_id);
            this.parentList.Children.Add(itemBorder);
        }

        void itemPanel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                Friend user = (sender as Border).Tag as Friend;
                if (user != null)
                {
                    parentWindow.OpenPrivate(user, true);
                }
            }
        }

        private Friend selectContextItem = null;
        void itemPanel_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            //context menu stuff
            Border container = sender as Border;
            selectContextItem = container.Tag as Friend;

            // CHeck button showing
            contextMenu.Items.Clear();
            if (selectContextItem.type == 1)
            {
                contextMenu.Items.Add(removeFriendMenu);
                contextMenu.Items.Add(ignoreFriendMenu);
            }
            else if (selectContextItem.type == 0)
            {
                if (selectContextItem.from != selectContextItem.user_id)
                {
                    contextMenu.Items.Add(acceptFriendMenu);
                    contextMenu.Items.Add(denyFriendMenu);
                }
                contextMenu.Items.Add(ignoreFriendMenu);
            }
            else if (selectContextItem.type == -1)
            {
                contextMenu.Items.Add(noIgonreFriendMenu);
            }

            contextMenu.PlacementTarget = container;            
            contextMenu.IsOpen = true;
        }

        public void SearchByString(string searchKey)
        {
            foreach (KeyValuePair<int, DataItem> entry in dataItems)
            {
                DataItem item = entry.Value;
                if (!item.user.user_name.Contains(searchKey))
                {
                    item.container.Height = 0;
                }
            }
        }

        public void ResetSearch()
        {
            foreach (KeyValuePair<int, DataItem> entry in dataItems)
            {
                DataItem item = entry.Value;
                item.container.Height = 62;
            }
        }

        #region contextmenu action
        private void acceptFriendItem_Click(object sender, EventArgs e)
        {
            Friend friend = this.selectContextItem;
            if (friend != null)
            {
                parentWindow.getPrivateController().UpdateFriendStatus(friend, 1, 0);
            }
        }

        private void denyFriendItem_Click(object sender, EventArgs e)
        {
            Friend friend = this.selectContextItem;
            if (friend != null)
            {
                parentWindow.getPrivateController().UpdateFriendStatus(friend, -2, 0);
            }
        }

        private void removeFriendItem_Click(object sender, EventArgs e)
        {
            Friend friend = this.selectContextItem;
            if (friend != null)
            {
                parentWindow.getPrivateController().UpdateFriendStatus(friend, -2, 0);
            }
        }

        private void ignoreFriendItem_Click(object sender, EventArgs e)
        {
            Friend friend = this.selectContextItem;
            if (friend != null)
            {
                parentWindow.getPrivateController().UpdateFriendStatus(friend, -1, -2);
            }
        }

        private void removeIgnoreItem_Click(object sender, EventArgs e)
        {
            Friend friend = this.selectContextItem;
            if (friend != null)
            {
                parentWindow.getPrivateController().UpdateFriendStatus(friend, 0, -2);
            }
        }
        #endregion
    }
}
