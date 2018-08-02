using GameShop.DataLayers;
using System;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using System.Diagnostics;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace GameShop.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class NewOrderPage : Page
    {
        ObservableCollection<Game> games = new ObservableCollection<Game>();
        /// ObservableCollection<Game> gamesPurchased = new ObservableCollection<Game>();
        Order newOrder;
        int initialOrderItemId = 0;

        public NewOrderPage()
        {
            this.InitializeComponent();
            this.games = Game.SelectSomeProps();
            this.newOrder = new Order(this.generateOrderId());
            this.initialOrderItemId = this.generateOrderItemId();

            OrderIdTextBox.Text = newOrder.OrderId.ToString();
            TotalItemTextBox.Text = newOrder.TotalItem.ToString();
            TotalPriceTextBox.Text = newOrder.TotalPrice.ToString();
        }

        private void AddGameItemButton_Click(object sender, RoutedEventArgs e)
        {
            if (GameItemComboBox.SelectedIndex != -1)
            {
                newOrder.OrderItems.Add((Game)GameItemComboBox.SelectedItem);
                TotalItemTextBox.Text = newOrder.TotalItem.ToString();
                TotalPriceTextBox.Text = newOrder.TotalPrice.ToString();
            }
        }

        private void OrderButton_Click(object sender, RoutedEventArgs e)
        {
            if(newOrder.OrderItems.Count < 1)
            {
                MyDialog.Show("Order Item is empty!",
                    "Please add at least one game item.");
            }
            else if (string.IsNullOrWhiteSpace(CashTextBox.Text))
            {
                MyDialog.Show("Cash is empty!",
                    "Please input the cash.");
            }
            else
            {
                newOrder.Cash = Convert.ToInt32(CashTextBox.Text);
                newOrder.OrderAt = DateTime.Now;

                if (newOrder.Cash < newOrder.TotalPrice)
                {
                    MyDialog.Show("Insufficient cash");
                    return;
                }

                newOrder.CountChange();

                Order.Insert(newOrder);

                int nextOrderItemId = this.initialOrderItemId;

                foreach (var orderItem in newOrder.OrderItems)
                {
                    OrderItem.Insert(nextOrderItemId, newOrder.OrderId, orderItem.GameId);
                    nextOrderItemId++;
                }

                MyDialog.ShowTo("Order is sucessfully added",
                    "Cash : Rp " + newOrder.Cash +
                    "\nTotal : Rp " + newOrder.TotalPrice +
                    "\nChange : Rp " + newOrder.Change);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;
            rootFrame.Navigate(typeof(MainPage));
        }

        public int generateOrderId()
        {
            int count = Order.Count();
            return count + 1;
        }

        public int generateOrderItemId()
        {
            int count = OrderItem.Count();
            return count;
        }

    }
}
