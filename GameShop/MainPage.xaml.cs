using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace GameShop
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            string appName = Windows.ApplicationModel.Package.Current.DisplayName;
            // AppTitleBar.Text = "";

            //  SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;


            var titleBarView = ApplicationView.GetForCurrentView().TitleBar;
            titleBarView.ButtonForegroundColor = Colors.Black;
        }

        private void NavView_Loaded(object sender, RoutedEventArgs e)
        {
            // you can also add items in code behind
            /*
            NavView.MenuItems.Add(new NavigationViewItemSeparator());
            NavView.MenuItems.Add(new NavigationViewItem()
            { Content = "My content", Icon = new SymbolIcon(Symbol.Folder), Tag = "content" });
            */
            // set the initial SelectedItem 
            foreach (NavigationViewItemBase item in NavView.MenuItems)
            {
                if (item is NavigationViewItem && item.Tag.ToString() == "order")
                {
                    NavView.SelectedItem = item;
                    break;
                }
            }         
        }

        private void NavView_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            if (args.IsSettingsInvoked)
            {
                //ContentFrame.Navigate(typeof(SettingsPage));
            }
            else
            {
                switch (args.InvokedItem)
                {
                    case "New Order":
                        ContentFrame.Navigate(typeof(Views.NewOrderPage));
                        break;

                    case "Orders":
                        ContentFrame.Navigate(typeof(Views.OrderPage));
                        break;

                    case "Game Items":
                        ContentFrame.Navigate(typeof(Views.GameItemPage));
                        break;

                    case "Publishers":
                        ContentFrame.Navigate(typeof(Views.PublisherPage));
                        break;
                }
            }
        }

        private void NavView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            if (args.IsSettingsSelected)
            {
                //ContentFrame.Navigate(typeof(SettingsPage));
            }
            else
            {

                NavigationViewItem item = args.SelectedItem as NavigationViewItem;

                switch (item.Tag)
                {
                    case "new-order":
                        ContentFrame.Navigate(typeof(Views.NewOrderPage));
                        NavView.Header = "  New Order";
                        break;

                    case "order":
                        ContentFrame.Navigate(typeof(Views.OrderPage));
                        NavView.Header = "  Orders";
                        break;

                    case "game-item":
                        ContentFrame.Navigate(typeof(Views.GameItemPage));
                        NavView.Header = "  Game Items";
                        break;

                    case "publisher":
                        ContentFrame.Navigate(typeof(Views.PublisherPage));
                        NavView.Header = "  Publishers";
                        break;
                }
            }
        }

    }
}
