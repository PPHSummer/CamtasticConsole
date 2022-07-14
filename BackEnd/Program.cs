using System;
using HtmlAgilityPack;
using System.Net;
using System.Linq;
using System.Collections.Generic;

public class Program
{
    public static void Main()
    {
        var document = new HtmlWeb().Load("https://photo-forum.net/");
        FindInformation(document);
    }
    internal static void FindInformation(HtmlAgilityPack.HtmlDocument document)
    {
        var ImageURLs = document.DocumentNode.Descendants("a").Select(e => e.GetAttributeValue("href", null)).Where(s => s.Any(stringToCheck => s.Contains("https://photo-forum.net/i/"))).ToList();
        
        for (int i = 0; i < 5; i++)
        {
            string pictureLink = ImageURLs[i];
            var web = new HtmlWeb();
            var documentChild = web.Load(pictureLink);
            List<string> rating = new List<string>();
            GetRatings(pictureLink, rating);
            List<string> cameraInfo = new List<string>();
            GetCameraInfo(pictureLink, cameraInfo);


            if (cameraInfo.Count >= 6)
            {
                PrintInformation(pictureLink, rating, cameraInfo);
            }
            else
            {
                PrintError(pictureLink);
            }
            Console.WriteLine("----------------------------");
        }
    }

    internal static void GetCameraInfo(string pictureLink, List<string> cameraInfo)
    {
        var web = new HtmlWeb();
        var documentChild = web.Load(pictureLink);
        var divs = documentChild.DocumentNode.SelectNodes("//span[@class='exif-info-data']");
        foreach (HtmlNode span in divs)
        {
            cameraInfo.Add(span.InnerText);
        }
    }
    internal static void GetRatings(string pictureLink, List<string> rating)
    {
        var web = new HtmlWeb();
        var documentChild = web.Load(pictureLink);
        var divs2 = documentChild.DocumentNode.SelectNodes("//span[@class='counter']");
        foreach (HtmlNode span in divs2)
        {
            rating.Add(span.InnerText.Trim());
        }
    }

    internal static void PrintInformation(string pictureLink, List<string> rating, List<string> cameraInfo)
    {
        Console.WriteLine($"This is a valid URL {pictureLink} with picture.\n With rating A: {rating[0]}. And camera brand {cameraInfo[0]}");

    }
    internal static void PrintError(string pictureLink)
    {
        Console.WriteLine($"A valid Url {pictureLink}, but no metadata.");
    }
}