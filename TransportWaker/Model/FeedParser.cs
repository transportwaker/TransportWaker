using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Diagnostics;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TransportWaker.ViewModel;
using System.IO.IsolatedStorage;
using System.IO;

namespace TransportWaker.Model
{
    public class FeedParser
    {
        private string agency;
        public const string routeDataFile = "RouteList.txt";
        public string dataRoutes;

        public FeedParser(string agency)
        {
            this.agency = agency;
        }

        public static async Task<string> DownloadString(string url)
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(new Uri(url));
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> GetRouteList()
        {
            string data = await DownloadString("http://webservices.nextbus.com/service/publicXMLFeed?command=routeList&a=" + agency);
            XDocument root = XDocument.Parse(data);
            List<XElement> routes = new List<XElement>();
            routes.AddRange(root.Descendants("route"));

            if (routes.Count == 0)
            {
                return "[{\"error\": \"No routes found\"}]";
            }

            string json = "[{\"tag\": \"" + routes[0].Attribute("tag").Value + "\", \"name\": \"" + routes[0].Attribute("title").Value + "\"}";
            for (int i = 1; i < routes.Count; i++)
            {
                json += ", {\"tag\": \"" + routes[i].Attribute("tag").Value + "\", \"name\": \"" + routes[i].Attribute("title").Value + "\"}";
            }
            json += "]";

            Debug.WriteLine(json);
            return json;
        }

        public async Task<string> GetRouteConfig(string route)
        {
            string data = await DownloadString("http://webservices.nextbus.com/service/publicXMLFeed?command=routeConfig&a=" + agency + "&r=" + route);
            XDocument root = XDocument.Parse(data);
            List<XElement> stops = new List<XElement>();
            stops.AddRange(root.Descendants("stop"));
            stops.RemoveAll(stop => stop.Parent.Name != "route");

            if (stops.Count == 0)
            {
                return "[{\"error\": \"No stops found for that route\"}]";
            }

            string json = "[{\"tag\": \"" + stops[0].Attribute("tag").Value + "\", \"name\": \"" + stops[0].Attribute("title").Value + "\", \"lat\": \"" + stops[0].Attribute("lat").Value + "\", \"lon\": \"" + stops[0].Attribute("lon").Value + "\"}";
            for (int i = 1; i < stops.Count; i++)
            {
                json += ", {\"tag\": \"" + stops[i].Attribute("tag").Value + "\", \"name\": \"" + stops[i].Attribute("title").Value + "\", \"lat\": \"" + stops[i].Attribute("lat").Value + "\", \"lon\": \"" + stops[i].Attribute("lon").Value + "\"}";
            }
            json += "]";

            Debug.WriteLine(json);
            return json;
        }






        //To work on later
        //public static async Task SaveRoutes()
        //{
        //    using (var storageFolder = IsolatedStorageFile.GetUserStoreForApplication())
        //    {
        //        using (var stream = storageFolder.CreateFile(routeDataFile))
        //        {
        //            using (StreamWriter writer = new StreamWriter(stream))
        //            {
        //                FeedParser agent = new FeedParser("ttc");
        //                string data = await agent.GetRouteList();

        //                writer.Write(data);
        //            }
        //        }
        //    }
        //}

        //public async static void GetSavedRoutes()
        //{


        //    using (IsolatedStorageFile storageFolder = IsolatedStorageFile.GetUserStoreForApplication())
        //    {
        //        if (!storageFolder.FileExists(routeDataFile))
        //            SaveRoutes();

        //        using (IsolatedStorageFileStream stream = storageFolder.OpenFile(routeDataFile, FileMode.Open))
        //        {
        //            using (StreamReader reader = new StreamReader(stream))
        //            {
        //                string routesData = reader.ReadToEnd();
        //            }
        //        }
        //    }

        //    routesData = dataRoutes;
        
        //}


        //        if (routesData != null)
        //    {

        //        LocViewModel data = new LocViewModel();

        //        for (int i = 0; i < routesData.Count; i++)
        //        {
        //            JToken route = routesData[i];
        //            data.Items.Add(new LocTags
        //            {
        //                Tag = route["tag"].ToString(),
        //                Title = route["title"].ToString()
        //            });

        //        }

        //public LocViewModel RouteList { get; set; }



    }
}