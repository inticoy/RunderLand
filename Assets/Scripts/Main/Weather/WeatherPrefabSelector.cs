using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherPrefabSelector : MonoBehaviour
{
    public KmaWeather kmaWeather;

    public GameObject sunny;
    public GameObject cloudy;
    public GameObject mostly_cloudy;
    public GameObject rain;
    public GameObject snow;
    public GameObject error;

    GameObject weatherObj;

    private bool isDrawn = false;

    // Start is called before the first frame update
    void Start()
    {
        weatherObj = Instantiate(error, transform.parent);
    }

    // Update is called once per frame
    void Update()
    {
        if (isDrawn == false)
        {
            if (kmaWeather.isReceived)
            {
                Destroy(weatherObj);
                isDrawn = true;

                if (kmaWeather.weather == "맑음")
                {
                    weatherObj = Instantiate(sunny, transform.parent);
                }
                else if (kmaWeather.weather == "구름 많음")
                {
                    weatherObj = Instantiate(mostly_cloudy, transform.parent);
                }
                else if (kmaWeather.weather == "흐림")
                {
                    weatherObj = Instantiate(cloudy, transform.parent);
                }
                else if (kmaWeather.weather == "비")
                {
                    weatherObj = Instantiate(rain, transform.parent);

                }
                else if (kmaWeather.weather == "눈")
                {
                    weatherObj = Instantiate(snow, transform.parent);
                }
                else
                {
                    weatherObj = Instantiate(error, transform.parent);
                }
            }
        }
    }
}
