using GameShop.DataLayers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace GameShop.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class GameItemPage : Page
    {
        ObservableCollection<Game> games = new ObservableCollection<Game>();
        ObservableCollection<Publisher> publishers = new ObservableCollection<Publisher>();
        public GameItemPage()
        {
            this.InitializeComponent();
            this.games = Game.SelectAll();
            this.publishers = Publisher.SelectAll();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(GameIdTextBox.Text))
            {
                MyDialog.Show("Game ID is empty!",
                    "You need to fill Game ID text box");
            }
            else
            {
                Game newGame = new Game();
                newGame.GameId = Convert.ToInt32(GameIdTextBox.Text);
                newGame.Title = TitleTextBox.Text;
                newGame.Genre = GenreTextBox.Text;
                newGame.Publisher = ((Publisher)PublisherComboBox.SelectedItem).Name;
                newGame.PublisherId = ((Publisher)PublisherComboBox.SelectedItem).PublisherId;
                newGame.Platform = PlatformComboBox.SelectedItem.ToString();
                newGame.Price = Convert.ToInt32(PriceTextBox.Text);
                newGame.Quantity = Convert.ToInt32(QuantityTextBox.Text);

                Game.Insert(newGame);

                MyDialog.Show(newGame.Title + " is successfully added");
                GameIdTextBox.Text = "";
                TitleTextBox.Text = "";
                GenreTextBox.Text = "";
                PublisherComboBox.SelectedIndex = -1;
                PlatformComboBox.SelectedIndex = -1;
                PriceTextBox.Text = "";
                QuantityTextBox.Text = "";

                this.games = Game.SelectAll();
                DataGrid.ItemsSource = games;
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(GameIdTextBox.Text))
            {
                MyDialog.Show("Game ID is empty!",
                    "You need to fill Game ID text box");
            }
            else
            {
                Game editedGame = new Game();
                editedGame.GameId = Convert.ToInt32(GameIdTextBox.Text);
                editedGame.Title = TitleTextBox.Text;
                editedGame.Genre = GenreTextBox.Text;
                editedGame.Publisher = ((Publisher)PublisherComboBox.SelectedItem).Name;
                editedGame.PublisherId = ((Publisher)PublisherComboBox.SelectedItem).PublisherId;
                editedGame.Platform = PlatformComboBox.SelectedItem.ToString();
                editedGame.Price = Convert.ToInt32(PriceTextBox.Text);
                editedGame.Quantity = Convert.ToInt32(QuantityTextBox.Text);

                Game.UpdateAllProperties(editedGame);

                MyDialog.Show(editedGame.Title + " has been edited");
                GameIdTextBox.Text = "";
                TitleTextBox.Text = "";
                GenreTextBox.Text = "";
                PublisherComboBox.SelectedIndex = -1;
                PlatformComboBox.SelectedIndex = -1;
                PriceTextBox.Text = "";
                QuantityTextBox.Text = "";

                this.games = Game.SelectAll();
                DataGrid.ItemsSource = games;
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(GameIdTextBox.Text))
            {
                MyDialog.Show("Game ID is empty!",
                    "You need to fill Game ID text box");
            }
            else
            {
                int gameId = Convert.ToInt32(GameIdTextBox.Text);
                Game.Delete(gameId);
                MyDialog.Show("Publisher ID = " + gameId + " has been deleted");
                this.games = Game.SelectAll();
                DataGrid.ItemsSource = games;
            }
        }

        private void DataGrid_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataGrid.SelectedItem != null)
            {
                Game selectedGame = (Game)DataGrid.SelectedItem;

                GameIdTextBox.Text = selectedGame.GameId.ToString();
                TitleTextBox.Text = selectedGame.Title;
                GenreTextBox.Text = selectedGame.Genre;
                PublisherComboBox.SelectedItem = publisherComboBoxSelected(selectedGame.Publisher);
                PlatformComboBox.SelectedItem = selectedGame.Platform;
                PriceTextBox.Text = selectedGame.Price.ToString();
                QuantityTextBox.Text = selectedGame.Quantity.ToString();
            }
            else
            {
                GameIdTextBox.Text = "";
                TitleTextBox.Text = "";
                GenreTextBox.Text = "";
                PublisherComboBox.SelectedIndex = -1;
                PlatformComboBox.SelectedIndex = -1;
                PriceTextBox.Text = "";
                QuantityTextBox.Text = "";
            }
        }

        private Publisher publisherComboBoxSelected(string name)
        {
            Publisher pub = new Publisher();
            foreach (var publisher in this.publishers)
            {
                if (name == publisher.Name)
                {
                    pub = publisher;
                    break;
                }
            }
            return pub;
        }
    }
}
