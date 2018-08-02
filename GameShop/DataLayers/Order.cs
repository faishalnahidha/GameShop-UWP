using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameShop.DataLayers
{
    class Order : INotifyPropertyChanged
    {
        public int OrderId { get; set; }

        public int TotalItem { get; set; }

        public int TotalPrice { get; set; }

        public int Cash { get; set; }

        public int Change { get; set; }

        public DateTime OrderAt { get; set; }

        // not included to database
        private ObservableCollection<Game> _orderItems;
        public ObservableCollection<Game> OrderItems {
            get { return _orderItems; }
            set
            {
                if (_orderItems == value) return;
                _orderItems = value;
                if(_orderItems != null )
                {
                    _orderItems.CollectionChanged += delegate
                    {
                        CountTotalPrice();
                        CountTotalItem();
                    };                  
                }
                NotifyPropertyChanged("OrderItems");
            }
        } 

        public Order()
        {
        }

        public Order(int id)
        {
            OrderId = id;
            TotalPrice = 0;
            Cash = 0;
            Change = 0;
            OrderItems = new ObservableCollection<Game>();
        }

        private void CountTotalPrice()
        {
            int totalPrice = 0;
            foreach (var orderItem in OrderItems)
            {
                totalPrice += orderItem.Price;
            }
            TotalPrice = totalPrice;
            NotifyPropertyChanged("TotalPrice");        
        }

        private void CountTotalItem()
        {
            TotalItem = OrderItems.Count;
            NotifyPropertyChanged("TotalItem");
        }

        public void CountChange()
        {
            Change = Cash - TotalPrice;
            NotifyPropertyChanged("Change");
        }

        public static void Insert(Order newOrder)
        {
            SqlConnection conn = App.Connect();
            conn.Open();
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "INSERT INTO Order1 " +
                "(OrderId, TotalItem, TotalPrice, Cash, Change, OrderAt)" +
                "VALUES(@orderId, @totalItem, @totalPrice, @cash, @change, @orderAt)";

            cmd.Parameters.AddWithValue("@orderId", newOrder.OrderId);
            cmd.Parameters.AddWithValue("@totalItem", newOrder.TotalItem);
            cmd.Parameters.AddWithValue("@totalPrice", newOrder.TotalPrice);
            cmd.Parameters.AddWithValue("@cash", newOrder.Cash);
            cmd.Parameters.AddWithValue("@change", newOrder.Change);
            cmd.Parameters.AddWithValue("@orderAt", newOrder.OrderAt);
            cmd.ExecuteNonQuery();

            conn.Close();
        }

        public static int Count()
        {
            SqlConnection conn = App.Connect();
            conn.Open();
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT COUNT(*) FROM Order1";
            Int32 count = (Int32)cmd.ExecuteScalar();
            conn.Close();

            return count;
        }

        public static ObservableCollection<Order> SelectAll()
        {
            ObservableCollection<Order> orders = new ObservableCollection<Order>();

            SqlConnection conn = App.Connect();
            string Query = "SELECT * FROM Order1";
            SqlCommand cmd = new SqlCommand(Query, conn);
            SqlDataReader rdr;
            conn.Open();
            rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                Order order = new Order();
                order.OrderId = Convert.ToInt32(rdr["OrderId"].ToString());
                order.TotalItem = Convert.ToInt32(rdr["TotalItem"].ToString());
                order.TotalPrice = Convert.ToInt32(rdr["TotalPrice"].ToString());
                order.Cash = Convert.ToInt32(rdr["Cash"].ToString());
                order.Change = Convert.ToInt32(rdr["Change"].ToString());
                order.OrderAt = Convert.ToDateTime(rdr["OrderAt"].ToString());

                orders.Add(order);
            }
            conn.Close();

            return orders;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
