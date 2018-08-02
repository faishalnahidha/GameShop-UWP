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
    public class Publisher: INotifyPropertyChanged
    {
        public int PublisherId { get; set; }
        public string Name { get; set; }

        public string Country { get; set; }

        public Publisher()
        {
        }

        public Publisher(int id, string name, string country)
        {
            PublisherId = id;
            Name = name;
            Country = country;
        }

        public static void Insert(Publisher publisher)
        {
            SqlConnection conn = App.Connect();
            conn.Open();
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "INSERT INTO Publisher " +
                "(PublisherId, Name, Country) " +
                "VALUES(@publisherId, @name, @country)";

            cmd.Parameters.AddWithValue("@publisherId", publisher.PublisherId);
            cmd.Parameters.AddWithValue("@name", publisher.Name);
            cmd.Parameters.AddWithValue("@country", publisher.Country);
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        public static void UpdateAllProperty(Publisher publisher)
        {
            SqlConnection conn = App.Connect();
            conn.Open();
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "UPDATE Publisher SET " +
                "Name = @name, " +
                "Country = @country, " +
                "WHERE PublisherId = @publisherId";

            cmd.Parameters.AddWithValue("@publisherId", publisher.PublisherId);
            cmd.Parameters.AddWithValue("@name", publisher.Name);
            cmd.Parameters.AddWithValue("@country", publisher.Country);
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        public static void Delete(int publisherId)
        {
            SqlConnection conn = App.Connect();
            conn.Open();
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "DELETE FROM Publisher WHERE PublisherId = @publisherId";
            cmd.Parameters.AddWithValue("@publisherId", publisherId);
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        public static ObservableCollection<Publisher> SelectAll()
        {
            ObservableCollection<Publisher> publishers = new ObservableCollection<Publisher>();

            SqlConnection conn = App.Connect();
            string Query = "SELECT * from Publisher";
            SqlCommand cmd = new SqlCommand(Query, conn);
            SqlDataReader rdr;
            conn.Open();
            rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                publishers.Add(new Publisher(
                    Convert.ToInt32(rdr["PublisherId"].ToString()),
                    rdr["Name"].ToString(),
                    rdr["Country"].ToString()));
            }
            conn.Close();

            return publishers;
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