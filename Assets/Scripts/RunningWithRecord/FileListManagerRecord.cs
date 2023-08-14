using System.IO;
using System.Xml;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
//using Qualcomm.Snapdragon.Spaces.Samples;
using System;
using Microsoft.Maps.Unity;
using Microsoft.Geospatial;
using System.Reflection;

public class FileListManagerRecord : MonoBehaviour
{
    [SerializeField] MapRenderer mapRenderer;
    [SerializeField] MapPin startMapPin;
    [SerializeField] MapPin endMapPin;
    [SerializeField] Transform content;
    [SerializeField] GameObject logPrefab;

    private List<List<string>> logDataList;
    private List<List<GPSData>> GPSDatasList;

    void Start()
    {
        PlayerPrefs.SetString("RecordFile", null);

        logDataList = new List<List<string>>();
        GPSDatasList = new List<List<GPSData>>();
        string path = Application.persistentDataPath + "/running_records";
        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);
        string[] fileList = Directory.GetFiles(path);

        for (int i = 0; i < fileList.Length; i++)
        {
            string filePath = fileList[i];
            int index = i;
            logDataList.Add(getMetadata(filePath));
            GPSDatasList.Add(GPXReader.ReadGPXFile(filePath));
            GameObject button = Instantiate(logPrefab, content);
            TMP_Text[] texts = button.GetComponentsInChildren<TMP_Text>();
            texts[0].text = logDataList[^1][2];
            texts[1].text = logDataList[^1][3];
            texts[2].text = logDataList[^1][4];
            button.GetComponent<Button>().onClick.AddListener(() =>
            {
                UpdateLogInfo(index);
            });
        }

        path = Application.persistentDataPath + "/running_logs";
        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);
        fileList = Directory.GetFiles(path);

        for (int i = 0; i < fileList.Length; i++)
        {
            string filePath = fileList[i];
            int index = i;
            logDataList.Add(getMetadata(filePath));
            GPSDatasList.Add(GPXReader.ReadGPXFile(filePath));
            GameObject button = Instantiate(logPrefab, content);
            TMP_Text[] texts = button.GetComponentsInChildren<TMP_Text>();
            texts[0].text = logDataList[^1][2];
            texts[1].text = logDataList[^1][3];
            texts[2].text = logDataList[^1][4];
            button.GetComponent<Button>().onClick.AddListener(() =>
            {
                UpdateLogInfo(index);
            });
        }

        if (fileList.Length > 0)
            UpdateLogInfo(0);
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
                                string.Format("{0}시간 {1}분 {2}초",
                                        (int)duration.TotalHours,              // Hours
                                        duration.Minutes,                      // Minutes
                                        duration.Seconds);

            stringList.Add(dateString);
        } catch
        {
            duration = new TimeSpan(0);
            stringList.Add(startTime.ToString("yyyy.MM.dd.ddd"));
        }

        // Get location from start and end Node

        float gpxDistance = GPXReader.getGPXDistance(gpsDataList);

        string distString = (gpxDistance / 1000.0).ToString("0.00") + "km";
        stringList.Add(distString);

        int paceTotalSeconds;
        if (gpxDistance != 0)
            paceTotalSeconds = (int) (duration.TotalSeconds / (gpxDistance / 1000.0));
        else
            paceTotalSeconds = 0;
        int paceMinutes = paceTotalSeconds / 60;
        int paceSeconds = paceTotalSeconds % 60;
        string paceString = "평균 페이스: " + paceMinutes.ToString() + "' " + paceSeconds.ToString() + "''";
        stringList.Add(paceString);

        return stringList;
    }

    public void UpdateLogInfo(int index)
    {
        PlayerPrefs.SetString("RecordFile", logDataList[index][0]);

        Image[] images = content.GetComponentsInChildren<Image>();

        foreach(Image image in images)
        {
            image.color = new Color(0, 0, 0, 0.3882f);
            TMP_Text[] innerTexts = image.GetComponentsInChildren<TMP_Text>();
            foreach (TMP_Text innerText in innerTexts)
                innerText.color = new Color(1, 1, 1);
        }
        images[index].color = new Color(0.9216f, 0.5647f, 0, 0.3882f);
        TMP_Text[] innerSelectedTexts = images[index].GetComponentsInChildren<TMP_Text>();
        foreach (TMP_Text innerText in innerSelectedTexts)
            innerText.color = new Color(0, 0, 0);



        try {
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
        } catch (Exception e)
        {
            Debug.Log(e.Message);
        }

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
