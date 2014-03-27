using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Diagnostics;
using Newtonsoft.Json;

namespace PhoneApp
{
    class FeedParser
    {
        private string agency;

        public FeedParser(string agency)
        {
            this.agency = agency;
        }

        private static async Task<string> DownloadString(string url)
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

            string json = "[ {\"tag\": \"" + routes[0].Attribute("tag").Value + "\", \"name\": \"" + routes[0].Attribute("title").Value + "\"}";
            for (int i = 1; i < routes.Count; i++)
            {
                json += ", {\"tag\": \"" + routes[i].Attribute("tag").Value + "\", \"name\": \"" + routes[i].Attribute("title").Value + "\"}";
            }
            json += "]";

            Debug.WriteLine(json);
            return json;
        }
    }
}
