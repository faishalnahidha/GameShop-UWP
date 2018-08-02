using GameShop.DataLayers;
using System.Collections.ObjectModel;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace GameShop.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class OrderPage : Page
    {
        ObservableCollection<Order> orders = new ObservableCollection<Order>(); 
        public OrderPage()
        {
            this.InitializeComponent();
            this.orders = Order.SelectAll();
        }
    }
}
