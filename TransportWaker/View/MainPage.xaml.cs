using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using TransportWaker.Resources;
using TransportWaker.ViewModel;
using TransportWaker.View;

namespace TransportWaker.View
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();

            // Set the data context of the LongListSelector control to the sample data
            DataContext = App.ViewModel;

            BuildLocalizedApplicationBar();
        }

        // Load data for the ViewModel Items
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (!App.ViewModel.IsDataLoaded)
            {
                App.ViewModel.LoadData();
            }
        }

        // Handle selection changed on LongListSelector
        //private void MainLongListSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
       // {
            // If selected item is null (no selection) do nothing
          //  if (MainLongListSelector.SelectedItem == null)
         //       return;
        //
            // Navigate to the new page
        //    NavigationService.Navigate(new Uri("/DetailsPage.xaml?selectedItem=" + (MainLongListSelector.SelectedItem as ItemViewModel).ID, UriKind.Relative));

            // Reset selected item to null (no selection)
         //   MainLongListSelector.SelectedItem = null;
       // }


        private void BuildLocalizedApplicationBar()
        {
            ApplicationBar = new ApplicationBar();

            ApplicationBarMenuItem aboutAppBar =
                new ApplicationBarMenuItem();
            aboutAppBar.Text = AppResources.AppBarAbout;

            aboutAppBar.Click += aboutAppBar_Click;

            ApplicationBar.MenuItems.Add(aboutAppBar);
        }

        private void aboutAppBar_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/View/AboutPage.xaml", UriKind.Relative));
        }

        private void Tap_SavedRoute(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/View/RouteHistory.xaml", UriKind.Relative));
        }

        private void Tap_FindRouteStart(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/View/FindRouteStart.xaml", UriKind.Relative));
        }
    }
}