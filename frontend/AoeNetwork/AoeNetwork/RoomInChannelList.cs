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
    class RoomInChannelList
    {
        public RoomInChannelList(RoomWindow parentWindow, StackPanel parentList)
        {
            this.parentList = parentList;
            this.parentWindow = parentWindow;
            this.addHeader();
        }

        class DataItem
        {
            public Label name;
            public Label minLevel;
            public Label member;
            public Border container;
            public Room room;
        }

        SolidColorBrush borderColor = (SolidColorBrush)(new BrushConverter().ConvertFrom("#3c444a"));
        SolidColorBrush textColor = (SolidColorBrush)(new BrushConverter().ConvertFrom("#828282"));
        SolidColorBrush bgColor = (SolidColorBrush)(new BrushConverter().ConvertFrom("#031012"));

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

        public void clearData()
        {
            _IDs = new ArrayList();
            dataItems = new Dictionary<int, DataItem>();
            this.parentList.Children.Clear();
            addHeader();
        }

        public void deleteItem(int item_id)
        {
            if (dataItems.ContainsKey(item_id))
            {
                DataItem existedItem = null;
                dataItems.TryGetValue(item_id, out existedItem);
                if (existedItem != null)
                {
                    this.parentList.Children.Remove(existedItem.container);
                    this.dataItems.Remove(item_id);
                    this.IDs.Remove(item_id);
                }
            }
        }

        public void addHeader()
        {
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
            columns[0].Width = new GridLength(40, GridUnitType.Star);
            itemPanel.ColumnDefinitions.Add(columns[0]);

            columns[1] = new ColumnDefinition();
            columns[1].Width = new GridLength(30, GridUnitType.Star);
            itemPanel.ColumnDefinitions.Add(columns[1]);

            columns[2] = new ColumnDefinition();
            columns[2].Width = new GridLength(30, GridUnitType.Star);
            itemPanel.ColumnDefinitions.Add(columns[2]);   


            Label name = new Label();
            name.Foreground = Brushes.White;
            name.VerticalContentAlignment = VerticalAlignment.Bottom;
            name.FontWeight = FontWeights.Bold;
            name.Content = "Phòng Game";
            itemPanel.Children.Add(name);
            dataItem.name = name;

            Label member = new Label();
            member.Foreground = Brushes.White;
            member.VerticalContentAlignment = VerticalAlignment.Bottom;
            member.HorizontalContentAlignment = HorizontalAlignment.Center;
            member.FontWeight = FontWeights.Bold;
            member.Content = "Số lượng";
            itemPanel.Children.Add(member);
            dataItem.member = member;
            Grid.SetColumn(member, 2);

            Label minLevel = new Label();
            minLevel.Foreground = Brushes.White;
            minLevel.VerticalContentAlignment = VerticalAlignment.Bottom;
            minLevel.HorizontalContentAlignment = HorizontalAlignment.Center;
            name.FontWeight = FontWeights.Bold;
            minLevel.Content = "Cấp độ đầu vào";
            itemPanel.Children.Add(minLevel);
            dataItem.minLevel = minLevel;
            Grid.SetColumn(minLevel, 1);

            Border itemBorder = new Border();
            itemBorder.BorderThickness = new Thickness(1, 1, 1, 1);
            itemBorder.BorderBrush = borderColor;
            itemBorder.Background = bgColor;
            itemBorder.Child = itemPanel;
            dataItem.container = itemBorder;

            this.parentList.Children.Add(itemBorder);
        }

        public void addItem(Room itemObj)
        {
            if (dataItems.ContainsKey(itemObj.room_id))
            {

                DataItem existedItem = null;
                dataItems.TryGetValue(itemObj.room_id, out existedItem);
                if (existedItem != null)
                {
                    //Update object
                    Room existedObj = existedItem.room;

                    //Update GUI
                    if (itemObj.room_name != existedObj.room_name)
                    {
                        existedObj.room_name = itemObj.room_name;
                        existedItem.name.Content = itemObj.room_name;
                    }

                    if (itemObj.maximum != existedObj.maximum || itemObj.members != existedObj.members)
                    {
                        existedObj.members = itemObj.members;
                        existedObj.maximum = itemObj.maximum;
                        existedItem.member.Content = itemObj.members + "/" + itemObj.maximum;
                    }
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
            columns[0].Width = new GridLength(40, GridUnitType.Star);
            itemPanel.ColumnDefinitions.Add(columns[0]);

            columns[1] = new ColumnDefinition();
            columns[1].Width = new GridLength(30, GridUnitType.Star);
            itemPanel.ColumnDefinitions.Add(columns[1]);

            columns[2] = new ColumnDefinition();
            columns[2].Width = new GridLength(30, GridUnitType.Star);
            itemPanel.ColumnDefinitions.Add(columns[2]);            
            
            Label name = new Label();
            name.Foreground = textColor;
            name.VerticalContentAlignment = VerticalAlignment.Bottom;
            name.Content = itemObj.room_name;
            itemPanel.Children.Add(name);
            dataItem.name = name;

            Label member = new Label();
            member.Foreground = textColor;
            member.VerticalContentAlignment = VerticalAlignment.Bottom;
            member.HorizontalContentAlignment = HorizontalAlignment.Center;
            member.Content = itemObj.members + "/" + itemObj.maximum;
            itemPanel.Children.Add(member);
            dataItem.member = member;
            Grid.SetColumn(member, 2);

            Label minLevel = new Label();
            minLevel.Foreground = textColor;
            minLevel.VerticalContentAlignment = VerticalAlignment.Bottom;
            minLevel.HorizontalContentAlignment = HorizontalAlignment.Center;
            minLevel.Content = itemObj.level;
            itemPanel.Children.Add(minLevel);
            dataItem.minLevel = minLevel;
            Grid.SetColumn(minLevel, 1);

            Border itemBorder = new Border();
            itemBorder.BorderThickness = new Thickness(1,0,1,1);
            itemBorder.BorderBrush = borderColor;
            itemBorder.Background = bgColor;
            itemBorder.Child = itemPanel;
            itemBorder.Tag = itemObj;
            itemBorder.Cursor = Cursors.Hand;
            itemBorder.ToolTip = "Double Click để chọn phòng này";
            itemBorder.MouseLeftButtonDown += itemPanel_MouseLeftButtonDown;
            itemBorder.MouseRightButtonDown += itemPanel_MouseLeftButtonDown;
            dataItem.container = itemBorder;
            dataItem.room = itemObj;

            this.dataItems.Add(itemObj.room_id, dataItem);
            this.IDs.Add(itemObj.room_id);
            this.parentList.Children.Add(itemBorder);
        }

        void itemPanel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                Room room = (sender as Border).Tag as Room;
                try
                {
                    if (int.Parse(StaticValue.level) < int.Parse(room.level))
                    {
                        throw new Exception();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Level của bạn chưa đạt yêu cầu của phòng.");
                    return;
                }

                if (room != null)
                {
                    parentWindow.JoinARoom(room);
                }
            }
        }

    }
}
