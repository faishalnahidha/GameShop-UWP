using GameShop.DataLayers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Diagnostics;
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
    public sealed partial class PublisherPage : Page
    {
        ObservableCollection<Publisher> publishers = new ObservableCollection<Publisher>();

        public PublisherPage()
        {
            this.InitializeComponent();
            publishers = Publisher.SelectAll();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(PublisherIdTextBox.Text))
            {
                MyDialog.Show("Publisher ID is empty!",
                    "You need to fill Publisher ID text box");
            }
            else
            {
                int id = Convert.ToInt32(PublisherIdTextBox.Text);
                string name = NameTextBox.Text;
                string country = CountryTextBox.Text;

                Publisher newPublisher = new Publisher(id, name, country);

                Publisher.Insert(newPublisher);

                MyDialog.Show(newPublisher.Name + " is successfully added");
                PublisherIdTextBox.Text = "";
                NameTextBox.Text = "";
                CountryTextBox.Text = "";


                this.publishers = Publisher.SelectAll();
                DataGrid.ItemsSource = publishers;
            }
        }
       
        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(PublisherIdTextBox.Text))
            {
                MyDialog.Show("Publisher ID is empty!",
                    "You need to fill Publisher ID text box");
            }
            else
            {
                int id = Convert.ToInt32(PublisherIdTextBox.Text);
                string name = NameTextBox.Text;
                string country = CountryTextBox.Text;

                Publisher editedPublisher = new Publisher(id, name, country);

                Publisher.UpdateAllProperty(editedPublisher);

                MyDialog.Show(editedPublisher.Name + " has been edited");
                PublisherIdTextBox.Text = "";
                NameTextBox.Text = "";
                CountryTextBox.Text = "";

                this.publishers = Publisher.SelectAll();
                DataGrid.ItemsSource = publishers;
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(PublisherIdTextBox.Text))
            {
                MyDialog.Show("Publisher ID is empty!", 
                    "You need to fill Publisher ID text box");
            }
            else
            {
                int publisherId = Convert.ToInt32(PublisherIdTextBox.Text);
                Publisher.Delete(publisherId);

                MyDialog.Show("Publisher ID = " + publisherId + " has been deleted");

                this.publishers = Publisher.SelectAll();
                DataGrid.ItemsSource = publishers;
            }
        }

        private void DataGrid_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(DataGrid.SelectedItem != null)
            {
                Publisher selectedPublisher = (Publisher)DataGrid.SelectedItem;

                Debug.WriteLine(selectedPublisher.PublisherId + ". " + selectedPublisher.Name);

                PublisherIdTextBox.Text = selectedPublisher.PublisherId.ToString();
                NameTextBox.Text = selectedPublisher.Name;
                CountryTextBox.Text = selectedPublisher.Country;
            }
            else
            {
                PublisherIdTextBox.Text = "";
                NameTextBox.Text = "";
                CountryTextBox.Text = "";
            }           
        }
    }
}
