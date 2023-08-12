using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Networking;
using Geocoding;

public class NaverReverseGeocodingLog : MonoBehaviour
{
    private Root response;
    private Root response2;
    private int status_code = -1;
    private int status_code2 = -1;

    string srcName, destName;

    public float srcLat, srcLon, destLat, destLon;

    [SerializeField] TMP_Text locationText;

    private string url = "https://naveropenapi.apigw.ntruss.com/map-reversegeocode/v2/gc";

    void Start()
    {
        srcLat = srcLon = destLat = destLon = 0;
        srcName = "";
        destName = "";
        StartCoroutine(GetSrcGeocode());
        StartCoroutine(GetDestGeocode());
    }

    void Update()
    {
        if (status_code == 0)
        {
            if (response.results[0].region.area3 != null)
                srcName = response.results[0].region.area3.ToString();
            else
                srcName = response.results[0].region.area2.ToString();
        }
        else
        {
            srcName = "Unknown";
        }

        string locText = srcName;

        if (status_code2 == 0)
        {
            if (response2.results[0].region.area3 != null)
                destName = response2.results[0].region.area3.ToString();
            else
                destName = response2.results[0].region.area2.ToString();
        }
        else
        {
            destName = "Unknown";
        }

        if (locText != "Unknown" && destName != "Unknown")
            locText += " -> " + destName;
        else if (destName != "Unknown")
            locText = destName;

        locationText.text = locText;
    }

    IEnumerator GetSrcGeocode()
    {
        if (srcLon == 0 && srcLat == 0)
            yield return new WaitForSecondsRealtime(1f);
        string srcurl = url + "?coords=" + srcLon.ToString() + "," + srcLat.ToString();
        srcurl += "&orders=legalcode&output=json";

        using (UnityWebRequest webRequest = UnityWebRequest.Get(srcurl))
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

    IEnumerator GetDestGeocode()
    {
       if (destLat == 0 && destLon == 0)
            yield return new WaitForSecondsRealtime(1f);
        string desturl = url + "?coords=" + destLon.ToString() + "," + destLat.ToString();
        desturl += "&orders=legalcode&output=json";
        using (UnityWebRequest webRequest = UnityWebRequest.Get(desturl))
        {
            webRequest.SetRequestHeader("X-NCP-APIGW-API-KEY-ID", "wnapo1j7yy");
            webRequest.SetRequestHeader("X-NCP-APIGW-API-KEY", "WD2NPl4SffTzHK1G8vyLlHUjOtAm3jYQUKqpjPsa");

            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError ||
                webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error: " + webRequest.error);
                status_code2 = 1;
            }
            else
            {
                response2 = JsonUtility.FromJson<Root>(webRequest.downloadHandler.text);
                status_code2 = response2.status.code;
                Debug.Log(response2.results[0].region.area2);
            }
        }
    }
}

