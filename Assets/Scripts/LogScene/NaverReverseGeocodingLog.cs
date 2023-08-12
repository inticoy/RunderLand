using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Networking;
using Geocoding;
using System.Threading;

public class NaverReverseGeocodingLog : MonoBehaviour
{
    private Root response;
    private int status_code = -1;


    private string url = "https://naveropenapi.apigw.ntruss.com/map-reversegeocode/v2/gc";

    public string GetLocationName(float latitude, float longitude)
    {
        StartCoroutine(GetReverseGeocode(latitude, longitude));
        int limit = 0;
        while (limit < 1000)
        {
            if (status_code == 0)
            {
                if (response.results[0].region.area3 == null)
                    return response.results[0].region.area2.name;
                else
                    return response.results[0].region.area3.name;
            }
            else if (status_code > 0)
                return "Unknown";
            limit++;
        }
        return "Unknown";
    }

    IEnumerator GetReverseGeocode(float latitude, float longitude)
    {
        url += "?coords=" + longitude.ToString() + "," + latitude.ToString();
        url += "&orders=legalcode&output=json";

        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            webRequest.SetRequestHeader("X-NCP-APIGW-API-KEY-ID", "wnapo1j7yy");
            webRequest.SetRequestHeader("X-NCP-APIGW-API-KEY", "WD2NPl4SffTzHK1G8vyLlHUjOtAm3jYQUKqpjPsa");

            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError ||
                webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error: " + webRequest.error);
                status_code = 1;
            }
            else
            {
                response = JsonUtility.FromJson<Root>(webRequest.downloadHandler.text);
                status_code = response.status.code;
            }
        }

    }
}

