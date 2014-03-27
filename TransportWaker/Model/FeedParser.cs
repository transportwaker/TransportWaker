using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Xml;

namespace TransportWaker.Model
{
    public class FeedParser
    {
        private string agency;

        public FeedParser(string agency)
        {
            this.agency = agency;
        }

        public List<Dictionary<string, string>> getRouteList()
        {
            WebClient client = new WebClient();
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(client.DownloadString("http://webservices.nextbus.com/service/publicXMLFeed?command=routeList&a=" + agency));
            XmlNode parent = xml.FirstChild;
            XmlNodeList routes = xml.GetElementsByTagName("route");
            List<Dictionary<string, string>> response = new List<Dictionary<string, string>>();

            if (routes.Count == 0)
            {
                Dictionary<string, string> error = new Dictionary<string, string>();
                error.Add("error", "No routes found");
                response.Add(error);
                return response;
            }

            for (int i = 0; i < routes.Count; i++)
            {
                Dictionary<string, string> route = new Dictionary<string, string>();
                route.Add("tag", routes[i].Attributes["tag"].Value);
                route.Add("name", routes[i].Attributes["title"].Value);
                response.Add(route);
            }
            return response;
        }

        public List<Dictionary<string, string>> getRouteConfig(string route)
        {
            WebClient client = new WebClient();
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(client.DownloadString("http://webservices.nextbus.com/service/publicXMLFeed?command=routeConfig&a=" + agency + "&r=" + route));
            XmlNode parent = xml.FirstChild;
            XmlNodeList stops = xml.GetElementsByTagName("stop");
            List<Dictionary<string, string>> response = new List<Dictionary<string, string>>();

            if (stops.Count == 0)
            {
                Dictionary<string, string> error = new Dictionary<string, string>();
                error.Add("error", "Could not retrieve that route");
                response.Add(error);
                return response;
            }

            for (int i = 0; i < stops.Count; i++)
            {
                if (stops[i].ParentNode.Name == "route")
                {
                    Dictionary<string, string> stop = new Dictionary<string, string>();
                    stop.Add("tag", stops[i].Attributes["tag"].Value);
                    stop.Add("name", stops[i].Attributes["title"].Value);
                    stop.Add("lat", stops[i].Attributes["lat"].Value);
                    stop.Add("lon", stops[i].Attributes["lon"].Value);
                    response.Add(stop);
                }
            }
            return response;
        }

        public List<Dictionary<string, string>> getDirectionNames(string route)
        {
            WebClient client = new WebClient();
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(client.DownloadString("http://webservices.nextbus.com/service/publicXMLFeed?command=routeConfig&a=" + agency + "&r=" + route));
            XmlNode parent = xml.FirstChild;
            XmlNodeList directions = xml.GetElementsByTagName("direction");
            List<Dictionary<string, string>> response = new List<Dictionary<string, string>>();

            if (directions.Count == 0)
            {
                Dictionary<string, string> error = new Dictionary<string, string>();
                error.Add("error", "Could not retrieve directions");
                response.Add(error);
                return response;
            }

            for (int i = 0; i < directions.Count; i++)
            {
                Dictionary<string, string> direction = new Dictionary<string, string>();
                direction.Add("tag", directions[i].Attributes["tag"].Value);
                direction.Add("name", directions[i].Attributes["title"].Value);
                response.Add(direction);
            }
            return response;
        }

        public List<Dictionary<string, string>> getPredictions(string route, string stop, string direction)
        {
            WebClient client = new WebClient();
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(client.DownloadString("http://webservices.nextbus.com/service/publicXMLFeed?command=predictions&a=" + agency + "&r=" + route + "&s=" + stop));
            XmlNode parent = xml.FirstChild;
            XmlNodeList predictions = xml.GetElementsByTagName("direction");
            List<Dictionary<string, string>> response = new List<Dictionary<string, string>>();

            if (predictions.Count == 0)
            {
                Dictionary<string, string> error = new Dictionary<string, string>();
                error.Add("error", "Could not retrieve any predictions");
                response.Add(error);
                return response;
            }

            for (int i = 0; i < predictions.Count; i++)
            {
                if (predictions[i].ParentNode.Name == direction)
                {
                    Dictionary<string, string> prediction = new Dictionary<string, string>();
                    prediction.Add("vehicle", predictions[i].Attributes["vehicle"].Value);
                    prediction.Add("time", predictions[i].Attributes["epochTime"].Value);
                    prediction.Add("seconds", predictions[i].Attributes["seconds"].Value);
                    prediction.Add("minutes", predictions[i].Attributes["minutes"].Value);
                    prediction.Add("trip", predictions[i].Attributes["tripTag"].Value);
                    response.Add(prediction);
                }
            }
            return response;
        }
    }


}