using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;
using System.Xml;
using System;

public class GPXLogger : MonoBehaviour
{
    // File path of the existing GPX file
    private string gpxFilePath;
    private string fileName = "log1.gpx";

    [SerializeField] NaverReverseGeocodingLog reverseGeocoding;

    // Time interval between each GPS update
    public float updateInterval = 1f;

    // LocationModule to get gps information
    public LocationModule LocationModule;

    void Start()
    {
        // Continuously log GPS data
        string path = Application.persistentDataPath + "/running_logs";
        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);
        gpxFilePath = Path.Combine(path, fileName);
        int suffix = 1;
        while (System.IO.File.Exists(gpxFilePath))
        {
            fileName = $"log{suffix}.gpx";
            gpxFilePath = Path.Combine(path, fileName);
            suffix++;
        }
        double latitude = LocationModule.GetComponent<LocationModule>().latitude;
        double longitude = LocationModule.GetComponent<LocationModule>().longitude;
        double altitude = LocationModule.GetComponent<LocationModule>().altitude;
        CreateGPXFile(latitude, longitude, altitude);
    }

    IEnumerator WriteDataToFile()
    {
        double latitude = LocationModule.GetComponent<LocationModule>().latitude;
        double longitude = LocationModule.GetComponent<LocationModule>().longitude;
        double altitude = LocationModule.GetComponent<LocationModule>().altitude;


        while (true)
        {
            latitude = LocationModule.GetComponent<LocationModule>().latitude;
            longitude = LocationModule.GetComponent<LocationModule>().longitude;
            altitude = LocationModule.GetComponent<LocationModule>().altitude;
            AppendTrackPointToGPXFile(latitude, longitude, altitude);
            yield return new WaitForSeconds(updateInterval);
        }
    }

    private void CreateGPXFile(double latitude, double longitude, double altitude)
    {
        XmlDocument doc = new XmlDocument();
        XmlElement root = doc.CreateElement("gpx");
        doc.AppendChild(root);


        XmlElement metadata = doc.CreateElement("metadata");
        XmlElement name = doc.CreateElement("name");
        name.InnerText = "Junseo Kim";
        metadata.AppendChild(name);
        XmlElement metaTime = doc.CreateElement("time");
        metaTime.InnerText = DateTime.Now.ToString("s") + "Z";
        metadata.AppendChild(metaTime);

        XmlElement extensions = doc.CreateElement("extensions");
        XmlElement locInfo = doc.CreateElement("locationinfo");

        extensions.AppendChild(locInfo);
        metadata.AppendChild(extensions);

        root.AppendChild(metadata);

        XmlElement trk = doc.CreateElement("trk");
        root.AppendChild(trk);

        XmlElement trkseg = doc.CreateElement("trkseg");
        trk.AppendChild(trkseg);

        //XmlElement trkpt = doc.CreateElement("trkpt");
        //trkpt.SetAttribute("lat", latitude.ToString());
        //trkpt.SetAttribute("lon", longitude.ToString());
        //trkseg.AppendChild(trkpt);

        //XmlElement ele = doc.CreateElement("ele");
        //ele.InnerText = altitude.ToString();
        //XmlElement time = doc.CreateElement("time");
        //time.InnerText = DateTime.Now.ToString("s") + "Z";
        //trkpt.AppendChild(ele);
        //trkpt.AppendChild(time);

        

        // Save the GPX file
        doc.Save(gpxFilePath);
    }

    public void AppendTrackPointToGPXFile(double latitude, double longitude, double altitude)
    {
        if (latitude == 0 && longitude == 0 && altitude == 0)
            return;

        XmlDocument doc = new XmlDocument();
        doc.Load(gpxFilePath);

        XmlNode trackSegment = doc.SelectSingleNode("//trkseg");

        if (doc.SelectSingleNode("//trkpt") == null)
        {
            XmlNode locInfoNode = doc.SelectSingleNode("//locationinfo");

            XmlElement source = doc.CreateElement("source");
            source.InnerText = reverseGeocoding.GetLocationName((float)latitude, (float)longitude);
            locInfoNode.AppendChild(source);
        }

        XmlElement trackPoint = doc.CreateElement("trkpt");
        trackPoint.SetAttribute("lat", latitude.ToString());
        trackPoint.SetAttribute("lon", longitude.ToString());

        XmlElement elevation = doc.CreateElement("ele");
        elevation.InnerText = altitude.ToString();
        XmlElement time = doc.CreateElement("time");
        time.InnerText = DateTime.Now.ToString("s") + "Z";

        trackPoint.AppendChild(elevation);
        trackPoint.AppendChild(time);
        trackSegment.AppendChild(trackPoint);

        doc.Save(gpxFilePath);
    }


    private void OnDestroy()
    {
        XmlDocument doc = new XmlDocument();
        doc.Load(gpxFilePath);

        try
        {
            XmlNodeList trkpts = doc.SelectNodes("//trkpt");
            XmlNode endtrkpt = trkpts[trkpts.Count - 1];

            double latitude = double.Parse(endtrkpt.Attributes["lat"].Value);
            double longitude = double.Parse(endtrkpt.Attributes["lon"].Value);

            XmlNode locInfoNode = doc.SelectSingleNode("//locationinfo");

            XmlElement destination = doc.CreateElement("destination");
            destination.InnerText = reverseGeocoding.GetLocationName((float)latitude, (float)longitude);
            locInfoNode.AppendChild(destination);
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
        // Stop GPS
        Input.location.Stop();
    }
}
