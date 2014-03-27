using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Windows.Devices.Geolocation;
using System.Device.Location;
using Microsoft.Phone.Controls.Maps.Platform;
using Microsoft.Phone.Maps.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Microsoft.Phone.Maps.Toolkit;

namespace TransportWaker.View
{
    public partial class FindRouteStart : PhoneApplicationPage
    {
        public FindRouteStart()
        {
            InitializeComponent();
            Loaded += MainPage_Loaded;
        }

        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            SystemTray.ProgressIndicator = new ProgressIndicator();

            GetUserLoc();

        }

        private async void GetUserLoc()
        {
            Geolocator geolocator = new Geolocator();
            geolocator.DesiredAccuracyInMeters = 10;

            SetProgressIndicator(true);
            SystemTray.ProgressIndicator.Text = "Getting GPS Location";

            try
            {
                Geoposition position =
                    await geolocator.GetGeopositionAsync(
                    TimeSpan.FromMinutes(1),
                    TimeSpan.FromSeconds(10));

                SystemTray.ProgressIndicator.Text = "Acquired";

                var gpsCoorCenter =
                    new GeoCoordinate(
                        position.Coordinate.Latitude,
                        position.Coordinate.Longitude);

                StartMap.Center = gpsCoorCenter;
                StartMap.ZoomLevel = 20;

                Pushpin userLoc = new Pushpin();

                userLoc.GeoCoordinate = new GeoCoordinate(
                                position.Coordinate.Latitude,
                                position.Coordinate.Longitude);
                MapLayer layer0 = new MapLayer();
                MapOverlay overlay0 = new MapOverlay();
                overlay0.Content = userLoc;
                overlay0.GeoCoordinate = new GeoCoordinate(position.Coordinate.Latitude,
                                                             position.Coordinate.Longitude);
                layer0.Add(overlay0);
                StartMap.Layers.Add(layer0);

                SetProgressIndicator(false);
            }
            catch (UnauthorizedAccessException)
            {
                MessageBox.Show("Location is disable in phone settings.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private static void SetProgressIndicator(bool isVisible)
        {
            SystemTray.ProgressIndicator.IsIndeterminate = isVisible;
            SystemTray.ProgressIndicator.IsVisible = isVisible;
        }

    }
}