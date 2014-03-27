using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneApp
{
    class Location
    {
        public double Latitude;
        public double Longitude;

        public Location(double Latitude, double Longitude)
        {
            this.Latitude = Latitude;
            this.Longitude = Longitude;
        }

        public double GetDistance(Location to)
        {
            double dLat = (this.Latitude - to.Latitude) * Math.PI / 180;
            double dLon = (this.Longitude - to.Longitude) * Math.PI / 180;
            double lat1 = this.Latitude * Math.PI / 180;
            double lat2 = to.Latitude * Math.PI / 180;
            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) + Math.Sin(dLon / 2)
            * Math.Sin(dLon / 2) * Math.Cos(lat1) * Math.Cos(lat2);
            return 6371 * 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
        }
    }
}
