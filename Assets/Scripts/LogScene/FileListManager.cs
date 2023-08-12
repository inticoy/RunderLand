using System.IO;
using System.Xml;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
//using Qualcomm.Snapdragon.Spaces.Samples;
using System;
using FancyScrollView.Example07;
using Microsoft.Maps.Unity;
using Microsoft.Geospatial;
using System.Security.Cryptography.X509Certificates;
using System.Reflection;

public class FileListManager : MonoBehaviour
{
    [SerializeField] Example07 scrollViewController;
    [SerializeField] TMP_Text nameText;
    [SerializeField] TMP_Text dateText;
    [SerializeField] TMP_Text locationText;
    [SerializeField] TMP_Text distanceText;
    [SerializeField] TMP_Text paceText;
    [SerializeField] MapRenderer mapRenderer;
    [SerializeField] MapPin startMapPin;
    [SerializeField] MapPin endMapPin;
    [SerializeField] NaverReverseGeocodingLog reverseGeocoding;

    private List<List<string>> logDataList;
    private List<List<GPSData>> GPSDatasList;

    void Start()
    {
        string path = Application.persistentDataPath + "/running_logs";
        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);
        string[] fileList = Directory.GetFiles(path);
        logDataList = new List<List<string>>();
        GPSDatasList = new List<List<GPSData>>();

        Debug.Log(path);
        Debug.Log(fileList.Length.ToString());
        foreach (string filePath in fileList)
        {
            Debug.Log("file: " + Path.GetFileName(filePath));
            logDataList.Add(getMetadata(filePath));
            GPSDatasList.Add(GPXReader.ReadGPXFile(filePath));
        }
        scrollViewController.GenerateCells(logDataList);
    }

    List<string> getMetadata(string filePath)
    {
        List<string> stringList = new List<string>();

        stringList.Add(filePath);

        XmlDocument doc = new XmlDocument();
        doc.Load(filePath);

        XmlNode metadata = doc.SelectSingleNode("//metadata");

        stringList.Add(metadata.SelectSingleNode("name").InnerText);

        // Select Track Points
        XmlNodeList trkPoints = doc.SelectNodes("//trkpt");

        List<GPSData> gpsDataList = new List<GPSData>();

        foreach (XmlNode trackPoint in trkPoints)
        {
            double latitude = double.Parse(trackPoint.Attributes["lat"].Value);
            double longitude = double.Parse(trackPoint.Attributes["lon"].Value);
            double altitude = double.Parse(trackPoint.SelectSingleNode("ele").InnerText);

            GPSData gpsData = new GPSData(latitude, longitude, altitude);
            gpsDataList.Add(gpsData);
        }

        // Get start and end Node of Track Points
        XmlNode startNode = trkPoints[0];
        XmlNode endNode = trkPoints[trkPoints.Count - 1];

        // Get time from start and end Node
        TimeSpan duration;

        DateTime startTime = DateTime.Parse(metadata.SelectSingleNode("time").InnerText);
        try
        {
            DateTime endTime = DateTime.Parse(endNode.SelectSingleNode("time").InnerText);

            duration = endTime.Subtract(startTime);

            string dateString = startTime.ToString("yyyy.MM.dd.ddd") + " - " +
                                string.Format("{0}h {1}' {2}''",
                                        (int)duration.TotalHours,              // Hours
                                        duration.Minutes,                      // Minutes
                                        duration.Seconds);

            stringList.Add(dateString);
        } catch
        {
            duration = new TimeSpan(0);
            stringList.Add(startTime.ToString("yyyy.MM.dd.ddd"));
        }

        try
        {
            string source = doc.SelectSingleNode("//source").InnerText;
            string destination = doc.SelectSingleNode("//destination").InnerText;

            stringList.Add(source + " -> " + destination);
        }
        catch (Exception e)
        {
            stringList.Add("Unknown");
        }

        //string sourceName, destinationName;

        //try {
        //    XmlNode sourceNode = doc.SelectSingleNode("//source");
        //    if (sourceNode == null)
        //    {
        //        sourceName = reverseGeocoding.GetLocationName(
        //            float.Parse(startNode.Attributes["lat"].Value),
        //            float.Parse(startNode.Attributes["lon"].Value));
        //    }
        //    else
        //        sourceName = sourceNode.InnerText;

        //    XmlNode destinationNode = doc.SelectSingleNode("//destination");
        //    if (destinationNode == null)
        //    {
        //        destinationName = "hi";
        //        //destinationName = reverseGeocoding.GetLocationName(
        //        //    float.Parse(endNode.Attributes["lat"].Value),
        //        //    float.Parse(endNode.Attributes["lon"].Value));
        //    }
        //    else
        //        destinationName = destinationNode.InnerText;

        //    //if (sourceName == "Unknown" && destinationName == "Unknown")
        //    //    stringList.Add("Unknown");
        //    //else if (sourceName == "Unknown")
        //    //    stringList.Add(destinationName);
        //    //else if (destinationName == "Unknown")
        //    //    stringList.Add(sourceName);
        //    //else
        //        stringList.Add(sourceName + " -> " + destinationName);
        //} catch
        //{
        //    stringList.Add("Unknown");
        //}

        // Get location from start and end Node

        float gpxDistance = GPXReader.getGPXDistance(gpsDataList);

        string distString = (gpxDistance / 1000.0).ToString() + "km";
        stringList.Add(distString);

        int paceTotalSeconds;
        if (gpxDistance != 0)
            paceTotalSeconds = (int) (duration.TotalSeconds / (gpxDistance / 1000.0));
        else
            paceTotalSeconds = 0;
        int paceMinutes = paceTotalSeconds / 60;
        int paceSeconds = paceTotalSeconds % 60;
        string paceString = "Avg. " + paceMinutes.ToString() + "' " + paceSeconds.ToString() + "''";
        stringList.Add(paceString);

        return stringList;
    }

    public void UpdateLogInfo(int index)
    {
        nameText.text = logDataList[index][1];
        dateText.text = logDataList[index][2];
        locationText.text = logDataList[index][3];
        distanceText.text = logDataList[index][4];
        paceText.text = logDataList[index][5];

        int lastIdx = 0;
        if (GPSDatasList[index].Count > 0)
        {
            lastIdx = GPSDatasList[index].Count - 1;
        }
        double distance = GPSUtils.CalculateDistance(GPSDatasList[index][0], GPSDatasList[index][lastIdx]);
        LatLon middle = new LatLon((GPSDatasList[index][0].latitude + GPSDatasList[index][lastIdx].latitude) / 2,
                                   (GPSDatasList[index][0].longitude + GPSDatasList[index][lastIdx].longitude) / 2);

        //mapRenderer.Center = new LatLon(37.4885, 127.0655);
        //startMapPin.Location = new LatLon(37.48, 127.06);
        //endMapPin.Location = new LatLon(37.49, 127.07);

        mapRenderer.Center = middle;
        startMapPin.Location = new LatLon(GPSDatasList[index][0].latitude, GPSDatasList[index][0].longitude);
        endMapPin.Location = new LatLon(GPSDatasList[index][lastIdx].latitude, GPSDatasList[index][lastIdx].longitude);

        StartCoroutine(AnimateMap(GPSDatasList[index], 17.0f, 17.0f, 5.0f));

    }

    private IEnumerator AnimateMap(List<GPSData> gpsDatas, float startScale, float endScale, float duration)
    {
        float elapsedTime = 0.0f;
        int lastIdx = 0;
        if (gpsDatas.Count > 0)
        {
            lastIdx = gpsDatas.Count - 1;
        }
        Vector2 startLocation = new Vector2((float)gpsDatas[0].latitude, (float)gpsDatas[0].longitude);
        Vector2 endLocation = new Vector2((float)gpsDatas[lastIdx].latitude, (float)gpsDatas[lastIdx].longitude);

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration; // Normalized time (0 to 1)

            // Interpolate positions and scale
            Vector2 interpolatedLocation = Vector2.Lerp(startLocation, endLocation, t);
            float interpolatedScale = Mathf.Lerp(startScale, endScale, t);

            // Update the map's position and scale
            mapRenderer.Center = new LatLon(interpolatedLocation.x, interpolatedLocation.y);
            mapRenderer.ZoomLevel = interpolatedScale;

            yield return null; // Wait for the next frame
            elapsedTime += Time.deltaTime;
        }

        // Ensure the final values are set exactly
        mapRenderer.Center = new LatLon(endLocation.x, endLocation.y);
        mapRenderer.ZoomLevel = endScale;
    }
}
