using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Waetter_Mobile.Resources;


namespace Waetter_Mobile
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            this.InitializeComponent();
          // SettingsPane.GetForCurrentView().CommandsRequested += SettingsCommandsRequested;
            DataContext = new MainViewModel();
            ((MainViewModel)DataContext).LoadData();

            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();
        }

        // Sample code for building a localized ApplicationBar
        //private void BuildLocalizedApplicationBar()
        //{
        //    // Set the page's ApplicationBar to a new instance of ApplicationBar.
        //    ApplicationBar = new ApplicationBar();

        //    // Create a new button and set the text value to the localized string from AppResources.
        //    ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/appbar.add.rest.png", UriKind.Relative));
        //    appBarButton.Text = AppResources.AppBarButtonText;
        //    ApplicationBar.Buttons.Add(appBarButton);

        //    // Create a new menu item with the localized string from AppResources.
        //    ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem(AppResources.AppBarMenuItemText);
        //    ApplicationBar.MenuItems.Add(appBarMenuItem);
        //}

        private void RefreshButton_OnClick(object sender, RoutedEventArgs e)
        {
            ((MainViewModel)DataContext).LoadData();

        }

        private void PostleitzahlTextbox_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            var model = ((MainViewModel)DataContext);
            if (PostleitzahlTextbox.Text.Length == 4)
            {
                try
                {
                    int newPlz = Int32.Parse(PostleitzahlTextbox.Text);
                    model.Plz = newPlz;
                    model.LoadData();
                }
                catch (Exception)
                {
                    model.SetErrorDescription("Postleitzahl darf nur aus Zahlen bestehen.");
                }
            }
            else
            {
                model.SetErrorDescription("Postleitzahl muss aus vier Zahlen bestehen.");
            }

        }

       

        
    }
}