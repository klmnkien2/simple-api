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
        }

        private ContextMenu contextMenu;
        MenuItem showUsername;
        MenuItem showUserIp;
        MenuItem copyIpMenu;
        private void InitContextMenu()
        {
            contextMenu = new ContextMenu();
            
            showUsername = new MenuItem();
            showUsername.IsEnabled = false;
            showUserIp = new MenuItem();
            showUserIp.IsEnabled = false;
            copyIpMenu = new MenuItem();
            copyIpMenu.Header = "Copy IP Address";
            copyIpMenu.Click += copyIpMenu_Click;

            contextMenu.Items.Add(showUsername);
            contextMenu.Items.Add(showUserIp);
            contextMenu.Items.Add(new Separator());
            contextMenu.Items.Add(copyIpMenu);
        }

        void copyIpMenu_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.Clipboard.SetText(copyIpMenu.Header as string);
        }

        private void listItem_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Grid container = sender as Grid;
            UserCache user = (UserCache)container.Tag;
            contextMenu.PlacementTarget = container;
            showUsername.Header = user.user_name;
            showUserIp.Header = user.ip;
            contextMenu.IsOpen = true;
        }

        class DataItem
        {
            public Image state;
            public Grid container;
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

        public void addUser(UserCache user)
        {
            if (dataItems.ContainsKey(user.user_id))
            {

                DataItem existedItem = null;
                dataItems.TryGetValue(user.user_id, out existedItem);
                if (existedItem != null)
                {
                    //Update object
                    UserCache existedUser = existedItem.user;

                    //Update GUI
                    if (user.state != existedUser.state)
                    {
                        existedUser.state = user.state;
                        if (user.state == 1)
                        {
                            existedItem.state.Source = getResource("state_avail"); ;
                        }
                        else
                        {
                            existedItem.state.Source = getResource("state_invi");
                        }
                    }
                }

                return; // da ton tai thi next luon
            }

            Label indexLbl = new Label();
            indexLbl.Content = (_IDs.Count + 1) + ".";
            indexLbl.VerticalContentAlignment = VerticalAlignment.Center;
            indexLbl.Foreground = Brushes.White;
            indexLbl.FontWeight = FontWeights.Bold;
            Grid.SetRow(indexLbl, 0);
            Grid.SetColumn(indexLbl, 0);

            Label name = new Label();
            name.Foreground = Brushes.White;
            name.VerticalContentAlignment = VerticalAlignment.Center;
            name.FontWeight = FontWeights.Bold;
            name.Tag = user.user_id;
            name.Content = user.user_name;
            Grid.SetRow(name, 0);
            Grid.SetColumn(name, 1);

            Image state = new Image();
            state.StretchDirection = StretchDirection.Both;
            state.Width = 12;
            state.Height = 12;
            Grid.SetRow(state, 0);
            Grid.SetColumn(state, 2);
            if (user.state == 1)
            {
                state.Source = getResource("state_avail");
            }
            else
            {
                state.Source = getResource("state_invi");
            }

            Grid itemPanel = new Grid();
            itemPanel.Tag = user;
            itemPanel.Children.Clear();
            itemPanel.MouseDown += this.listItem_MouseDown;

            // Adding Rows and Colums to Grid.
            RowDefinition[] rows = new RowDefinition[1];
            ColumnDefinition[] columns = new ColumnDefinition[3];
            // Draw Rows.
            rows[0] = new RowDefinition();
            rows[0].Height = new GridLength(30);
            itemPanel.RowDefinitions.Add(rows[0]);

            // Draw Columns.
            columns[0] = new ColumnDefinition();
            columns[0].Width = new GridLength(40);
            itemPanel.ColumnDefinitions.Add(columns[0]);

            columns[1] = new ColumnDefinition();
            columns[1].Width = new GridLength(100, GridUnitType.Star);
            itemPanel.ColumnDefinitions.Add(columns[1]);

            columns[2] = new ColumnDefinition();
            columns[2].Width = new GridLength(40);
            itemPanel.ColumnDefinitions.Add(columns[2]);

            itemPanel.Width = this.parentList.Width;
            itemPanel.Height = 50;

            itemPanel.Children.Add(indexLbl);
            itemPanel.Children.Add(name);
            itemPanel.Children.Add(state);

            DataItem dataItem = new DataItem();
            dataItem.state = state;
            dataItem.user = user;
            dataItem.container = itemPanel;

            this.dataItems.Add(user.user_id, dataItem);
            this.IDs.Add(user.user_id);
            this.parentList.Children.Add(itemPanel);
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

        private BitmapImage getResource(string name)
        {
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri("pack://application:,,,/Images/" + name + ".png");
            bitmap.EndInit();

            return bitmap;
        }
    }
}
