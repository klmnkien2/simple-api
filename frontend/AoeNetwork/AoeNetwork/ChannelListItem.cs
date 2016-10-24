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

        class DataItem
        {
            public Image image;
            public Label name;
            public Border container;
            public Channel channel;
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
                        existedItem.name.Content = itemObj.name;
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
            
            if (itemObj.image != null && itemObj.image != "") {
                Image image = new Image();
                image.Margin = new Thickness(6, 6, 6, 6);
                image.Source = SystemUtils.getImageUrl(itemObj.image);
                itemPanel.Children.Add(image);
                dataItem.image = image;
            }

            Label name = new Label();
            name.Foreground = Brushes.White;
            name.VerticalContentAlignment = VerticalAlignment.Bottom;
            name.FontWeight = FontWeights.Bold;
            name.Content = itemObj.name;
            itemPanel.Children.Add(name);
            dataItem.name = name;

            Border itemBorder = new Border();
            itemBorder.BorderThickness = new Thickness(1,1,1,0);
            itemBorder.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#0e1e28"));
            itemBorder.BorderBrush = (SolidColorBrush)(new BrushConverter().ConvertFrom("#010202"));
            itemBorder.Child = itemPanel;
            itemBorder.Tag = itemObj;
            itemBorder.MouseLeftButtonDown += itemPanel_MouseLeftButtonDown;
            dataItem.container = itemBorder;

            this.dataItems.Add(itemObj.id, dataItem);
            this.IDs.Add(itemObj.id);
            this.parentList.Children.Add(itemBorder);
        }

        void itemPanel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                Friend user = (sender as Border).Tag as Friend;
                if (user != null)
                {
                    //parentWindow.OpenPrivate(user, true);
                }
            }
        }

    }
}
