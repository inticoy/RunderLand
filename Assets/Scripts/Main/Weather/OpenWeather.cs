using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Networking;


[System.Serializable]
public class WeatherResponse
{
    public float lat;
    public float lon;
    public string timezone;
    public int timezone_offset;
    public CurrentWeather current;
}

[System.Serializable]
public class CurrentWeather
{
    public long dt;
    public long sunrise;
    public long sunset;
    public float temp;
    public float feels_like;
    public int pressure;
    public int humidity;
    public float dew_point;
    public float uvi;
    public int clouds;
    public int visibility;
    public float wind_speed;
    public int wind_deg;
    public float wind_gust;
    public WeatherData[] weather;
}

[System.Serializable]
public class WeatherData
{
    public int id;
    public string main;
    public string description;
    public string icon;
}

public class OpenWeather : MonoBehaviour
{
    [SerializeField]
    TMP_Text weatherMainText;

    [SerializeField]
    TMP_Text weatherTempText;

    public WeatherResponse weatherResponse;   

    void Start()
    {
        StartCoroutine(GetWeatherData());
    }

    IEnumerator GetWeatherData()
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get("https://api.openweathermap.org/data/3.0/onecall?lat=37.4885&lon=127.0655&exclude=minutely,hourly,daily,alerts&appid=a061a2b6642f63e0b5d3fee90ebb89c7&units=metric"))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError ||
                webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error: " + webRequest.error);
            }
            else
            {
                weatherResponse = JsonUtility.FromJson<WeatherResponse>(webRequest.downloadHandler.text);
                Debug.Log("Temperature in " + weatherResponse.current + ": " + weatherResponse.current.temp + "°C");
                Debug.Log("Weather : " + weatherResponse.current.weather[0].main);
            }
        }
    }

    void Update()
    {
        weatherMainText.text = weatherResponse.current.weather[0].main;
        weatherTempText.text = weatherResponse.current.temp.ToString() + "°C" ;
    }
}
