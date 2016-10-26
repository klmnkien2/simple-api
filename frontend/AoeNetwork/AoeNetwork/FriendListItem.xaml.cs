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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AoeNetwork
{
    /// <summary>
    /// Interaction logic for FriendListItem.xaml
    /// </summary>
    public partial class FriendListItem : UserControl
    {
        public FriendListItem()
        {
            InitializeComponent();
        }

        class DataItem
        {
            public Image state;
            public Image avatar;
            public Label status;
            public Grid container;
            public Friend user;
        }

        Dictionary<int, DataItem> dataItems = new Dictionary<int, DataItem>();
        ChatWindow formClass;

        ArrayList _IDs = new ArrayList();
        public ArrayList IDs
        {
            get
            {
                return this._IDs;
            }
        }
        private int _SelectedContextId = -1;
        public int SelectedContextId
        {
            get
            {
                return _SelectedContextId;
            }
            set
            {
                _SelectedContextId = value;
            }
        }

        public Friend SelectedContextData
        {
            get
            {
                DataItem existedItem = null;
                this.dataItems.TryGetValue(this._SelectedContextId, out existedItem);
                if (existedItem != null)
                {
                    return existedItem.user;
                }
                else
                {
                    return null;
                }
            }
        }

        private Friend _SelectedClickData = null;
        public Friend SelectedClickData
        {
            get
            {
                return _SelectedClickData;
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
                    this.container.Children.Remove(existedItem.container);
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
                    }

                    if (user.status != existedUser.status)
                    {
                        existedUser.status = user.status;
                        existedItem.status.Content = user.status;
                    }

                    if (user.avatar != existedUser.avatar)
                    {
                        existedUser.avatar = user.avatar;

                        BitmapImage bitmap = new BitmapImage();
                        bitmap.BeginInit();
                        bitmap.UriSource = new Uri(user.avatar, UriKind.RelativeOrAbsolute);
                        bitmap.CacheOption = BitmapCacheOption.OnLoad;
                        bitmap.EndInit();
                        existedItem.avatar.Source = bitmap;
                    }
                }

                return; // da ton tai thi next luon
            }

            Label name = new Label();
            name.Tag = user.user_id;
            Grid.SetRow(name, 0);
            Grid.SetColumn(name, 1);

            Label status = new Label();
            status.Tag = user.user_id;
            Grid.SetRow(status, 1);
            Grid.SetColumn(status, 1);
            //status.MouseDown += new System.Windows.Forms.MouseEventHandler(this.listItem_MouseDown);
            //status.MouseDoubleClick += new MouseEventHandler(this.listItem_MouseDoubleClick);

            Image state = new Image();
            Grid.SetRow(state, 0);
            Grid.SetColumn(state, 2);
            Grid.SetRowSpan(state, 2);
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
            }

            Image avatar = new Image();
            avatar.Source = (ImageSource)Resources["user"];
            Grid.SetRow(avatar, 0);
            Grid.SetColumn(avatar, 0);
            Grid.SetRowSpan(avatar, 2);

            Grid itemPanel = new Grid();
            itemPanel.Children.Clear();

            // Adding Rows and Colums to Grid.
            RowDefinition[] rows = new RowDefinition[2];
            ColumnDefinition[] columns = new ColumnDefinition[3];
            // Draw Rows.
            rows[0] = new RowDefinition();
            rows[0].Height = new GridLength(25);
            itemPanel.RowDefinitions.Add(rows[0]);

            rows[1] = new RowDefinition();
            rows[1].Height = new GridLength(25);
            itemPanel.RowDefinitions.Add(rows[1]);

            // Draw Columns.
            columns[0] = new ColumnDefinition();
            columns[0].Width = new GridLength(50);
            itemPanel.ColumnDefinitions.Add(columns[0]);

            columns[1] = new ColumnDefinition();
            columns[1].Width = new GridLength(100, GridUnitType.Star);
            itemPanel.ColumnDefinitions.Add(columns[1]);

            columns[2] = new ColumnDefinition();
            columns[2].Width = new GridLength(40);
            itemPanel.ColumnDefinitions.Add(columns[2]);

            itemPanel.Width = this.container.Width;
            itemPanel.Height = 50;
            
            itemPanel.Children.Add(avatar);
            itemPanel.Children.Add(name);
            itemPanel.Children.Add(status);
            itemPanel.Children.Add(state);

            DataItem dataItem = new DataItem();
            dataItem.state = state;
            dataItem.avatar = avatar;
            dataItem.status = status;
            dataItem.user = user;
            dataItem.container = itemPanel;

            this.dataItems.Add(user.user_id, dataItem);
            this.IDs.Add(user.user_id);
            this.container.Children.Add(itemPanel);
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
