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
    class ChannelListItem
    {
        public ChannelListItem(RoomWindow parentWindow, StackPanel parentList)
        {
            this.parentList = parentList;
            this.parentWindow = parentWindow;

        }

        SolidColorBrush borderColor = (SolidColorBrush)(new BrushConverter().ConvertFrom("#3c444a"));
        SolidColorBrush bgColor = (SolidColorBrush)(new BrushConverter().ConvertFrom("#080909"));
        SolidColorBrush selectBorderColor = (SolidColorBrush)(new BrushConverter().ConvertFrom("#3381bb"));
        SolidColorBrush selectBgColor = (SolidColorBrush)(new BrushConverter().ConvertFrom("#1e1e1e"));

        class DataItem
        {
            public Image image;
            public TextBlock name;
            public Border container;
            public Channel channel;
        }

        Dictionary<int, DataItem> dataItems = new Dictionary<int, DataItem>();
        RoomWindow parentWindow;
        StackPanel parentList;

        int selectedId = -1;
        ArrayList _IDs = new ArrayList();
        public ArrayList IDs
        {
            get
            {
                return this._IDs;
            }
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

        public void addItem(Channel itemObj)
        {
            if (dataItems.ContainsKey(itemObj.id))
            {

                DataItem existedItem = null;
                dataItems.TryGetValue(itemObj.id, out existedItem);
                if (existedItem != null)
                {
                    //Update object
                    Channel existedChannel = existedItem.channel;

                    //Update GUI
                    if (itemObj.name != existedChannel.name)
                    {
                        existedChannel.name = itemObj.name;
                        existedItem.name.Text = itemObj.name;
                    }

                    if (itemObj.image != existedChannel.image)
                    {
                        existedChannel.image = itemObj.image;

                        BitmapImage bitmap = new BitmapImage();
                        bitmap.BeginInit();
                        bitmap.UriSource = new Uri(itemObj.image, UriKind.RelativeOrAbsolute);
                        bitmap.CacheOption = BitmapCacheOption.OnLoad;
                        bitmap.EndInit();
                        existedItem.image.Source = bitmap;
                    }
                }

                return; // da ton tai thi next luon
            }

            DataItem dataItem = new DataItem();
            StackPanel itemPanel = new StackPanel();
            itemPanel.Children.Clear();
            itemPanel.Width = this.parentList.Width;
            itemPanel.Cursor = Cursors.Hand;
            
            if (itemObj.image != null && itemObj.image != "") {
                Image image = new Image();
                image.Margin = new Thickness(6, 6, 6, 6);
                image.Stretch = Stretch.Uniform;
                image.Width = 50;
                image.Source = SystemUtils.getImageUrl(itemObj.image);
                itemPanel.Children.Add(image);
                dataItem.image = image;
            }

            TextBlock name = new TextBlock();
            name.Foreground = Brushes.WhiteSmoke;
            name.HorizontalAlignment = HorizontalAlignment.Center;
            name.TextAlignment = TextAlignment.Center;
            name.TextWrapping = TextWrapping.WrapWithOverflow;
            name.Text = itemObj.name;
            itemPanel.Children.Add(name);
            dataItem.name = name;

            Border itemBorder = new Border();
            itemBorder.Margin = new Thickness(5, 2, 5, 2);
            itemBorder.Padding = new Thickness(5, 5, 5, 5);
            itemBorder.BorderThickness = new Thickness(1,1,1,1);
            itemBorder.BorderBrush = borderColor;
            itemBorder.Background = bgColor;
            itemBorder.Child = itemPanel;
            itemBorder.Tag = itemObj;
            itemBorder.MouseLeftButtonDown += itemPanel_MouseLeftButtonDown;
            itemBorder.MouseRightButtonDown += itemPanel_MouseLeftButtonDown;
            dataItem.container = itemBorder;
            dataItem.channel = itemObj;

            this.dataItems.Add(itemObj.id, dataItem);
            this.IDs.Add(itemObj.id);
            this.parentList.Children.Add(itemBorder);
        }

        void itemPanel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
                Channel channel = (sender as Border).Tag as Channel;

                if (channel != null)
                {
                    //update select item
                    DataItem existedItem = null;
                    dataItems.TryGetValue(selectedId, out existedItem);
                    if (existedItem != null)
                    {
                        existedItem.container.BorderBrush = borderColor;
                        existedItem.container.Background = bgColor;
                    }

                    selectedId = channel.id; 
                    existedItem = null;
                    dataItems.TryGetValue(selectedId, out existedItem);
                    if (existedItem != null)
                    {
                        existedItem.container.BorderBrush = selectBorderColor;
                        existedItem.container.Background = selectBgColor;
                    }

                    // load data
                    parentWindow.LoadRoomInChannel(channel);
                }
         }

    }
}
