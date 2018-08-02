using System;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Diagnostics;

namespace GameShop.DataLayers
{
    class OrderItem : INotifyPropertyChanged
    {
        public int OrderItemId { get; set; }

        public int OrderId { get; set; }

        public int GameId { get; set; }

        public static void Insert(int orderItemId, int orderId, int gameId)
        {
            SqlConnection conn = App.Connect();
            conn.Open();
            SqlCommand cmd = conn.CreateCommand();

            cmd.CommandText = "INSERT INTO OrderItem " +
                        "(OrderItemId, OrderId, GameId) " +
                        "VALUES(@orderItemId, @orderIdFK, @gameIdFK)";

            Debug.WriteLine("OrderItemId now = " + orderItemId);
            cmd.Parameters.AddWithValue("@orderItemId", orderItemId);
            cmd.Parameters.AddWithValue("@orderIdFK", orderId);
            cmd.Parameters.AddWithValue("@gameIdFK", gameId);
            cmd.ExecuteNonQuery();

            conn.Close();
        }

        public static int Count()
        {
            SqlConnection conn = App.Connect();
            conn.Open();
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT COUNT(*) FROM OrderItem";
            Int32 count = (Int32)cmd.ExecuteScalar();
            conn.Close();

            return count;
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
