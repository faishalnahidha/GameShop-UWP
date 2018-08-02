using System;
using System.Linq;
using System.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace GameShop
{
    class MyDialog
    {
        public static async void Show(string message)
        {
            ContentDialog contentDialog = new ContentDialog
            {
                Title = message,
                Content = "Click OK to continue.",
                CloseButtonText = "OK",
                DefaultButton = ContentDialogButton.Close
            };

            ContentDialogResult result = await contentDialog.ShowAsync();
        }

        public static async void Show(string title, string content)
        {
            ContentDialog contentDialog = new ContentDialog
            {
                Title = title,
                Content = content,
                CloseButtonText = "OK",
                DefaultButton = ContentDialogButton.Close
            };

            ContentDialogResult result = await contentDialog.ShowAsync();
        }

        public static async void ShowTo(string title, string content)
        {
            ContentDialog contentDialog = new ContentDialog
            {
                Title = title,
                Content = content,
                PrimaryButtonText = "OK",
                DefaultButton = ContentDialogButton.Primary
            };

            ContentDialogResult result = await contentDialog.ShowAsync();


            if (result == ContentDialogResult.Primary)
            {
                Frame rootFrame = Window.Current.Content as Frame;
                rootFrame.Navigate(typeof(MainPage));
            }
        }
    }
}
