using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using Microsoft.Maps.Unity;
using Microsoft.Geospatial;
using Newtonsoft.Json.Linq;


[System.Serializable]
public class GeoJSON
{
    public string type;
    public Feature[] features;
}

[System.Serializable]
public class Feature
{
    public string type;
    public Geometry geometry;
    public Properties properties;
}

[System.Serializable]
public class Geometry
{
    public string type;
    public double[] coordinates;
}

[System.Serializable]
public class Properties
{
    // Define properties matching your JSON structure here
    // For example:
    public string name;
    public string description;
    public int turnType;
    // ...
}


public class TmapRouteCalculator : MonoBehaviour
{
    [SerializeField] TMP_Text navigationText;
    [SerializeField] TMP_Text distanceText;
    [SerializeField] LocationModule locationModule;
    [SerializeField] MapPin mapPin;
    [SerializeField] GameObject arrow;
    [SerializeField] Camera arCamera;
    [SerializeField] GameObject blueCircle;
    [SerializeField] Transform mapByBing;
    [SerializeField] Transform mapByBingAR;

    private const string baseUrl = "https://apis.openapi.sk.com/tmap/routes/pedestrian";
    private const string apiKey = "MkWBdAMR859mRs2vFJthA9kWMnUilNTf76DNUNCk"; // Replace with your actual API key
    bool isGeoDataReady = false;
    GeoJSON geoData;
    int currentIdx = 0;
    double distanceNextPoint = 0, distanceNextNextPoint = 0;
    List<Feature> points;

    List<MapPin> circles;
    List<MapPin> circlesAR;

    private IEnumerator Start()
    {
        isGeoDataReady = false;
        points = new List<Feature>();
        circles = new();
        circlesAR = new();

        while (!locationModule.isLocationModuleReady)
        {
            Debug.Log("gogo");
            yield return new WaitForSecondsRealtime(1f);
        }
        // Define the request parameters
        StartCoroutine(Navigate());
    }

    public IEnumerator Navigate()
    {
        string startX = locationModule.longitude.ToString();
        string startY = locationModule.latitude.ToString();
        string endX = PlayerPrefs.GetFloat("longitude").ToString();
        string endY = PlayerPrefs.GetFloat("latitude").ToString();
        Debug.Log(endX + ", " + endY);
        string reqCoordType = "WGS84GEO";
        string resCoordType = "WGS84GEO";
        string startName = "출발지";
        string endName = "도착지";

        // Create the URL with query parameters
        string url = $"{baseUrl}?version=1&format=json" +
            $"&startX={startX}&startY={startY}&endX={endX}&endY={endY}" +
            $"&reqCoordType={reqCoordType}&resCoordType={resCoordType}" +
            $"&startName={startName}&endName={endName}";

        // Create a UnityWebRequest
        UnityWebRequest request = UnityWebRequest.Get(url);
        request.SetRequestHeader("appKey", apiKey);

        // Send the request
        yield return request.SendWebRequest();

        // Check for errors
        if (request.result != UnityWebRequest.Result.Success)
        {
            navigationText.text = "인터넷 연결 문제";
            Debug.LogError($"Error: {request.error}");
            yield break;
        }

        // Parse the JSON response
        string jsonResponse = request.downloadHandler.text;
        // Now you can parse the JSON response and extract the route information as needed.
        // You may need to create classes to deserialize the JSON response.

        // Example of deserializing the JSON (you'll need to create appropriate classes):
        // MyRouteData routeData = JsonUtility.FromJson<MyRouteData>(jsonResponse);
        // Debug.Log($"Total Distance: {routeData.totalDistance}");
        // Debug.Log($"Total Time: {routeData.totalTime}");
        //Debug.Log(jsonResponse);
        geoData = JsonUtility.FromJson<GeoJSON>(jsonResponse);
        isGeoDataReady = true;
        foreach (Feature feature in geoData.features)
        {
            if (feature.geometry.type.Equals("Point"))
                points.Add(feature);
        }

        JObject jObject = JObject.Parse(jsonResponse);
        var features = jObject["features"];
        List<Tuple<double, double>> coordinates = new List<Tuple<double, double>>();

        foreach (JObject obj in features)
        {
            if (obj["geometry"]["type"].ToString() == "LineString")
            {
                foreach (var coord in obj["geometry"]["coordinates"])
                {
                    coordinates.Add(new Tuple<double, double>(double.Parse(coord[0].ToString()), double.Parse(coord[1].ToString())));
                }
            }
        }

        foreach (Tuple<double, double> lonlat in coordinates)
        {
            GameObject obj = Instantiate(blueCircle, mapByBing);
            obj.transform.localScale = new Vector3(0.02f, 0.02f, 0.02f);
            obj.GetComponent<MapPin>().Location = new LatLon(lonlat.Item2, lonlat.Item1);
            GameObject objAR = Instantiate(blueCircle, mapByBingAR);
            objAR.transform.localScale = new Vector3(0.02f, 0.02f, 0.02f);
            objAR.GetComponent<MapPin>().Location = new LatLon(lonlat.Item2, lonlat.Item1);
            circles.Add(obj.GetComponent<MapPin>());
            circlesAR.Add(objAR.GetComponent<MapPin>());
        }

    }

    private void Update()
    {
        if (!isGeoDataReady)
            return;
        navigationText.text = points[currentIdx].properties.description;

        GPSData currGPS = new GPSData(locationModule.latitude, locationModule.longitude, 0);
        GPSData nextPoint = new GPSData(points[currentIdx + 1].geometry.coordinates[1], points[currentIdx + 1].geometry.coordinates[0], 0);
        GPSData nextNextPoint = new GPSData(points[currentIdx + 2].geometry.coordinates[1], points[currentIdx + 2].geometry.coordinates[0], 0);

        if (GPSUtils.CalculateDistance(currGPS, nextPoint) < 5 ||
            (GPSUtils.CalculateDistance(currGPS, nextPoint) - distanceNextPoint > 0 &&
            GPSUtils.CalculateDistance(currGPS, nextNextPoint) - distanceNextNextPoint < 0) &&
            GPSUtils.CalculateDistance(currGPS, nextPoint) < 20)
        {
            currentIdx++;
            navigationText.text = points[currentIdx].properties.description;
            //mapPin.Location = new LatLon(locationModule.latitude, locationModule.longitude);
        }

        distanceNextPoint = GPSUtils.CalculateDistance(currGPS, nextPoint);
        distanceNextNextPoint = GPSUtils.CalculateDistance(currGPS, nextNextPoint);

        //for (int i = 0; i < 10; i++)
        //{
        //    circles[i].Location = new LatLon(Mathf.Lerp((float)currGPS.latitude, (float)nextPoint.latitude, 10 * i/(float)distanceNextPoint), Mathf.Lerp((float)currGPS.longitude, (float)nextPoint.longitude, i/10.0f));
        //    circlesAR[i].Location = new LatLon(Mathf.Lerp((float)currGPS.latitude, (float)nextPoint.latitude, 10 * i/(float)distanceNextPoint), Mathf.Lerp((float)currGPS.longitude, (float)nextPoint.longitude, i/10.0f));
        //}

        mapPin.Location = new LatLon(points[currentIdx + 1].geometry.coordinates[1], points[currentIdx + 1].geometry.coordinates[0]);
        distanceText.text = distanceNextPoint.ToString("0.0") + "m";
    }
}
