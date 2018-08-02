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
    class Game : INotifyPropertyChanged
    {
        public int GameId { get; set; }

        public string Title { get; set; }

        public string Genre { get; set; }

        public string Publisher { get; set; } // bukan kolom di tabel Publisher

        public int PublisherId { get; set; }

        public string Platform { get; set; }

        public int Price { get; set; }

        public int Quantity { get; set; }

        // bukan kolom di tabel Publisher
        public string ToString2 {
            get { return GameId + ". " + Title + " (" + Platform + ")"; }
        } 

        public Game()
        {
        }

        public Game(int id, string title)
        {
            GameId = id;
            Title = title;
        }

        public Game(int id, string title, string platform, int price)
        {
            GameId = id;
            Title = title;
            Platform = platform;
            Price = price;
        }

        public static void Insert(Game game)
        {
            SqlConnection conn = App.Connect();
            conn.Open();
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "INSERT INTO Game " +
                "(GameId, Title, Genre, PublisherId, Platform, Price, Quantity) " +
                "VALUES(@gameId, @title, @genre, @publisherId, @platform, @price, @quantity)";

            cmd.Parameters.AddWithValue("@gameId", game.GameId);
            cmd.Parameters.AddWithValue("@title", game.Title);
            cmd.Parameters.AddWithValue("@genre", game.Genre);
            cmd.Parameters.AddWithValue("@publisherId", game.PublisherId);
            cmd.Parameters.AddWithValue("@platform", game.Platform);
            cmd.Parameters.AddWithValue("@price", game.Price);
            cmd.Parameters.AddWithValue("@quantity", game.Quantity);
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        public static void UpdateAllProperties(Game game)
        {
            SqlConnection conn = App.Connect();
            conn.Open();
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "UPDATE Game SET " +
                "Title = @title, " +
                "Genre = @genre, " +
                "PublisherId = @publisherId, " +
                "Platform = @platform, " +
                "Price = @price, " +
                "Quantity = @quantity " + 
                "WHERE GameId = @gameId";

            cmd.Parameters.AddWithValue("@gameId", game.GameId);
            cmd.Parameters.AddWithValue("@title", game.Title);
            cmd.Parameters.AddWithValue("@genre", game.Genre);
            cmd.Parameters.AddWithValue("@publisherId", game.PublisherId);
            cmd.Parameters.AddWithValue("@platform", game.Platform);
            cmd.Parameters.AddWithValue("@price", game.Price);
            cmd.Parameters.AddWithValue("@quantity", game.Quantity);
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        public static void Delete(int gameId)
        {
            SqlConnection conn = App.Connect();
            conn.Open();
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "DELETE FROM Game WHERE GameId = @gameId";
            cmd.Parameters.AddWithValue("@gameId", gameId);
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        public static ObservableCollection<Game> SelectAll()
        {
            ObservableCollection<Game> games = new ObservableCollection<Game>();

            SqlConnection conn = App.Connect();
            string Query = "SELECT Game.GameId, Game.Title, Game.Genre, Publisher.Name, " +
                "Game.Platform, Game.Price, Game.Quantity " +
                "FROM Game INNER JOIN Publisher ON Game.PublisherId=Publisher.PublisherId";
            SqlCommand cmd = new SqlCommand(Query, conn);
            SqlDataReader rdr;
            conn.Open();
            rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                Game game = new Game();
                game.GameId = Convert.ToInt32(rdr["GameId"].ToString());
                game.Title = rdr["Title"].ToString();
                game.Genre = rdr["Genre"].ToString();
                game.Publisher = rdr["Name"].ToString();
                game.Platform = rdr["Platform"].ToString();
                game.Price = Convert.ToInt32(rdr["Price"].ToString());
                game.Quantity = Convert.ToInt32(rdr["Quantity"].ToString());

                games.Add(game);
            }
            conn.Close();

            return games;
        }

        public static ObservableCollection<Game> SelectSomeProps()
        {
            ObservableCollection<Game> games = new ObservableCollection<Game>();

            SqlConnection conn = App.Connect();
            string Query = "SELECT GameId, Title, Platform, Price FROM Game";
            SqlCommand cmd = new SqlCommand(Query, conn);
            SqlDataReader rdr;
            conn.Open();
            rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                Game game = new Game();
                game.GameId = Convert.ToInt32(rdr["GameId"].ToString());
                game.Title = rdr["Title"].ToString();
                game.Platform = rdr["Platform"].ToString();
                game.Price = Convert.ToInt32(rdr["Price"].ToString());

                games.Add(game);
            }
            conn.Close();

            return games;
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
