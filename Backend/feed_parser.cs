using System;
using System.Net;
using System.Xml;
using System.Collections.Generic;

public class GeoPoint
{
	public double Latitude;
	public double Longitude;

	public GeoPoint(double Latitude, double Longitude)
	{
		this.Latitude = Latitude;
		this.Longitude = Longitude;
	}

	public double getDistance(GeoPoint to)
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

public class FeedParser
{
	public static void Main()
	{
		int route = 501;
		Dictionary<string, string> response = findClosestStop(new GeoPoint(43.659374, -79.382545), route);
		Console.WriteLine("The nearest bus stop on Route {0} is {1}. It is {2} away.", route, response["name"], response["distance"]);
		Console.WriteLine("tag={0} stopId={1}", response["tag"], response["stopId"]);
		List<string> directions = getDirectionNames(501);
		Console.WriteLine(directions[0]);
		Console.ReadLine();
	}

	public static Dictionary<string, string> findClosestStop(GeoPoint position, int route)
	{
		WebClient client = new WebClient();
		XmlDocument xml = new XmlDocument();
		xml.LoadXml(client.DownloadString("http://webservices.nextbus.com/service/publicXMLFeed?command=routeConfig&a=ttc&r=" + route));
		XmlNode parent = xml.FirstChild;
		XmlNodeList stops = xml.GetElementsByTagName("stop");
		Dictionary<string, string> response = new Dictionary<string, string>();

		if (stops.Count == 0)
		{
			response.Add("error", "Invalid Route");
			return response;
		}

		double bestDistance = position.getDistance(new GeoPoint(Convert.ToDouble(stops[0].Attributes["lat"].Value), Convert.ToDouble(stops[0].Attributes["lon"].Value)));
		XmlNode bestStop = stops[0];
		for (int i = 1; i < stops.Count; i++)
		{
			XmlNode stop = stops[i];
			if (stop.ParentNode.Name == "route")
			{
				double distance = position.getDistance(new GeoPoint(Convert.ToDouble(stops[i].Attributes["lat"].Value), Convert.ToDouble(stops[i].Attributes["lon"].Value)));
				if (distance < bestDistance)
				{
					bestDistance = distance;
					bestStop = stops[i];
				}
			}
		}

		response.Add("tag", bestStop.Attributes["tag"].Value);
		response.Add("name", bestStop.Attributes["title"].Value);
		response.Add("stopId", bestStop.Attributes["stopId"].Value);
		if (bestDistance > 1)
		{
			response.Add("distance", bestDistance.ToString("G3") + "km");
		}
		else
		{
			response.Add("distance", Convert.ToInt32(bestDistance*1000).ToString() + "m");
		}
		return response;
	}

	public static Dictionary<string, string> getNextBus(int route, int stop, string direction)
	{
		WebClient client = new WebClient();
		XmlDocument xml = new XmlDocument();
		xml.LoadXml(client.DownloadString("http://webservices.nextbus.com/service/publicXMLFeed?command=predictions&a=ttc&r=" + route + "&stopId=" + stop));
		XmlNode parent = xml.FirstChild;
		XmlNodeList predictions = xml.GetElementsByTagName("prediction");
		Dictionary<string, string> response = new Dictionary<string, string>();

		for (int i = 0; i < predictions.Count; i++)
		{
			if (predictions[i].ParentNode.Name == direction)
			{
				response.Add("time", predictions[i].Attributes["seconds"].Value);
				response.Add("vehicle", predictions[i].Attributes["vehicle"].Value);
				return response;
			}
		}

		response.Add("error", "No predictions available in that direction");
		return response;
	}

	public static List<string> getDirectionNames(int route)
	{
		WebClient client = new WebClient();
		XmlDocument xml = new XmlDocument();
		xml.LoadXml(client.DownloadString("http://webservices.nextbus.com/service/publicXMLFeed?command=routeConfig&a=ttc&r=" + route));
		XmlNode parent = xml.FirstChild;
		XmlNodeList directions = xml.GetElementsByTagName("direction");
		List<string> response = new List<string>();

		for (int i = 0; i < directions.Count; i++)
		{
			response.Add(directions[i].Attributes["title"].Value);
		}
		return response;
	}
}