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
    class RoomListItem
    {
        public RoomListItem(RoomWindow parentWindow, StackPanel parentList)
        {
            this.parentList = parentList;
            this.parentWindow = parentWindow;
            InitContextMenu();
            addHeader();
        }

        SolidColorBrush borderColor = (SolidColorBrush)(new BrushConverter().ConvertFrom("#3c444a"));

        private ContextMenu contextMenu;
        MenuItem showUsername;
        MenuItem showUserIp;
        MenuItem copyIpMenu;
        private void InitContextMenu()
        {
            Style styleMenu = parentWindow.FindResource("customContextMenu") as Style;
            Style styleMenuItem = parentWindow.FindResource("customContextMenuItem") as Style;

            contextMenu = new ContextMenu();
            contextMenu.Style = styleMenu;
            
            showUsername = new MenuItem();
            showUsername.Foreground = Brushes.Red;
            showUsername.Style = styleMenuItem;
            showUsername.Tag = "Images/arrow_right.png";

            showUserIp = new MenuItem();
            showUserIp.Style = styleMenuItem;
            showUserIp.Foreground = Brushes.Red;
            showUserIp.Tag = "Images/arrow_right.png";

            copyIpMenu = new MenuItem();
            copyIpMenu.Header = "Copy IP Address";
            copyIpMenu.Style = styleMenuItem;
            copyIpMenu.Tag = "Images/arrow_right.png";
            copyIpMenu.Click += copyIpMenu_Click;

            contextMenu.Items.Add(showUsername);
            contextMenu.Items.Add(showUserIp);            
            contextMenu.Items.Add(copyIpMenu);
        }

        void copyIpMenu_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.Clipboard.SetText(copyIpMenu.Header as string);
        }

        private void listItem_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Border container = sender as Border;
            UserCache user = (UserCache)container.Tag;
            contextMenu.PlacementTarget = container;
            showUsername.Header = user.user_name;
            showUserIp.Header = user.ip;
            contextMenu.IsOpen = true;
        }

        class DataItem
        {
            public Image state;
            public Label ping;
            public Border container;
            public UserCache user;
        }

        Dictionary<int, DataItem> dataItems = new Dictionary<int, DataItem>();
        RoomWindow parentWindow;
        StackPanel parentList;

        ArrayList _IDs = new ArrayList();
        public ArrayList IDs
        {
            get
            {
                return this._IDs;
            }
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

        public void addHeader()
        {
            DataItem dataItem = new DataItem();
            Grid itemPanel = new Grid();
            itemPanel.Children.Clear();
            itemPanel.Width = this.parentList.Width;
            itemPanel.Height = 30;

            // Adding Rows and Colums to Grid.
            RowDefinition[] rows = new RowDefinition[1];
            ColumnDefinition[] columns = new ColumnDefinition[3];
            // Draw Rows.

            rows[0] = new RowDefinition();
            rows[0].Height = new GridLength(100, GridUnitType.Star);
            itemPanel.RowDefinitions.Add(rows[0]);

            // Draw Columns.
            columns[0] = new ColumnDefinition();
            columns[0].Width = new GridLength(50);
            itemPanel.ColumnDefinitions.Add(columns[0]);

            columns[1] = new ColumnDefinition();
            columns[1].Width = new GridLength(100, GridUnitType.Star);
            itemPanel.ColumnDefinitions.Add(columns[1]);

            columns[2] = new ColumnDefinition();
            columns[2].Width = new GridLength(50);
            itemPanel.ColumnDefinitions.Add(columns[2]);

            Label level = new Label();
            level.Foreground = Brushes.WhiteSmoke;
            level.Content = "Bậc";// itemObj.level;
            level.VerticalContentAlignment = VerticalAlignment.Center;
            level.FontWeight = FontWeights.Bold;
            itemPanel.Children.Add(level);

            Label name = new Label();
            name.Foreground = Brushes.WhiteSmoke;
            name.Content = "Tên tài khoản";
            name.VerticalContentAlignment = VerticalAlignment.Center;
            name.FontWeight = FontWeights.Bold;
            itemPanel.Children.Add(name);
            Grid.SetColumn(name, 1);

            Label ping = new Label();
            ping.Foreground = Brushes.WhiteSmoke;
            ping.Content = "Ping";// itemObj.ping;
            ping.VerticalContentAlignment = VerticalAlignment.Center;
            ping.FontWeight = FontWeights.Bold;
            itemPanel.Children.Add(ping);
            dataItem.ping = ping;
            Grid.SetColumn(ping, 2);
            
            Border itemBorder = new Border();
            itemBorder.BorderThickness = new Thickness(1, 1, 1, 1);
            itemBorder.BorderBrush = borderColor;
            itemBorder.Child = itemPanel;

            this.parentList.Children.Add(itemBorder);
        }

        public void addUser(UserCache itemObj)
        {
            if (dataItems.ContainsKey(itemObj.user_id))
            {

                DataItem existedItem = null;
                dataItems.TryGetValue(itemObj.user_id, out existedItem);
                if (existedItem != null)
                {
                    //Update object
                    UserCache existedUser = existedItem.user;
                    
                    // Update ping
                    //if (itemObj.ping != existedUser.ping)
                    //{
                    //}

                    /*
                    //Update GUI
                    if (user.state != existedUser.state)
                    {
                        existedUser.state = user.state;
                        if (user.state == 1)
                        {
                            existedItem.state.Source = SystemUtils.getResource("login_avail");
                        }
                        else if (user.state == 2)
                        {
                            existedItem.state.Source = SystemUtils.getResource("login_busy");
                        }
                        else
                        {
                            existedItem.state.Source = SystemUtils.getResource("login_offline");
                        }
                    }*/
                }

                return; // da ton tai thi next luon
            }

            DataItem dataItem = new DataItem();
            Grid itemPanel = new Grid();
            itemPanel.Children.Clear();
            itemPanel.Width = this.parentList.Width;
            itemPanel.Height = 25;

            // Adding Rows and Colums to Grid.
            RowDefinition[] rows = new RowDefinition[1];
            ColumnDefinition[] columns = new ColumnDefinition[3];
            // Draw Rows.

            rows[0] = new RowDefinition();
            rows[0].Height = new GridLength(100, GridUnitType.Star);
            itemPanel.RowDefinitions.Add(rows[0]);

            // Draw Columns.
            columns[0] = new ColumnDefinition();
            columns[0].Width = new GridLength(50);
            itemPanel.ColumnDefinitions.Add(columns[0]);

            columns[1] = new ColumnDefinition();
            columns[1].Width = new GridLength(100, GridUnitType.Star);
            itemPanel.ColumnDefinitions.Add(columns[1]);

            columns[2] = new ColumnDefinition();
            columns[2].Width = new GridLength(50);
            itemPanel.ColumnDefinitions.Add(columns[2]);

            Label level = new Label();
            level.Foreground = Brushes.WhiteSmoke;
            level.Content = itemObj.level;
            itemPanel.Children.Add(level);

            Label name = new Label();
            name.Foreground = Brushes.WhiteSmoke;
            name.Content = itemObj.user_name;
            itemPanel.Children.Add(name);
            Grid.SetColumn(name, 1);

            Label ping = new Label();
            ping.Foreground = Brushes.WhiteSmoke;
            ping.Content = itemObj.ping;
            itemPanel.Children.Add(ping);
            dataItem.ping = ping;
            Grid.SetColumn(ping, 2);

            #region state of user , not use now
            /*Image state = new Image();
            state.StretchDirection = StretchDirection.Both;
            state.Width = 12;
            state.Height = 12;
            Grid.SetRow(state, 0);
            Grid.SetColumn(state, 2);
            if (user.state == 1)
            {
                state.Source = SystemUtils.getResource("login_avail");
            }
            else if (user.state == 2)
            {
                state.Source = SystemUtils.getResource("login_busy");
            }
            else
            {
                state.Source = SystemUtils.getResource("login_offline");
            }*/
            #endregion

            Border itemBorder = new Border();
            itemBorder.BorderThickness = new Thickness(1, 0, 1, 1);
            itemBorder.BorderBrush = borderColor;
            itemBorder.Child = itemPanel;
            itemBorder.Tag = itemObj;
            itemBorder.ToolTip = "Chuột phải để chọn copy IP";
            itemBorder.MouseRightButtonDown += listItem_MouseDown;
            dataItem.container = itemBorder;
            dataItem.user = itemObj;

            this.dataItems.Add(itemObj.user_id, dataItem);
            this.IDs.Add(itemObj.user_id);
            this.parentList.Children.Add(itemBorder);
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
                item.container.Height = 50;
            }
        }
    }
}
