using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Networking;
using Weather;

namespace Weather
{
    [System.Serializable]
    public class Header
    {
        public string resultCode;
        public string resultMsg;
    }

    [System.Serializable]
    public class ForecastItem
    {
        public string baseDate;
        public string baseTime;
        public string category;
        public string fcstDate;
        public string fcstTime;
        public string fcstValue;
        public int nx;
        public int ny;
    }

    [System.Serializable]
    public class ForecastItems
    {
        public List<ForecastItem> item;
    }

    [System.Serializable]
    public class Body
    {
        public string dataType;
        public ForecastItems items;
        public int pageNo;
        public int numOfRows;
        public int totalCount;
    }

    [System.Serializable]
    public class Response
    {
        public Header header;
        public Body body;
    }

    [System.Serializable]
    public class WeatherData
    {
        public Response response;
    }
};



public class KmaWeather : MonoBehaviour
{
    [SerializeField]
    LocationModuleForMain location;

    [SerializeField]
    TMP_Text weatherTempText;

    [SerializeField]
    TMP_Text weatherMainText;

    [SerializeField]
    TMP_Text canvasWeatherTempText;

    [SerializeField]
    TMP_Text canvasWeatherMainText;

    public string temparature;
    public string weather;


    private string url = "http://apis.data.go.kr/1360000/VilageFcstInfoService_2.0/getUltraSrtFcst";
    public bool isReceived = false;

    public WeatherData response;   

    IEnumerator Start()
    {
        while (true)
        {
            if (location.isLocationModuleReady)
                break;
            yield return new WaitForSecondsRealtime(1f);
        }
        WgsToBaseStationCoord conv = new WgsToBaseStationCoord();
        LatXLonY latXlonY = conv.dfs_xy_conv(location.latitude, location.longitude);

        Debug.Log(getDate() + getTime());
        Debug.Log("lat: " + latXlonY.lat.ToString() + ", lon:" + latXlonY.lon.ToString());
        Debug.Log("x: " + latXlonY.x.ToString() + ", y:" + latXlonY.y.ToString());

        temparature = "0";
        weather = "날씨 없음";

        url += "?ServiceKey=vbmTlE%2BsLjBdCH3Y3IFYxG8aAetC7ML6Vu8nppsbK5Ot650NkTjxiZ3Xg18hfP8ZW4VWtageGek%2BougB5Qma5g%3D%3D";
        url += "&pageNo=1";
        url += "&numOfRows=1000";
        url += "&dataType=JSON";
        url += "&base_date=" + getDate();
        url += "&base_time=" + getTime();
        url += "&nx=" + latXlonY.x;
        url += "&ny=" + latXlonY.y;

        StartCoroutine(GetWeatherData());
    }

    IEnumerator GetWeatherData()
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError ||
                webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error: " + webRequest.error);
            }
            else
            {
                isReceived = true;
                response = JsonUtility.FromJson<WeatherData>(webRequest.downloadHandler.text);
                temparature = response.response.body.items.item[24].fcstValue;
                weather = getWeather(response);
            }
        }
    }

    void Update()
    {
        if (isReceived)
        {
            weatherTempText.text = temparature + "°C";
            weatherMainText.text = weather;

            canvasWeatherMainText.text = weather;
            canvasWeatherTempText.text = temparature + "°C";
        }
        else
        {
            weatherTempText.text = "날씨";
            weatherMainText.text = "로딩 중";

            canvasWeatherMainText.text = "날씨";
            canvasWeatherTempText.text = "로딩 중";
        }
    }

    string getWeather(WeatherData response)
    {
        // SKY
        string sky = response.response.body.items.item[18].fcstValue;
        // PTY
        string rain = response.response.body.items.item[6].fcstValue;

        // (초단기) 없음(0), 비(1), 비 / 눈(2), 눈(3), 빗방울(5), 빗방울눈날림(6), 눈날림(7)
        if (sky == "1") // sunny
        {
            if (rain == "0")
            {
                return "맑음";
            }
            else if (rain == "1" || rain == "2" || rain == "5" || rain == "6")
            {
                return "비";
            }
            else if (rain == "3" || rain == "7")
            {
                return "눈";
            }
        }
        else if (sky == "3")
        {
            if (rain == "0")
            {
                return "구름 많음";
            }
            else if (rain == "1" || rain == "2" || rain == "5" || rain == "6")
            {
                return "비";
            }
            else if (rain == "3" || rain == "7")
            {
                return "눈";
            }
        }
        else if (sky == "4")
        {
            if (rain == "0")
            {
                return "흐림";
            }
            else if (rain == "1" || rain == "2" || rain == "5" || rain == "6")
            {
                return "비";
            }
            else if (rain == "3" || rain == "7")
            {
                return "눈";
            }
        }
       return "날씨 오류";
    }

    string getDate()
    {
        int year = DateTime.Now.Year;
        int month = DateTime.Now.Month;
        int day = DateTime.Now.Day;
        int hour = DateTime.Now.Hour;
        int minute = DateTime.Now.Minute;

        if (hour == 0 && minute <= 40)
        {
            if (month == 1 && day == 1)
            {
                return (year - 1).ToString() + "1231";
            }
            else if (day == 1)
            {
                return year.ToString() + numToString(month - 1) + getLastDay(year, month);
            }
            else
            {
                return year.ToString() + numToString(month) + numToString(day - 1);
            }
        }
        else
        {
            return year.ToString() + numToString(month) + numToString(day);
        }
    }

    string getLastDay(int year, int month)
    {
        if (month == 1 || month == 3 || month == 5 ||
            month == 7 || month == 8 || month == 10 ||
            month == 12)
        {
            return "31";
        }
        else if (month == 4 || month == 6 || month == 9 ||
                month == 11)
        {
            return "30";
        }
        else // 2
        {
            if (year % 4 == 0)
            {
                if (year % 400 != 0 && year % 100 == 0)
                {
                    return "28";
                }
                else
                {
                    return "29";
                }
            }
            else
            {
                return "28";
            }
        }
    }

    string getTime()
    {
        int hour = DateTime.Now.Hour;
        int minute = DateTime.Now.Minute;

        if (minute > 40)
        {
            return numToString(hour) + "00";
        }
        else
        {
            if (hour == 0)
            {
                return "2300";
            }
            else
            {
                return numToString(hour - 1) + "00";
            }
        }
    }

    string numToString(int hour)
    {
        if (hour < 10)
        {
            return "0" + hour.ToString();
        }
        else
        {
            return hour.ToString();
        }
    }


    // Coord
    public struct lamc_parameter
    {
        public double Re;          /* 사용할 지구반경 [ km ]      */
        public double grid;        /* 격자간격        [ km ]      */
        public double slat1;       /* 표준위도        [degree]    */
        public double slat2;       /* 표준위도        [degree]    */
        public double olon;        /* 기준점의 경도   [degree]    */
        public double olat;        /* 기준점의 위도   [degree]    */
        public double xo;          /* 기준점의 X좌표  [격자거리]  */
        public double yo;          /* 기준점의 Y좌표  [격자거리]  */
    };

    public class WgsToBaseStationCoord
    {
        lamc_parameter map;

        public WgsToBaseStationCoord()
        {
            map.Re = 6371.00877;         // 지도반경
            map.grid = 5.0;              // 격자간격 (km)
            map.slat1 = 30.0;            // 표준위도 1
            map.slat2 = 60.0;            // 표준위도 2
            map.olon = 126.0;            // 기준점 경도
            map.olat = 38.0;             // 기준점 위도
            map.xo = 43;                 // 기준점 X좌표
            map.yo = 136;                // 기준점 Y좌표
        }

        public LatXLonY dfs_xy_conv(double _dLat, double _dLon)
        {
            double DEGARD = Math.PI / 180.0;
            //double RADDEG = 180.0 / Math.PI;

            double re = map.Re / map.grid;
            double slat1 = map.slat1 * DEGARD;
            double slat2 = map.slat2 * DEGARD;
            double olon = map.olon * DEGARD;
            double olat = map.olat * DEGARD;

            double sn = Math.Tan(Math.PI * 0.25 + slat2 * 0.5) / Math.Tan(Math.PI * 0.25 + slat1 * 0.5);
            sn = Math.Log(Math.Cos(slat1) / Math.Cos(slat2)) / Math.Log(sn);
            double sf = Math.Tan(Math.PI * 0.25 + slat1 * 0.5);
            sf = Math.Pow(sf, sn) * Math.Cos(slat1) / sn;
            double ro = Math.Tan(Math.PI * 0.25 + olat * 0.5);
            ro = re * sf / Math.Pow(ro, sn);

            LatXLonY rs = new LatXLonY();
            rs.lat = _dLat;
            rs.lon = _dLon;

            double ra = Math.Tan(Math.PI * 0.25 + _dLat * DEGARD * 0.5);
            ra = re * sf / Math.Pow(ra, sn);
            double theta = _dLon * DEGARD - olon;
            if (theta > Math.PI) theta -= 2.0 * Math.PI;
            if (theta < -Math.PI) theta += 2.0 * Math.PI;
            theta *= sn;
            rs.x = Math.Floor(ra * Math.Sin(theta) + map.xo + 0.5);
            rs.y = Math.Floor(ro - ra * Math.Cos(theta) + map.yo + 0.5);

            return rs;
        }
    }

    public class LatXLonY
    {
        public double lat;
        public double lon;

        public double x;
        public double y;
    }
}
