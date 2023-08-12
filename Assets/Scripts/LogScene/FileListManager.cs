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

public class FileListManager : MonoBehaviour
{
    [SerializeField] Example07 scrollViewController;
    [SerializeField] TMP_Text nameText;
    [SerializeField] TMP_Text dateText;
    [SerializeField] TMP_Text locationText;
    [SerializeField] TMP_Text distanceText;
    [SerializeField] TMP_Text paceText;

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
        DateTime startTime = DateTime.Parse(startNode.SelectSingleNode("time").InnerText);
        DateTime endTime = DateTime.Parse(endNode.SelectSingleNode("time").InnerText);

        TimeSpan duration = endTime.Subtract(startTime);

        string dateString = startTime.ToString("yyyy.MM.dd.ddd") + " - " +
                            string.Format("{0}h {1}' {2}''",
                                    (int)duration.TotalHours,              // Hours
                                    duration.Minutes,                      // Minutes
                                    duration.Seconds);

        stringList.Add(dateString);

        try
        {
            string source = doc.SelectSingleNode("//source").InnerText;
            string destination = doc.SelectSingleNode("//destination").InnerText;

            stringList.Add(source + " -> " + destination);
        } catch (Exception e)
        {
            stringList.Add("Unknown");
        }

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
    }
}
