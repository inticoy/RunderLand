using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using Microsoft.Maps.Unity;
using Microsoft.Geospatial;


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

    private const string baseUrl = "https://apis.openapi.sk.com/tmap/routes/pedestrian";
    private const string apiKey = "MkWBdAMR859mRs2vFJthA9kWMnUilNTf76DNUNCk"; // Replace with your actual API key
    bool isGeoDataReady = false;
    GeoJSON geoData;
    int currentIdx = 0;
    double distanceNextPoint = 0, distanceNextNextPoint = 0;
    List<Feature> points;
    Vector3[] turnVectors;

    private IEnumerator Start()
    {
        isGeoDataReady = false;
        points = new List<Feature>();
        turnVectors = new Vector3[234];

        //Initialize

        turnVectors[11] = new Vector3(0, 0, 0);
        turnVectors[12] = new Vector3(0, -90, 0);
        turnVectors[13] = new Vector3(0, 90, 0);
        turnVectors[14] = new Vector3(0, 180, 0);
        turnVectors[16] = new Vector3(0, -135, 0);
        turnVectors[17] = new Vector3(0, -45, 0);
        turnVectors[18] = new Vector3(0, 45, 0);
        turnVectors[19] = new Vector3(0, 135, 0);
        turnVectors[211] = new Vector3(0, 0, 0);
        turnVectors[212] = new Vector3(0, -90, 0);
        turnVectors[213] = new Vector3(0, 90, 0);
        turnVectors[214] = new Vector3(0, -135, 0);
        turnVectors[215] = new Vector3(0, -45, 0);
        turnVectors[216] = new Vector3(0, 45, 0);
        turnVectors[217] = new Vector3(0, 135, 0);

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
    }

    private void Update()
    {
        if (!isGeoDataReady)
            return;
        navigationText.text = geoData.features[currentIdx].properties.description;

        GPSData currGPS = new GPSData(locationModule.latitude, locationModule.longitude, 0);
        GPSData nextPoint = new GPSData(points[currentIdx + 1].geometry.coordinates[1], points[currentIdx + 1].geometry.coordinates[0], 0);
        GPSData nextNextPoint = new GPSData(points[currentIdx + 2].geometry.coordinates[1], points[currentIdx + 2].geometry.coordinates[0], 0);

        if (GPSUtils.CalculateDistance(currGPS, nextPoint) < 5 ||
            (GPSUtils.CalculateDistance(currGPS, nextPoint) - distanceNextPoint > 0 &&
            GPSUtils.CalculateDistance(currGPS, nextNextPoint) - distanceNextNextPoint < 0))
        {
            currentIdx++;
            navigationText.text = points[currentIdx].properties.description;
            //mapPin.Location = new LatLon(locationModule.latitude, locationModule.longitude);
        }

        distanceNextPoint = GPSUtils.CalculateDistance(currGPS, nextPoint);
        distanceNextNextPoint = GPSUtils.CalculateDistance(currGPS, nextNextPoint);

        if (distanceNextPoint < 15)
        {
            arrow.SetActive(true);
            arrow.transform.position = arCamera.transform.position + locationModule.GetDirectionVector() * (float)distanceNextPoint;
            arrow.transform.rotation = Quaternion.Euler(turnVectors[points[currentIdx + 1].properties.turnType]);
        }
        else
            arrow.SetActive(false);

        mapPin.Location = new LatLon(points[currentIdx + 1].geometry.coordinates[1], points[currentIdx + 1].geometry.coordinates[0]);
        distanceText.text = distanceNextPoint.ToString("0.0") + "m";
    }
}
